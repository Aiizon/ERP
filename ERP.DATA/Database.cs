using System.Data;
using System.Data.Common;

namespace ERP.DATA;

public abstract class Database
{
    /// <summary>
    /// Connection builder
    /// </summary>
    internal ConnectionBuilder Builder { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="builder"></param>
    public Database(ConnectionBuilder builder)
    {
        Builder = builder;
    }
}

public abstract class Database<TConn,TCom> : Database
    where TConn : DbConnection
    where TCom : DbCommand
{

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="builder"></param>
    protected Database(ConnectionBuilder builder) : base(builder) { }

    /// <summary>
    /// Get connection
    /// </summary>
    /// <returns></returns>
    public abstract TConn GetConnection();

    /// <summary>
    /// Get command
    /// </summary>
    /// <param name="connection">Connection</param>
    /// <param name="query">Query</param>
    /// <returns></returns>
    public abstract TCom GetCommand(TConn connection, string query);

    /// <summary>
    /// Try to open connection
    /// </summary>
    /// <returns>True if connection is successful, false otherwise</returns>
    public virtual bool TryConnection()
    {
        try
        {
            // Open connection and execute the default query to test it
            using (TConn connection = GetConnection())
            {
                connection.Open();
                using (TCom command = GetCommand(connection, Builder.DefaultQuery))
                {
                    var result = command.ExecuteNonQuery();

                    if (result < 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Execute a non-query
    /// </summary>
    /// <param name="query">Query</param>
    /// <exception cref="DataException"></exception>
    public virtual void ExecuteNonQuery(string query)
    {
        try
        {
            // Open connection and execute the query
            using (TConn connection = GetConnection())
            {
                connection.Open();
                using (TCom command = GetCommand(connection, query))
                {
                    var result = command.ExecuteNonQuery();

                    if (result < 0)
                    {
                        throw new DataException("Query failed");
                    }
                }
            }
        }
        catch (DbException ex)
        {
            HandleException(ex, "Database.ExecuteNonQuery");
        }
    }

    /// <summary>
    /// Execute a scalar query
    /// </summary>
    /// <param name="query">Query</param>
    /// <param name="parser">Parsing method</param>
    /// <returns>Integer result</returns>
    /// <exception cref="DataException"></exception>
    public virtual T ExecuteScalar<T>(string query, Func<Database, T> parser)
    {
        T result = default(T)!;
        try
        {
            // Open connection, execute the query and return its result
            using (TConn connection = GetConnection())
            {
                connection.Open();
                using (TCom command = GetCommand(connection, query))
                {
                    var tmp = command.ExecuteScalar();
                    result = parser(this);

                    if (result == null)
                    {
                        throw new DataException("Query failed");
                    }
                    return result;
                }
            }
        }
        catch (DbException ex)
        {
            HandleException(ex, "Database.ExecuteScalar");
        }

        return result;
    }

    /// <summary>
    /// Execute a reader query
    /// </summary>
    /// <param name="query">Query</param>
    /// <param name="parser">Parsing method</param>
    /// <typeparam name="T">Data type</typeparam>
    /// <returns>Result</returns>
    public virtual List<T> ExecuteReader<T>(string query, Func<Database, DbDataReader, T> parser)
    {
        List<T> result = new List<T>();

        try
        {
            // Open connection, execute the query and parse the result using the parser
            using (TConn connection = GetConnection())
            {
                connection.Open();
                using (TCom command = GetCommand(connection, query))
                {
                    DbDataReader reader = command.ExecuteReader();

                    // Read result to parse it, throw exception if parsing fails at any point
                    while (reader.Read())
                    {
                        var item = parser(this, reader);
                        if (null == item)
                        {
                            throw new DataException("Parser failed");
                        }
                        result.Add(item);
                    }
                }
            }
        }
        catch (DbException ex)
        {
            HandleException(ex, "Database.ExecuteReader");
        }
        catch (Exception ex)
        {
            HandleException(new Exception(ex.Message), "Database.ExecuteReader failed at parsing");
        }

        return result;
    }

    public static void HandleException(Exception ex, string title)
    {

    }
}