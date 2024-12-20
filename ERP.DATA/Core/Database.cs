using System.Data;
using System.Data.Common;
using System.Reflection;

namespace ERP.DATA.Core;

public abstract class Database
{
    /// <summary>
    /// Connection builder
    /// </summary>
    protected ConnectionBuilder Builder { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="builder"></param>
    protected Database(ConnectionBuilder builder)
    {
        Builder = builder;
    }

    /// <summary>
    /// Get entities
    /// </summary>
    /// <param name="where">Condition clause</param>
    /// <param name="orderBy">Ordering method</param>
    /// <typeparam name="TE">Entity type</typeparam>
    /// <returns>List of entities</returns>
    public abstract List<TE> GetEntities<TE>(string where, string? orderBy = null) where TE : Entity, new();

    /// <summary>
    /// Count entities
    /// </summary>
    /// <param name="where">Condition clause</param>
    /// <typeparam name="TE">Entity type</typeparam>
    /// <returns>Entity count</returns>
    public abstract int? CountEntities<TE>(string where) where TE : Entity, new();

    /// <summary>
    /// Get one entity
    /// </summary>
    /// <param name="where">Condition clause</param>
    /// <param name="orderBy">Ordering clause</param>
    /// <typeparam name="TE">Entity type</typeparam>
    /// <returns>Single entity</returns>
    public abstract TE GetEntity<TE>(string where, string? orderBy = null) where TE : Entity, new();


    /// <summary>
    /// Execute a non-query
    /// </summary>
    /// <param name="query">Query</param>
    /// <exception cref="DataException"></exception>
    public abstract void ExecuteNonQuery(string query);

    /// <summary>
    /// Execute a scalar query
    /// </summary>
    /// <param name="query">Query</param>
    /// <param name="parser">Parsing method</param>
    /// <returns>Integer result</returns>
    /// <exception cref="DataException"></exception>
    public abstract T ExecuteScalar<T>(string query, Func<object?, T> parser);

    /// <summary>
    /// Execute a reader query
    /// </summary>
    /// <param name="query">Query</param>
    /// <param name="parser">Parsing method</param>
    /// <typeparam name="T">Data type</typeparam>
    /// <returns>Result</returns>
    public abstract List<T> ExecuteReader<T>(string query, Func<Database, DbDataReader, T> parser);

     /// <summary>
    /// Get string value
    /// </summary>
    /// <param name="reader">Reader</param>
    /// <param name="columnName">Column name</param>
    /// <typeparam name="TR">Reader type</typeparam>
    /// <returns>Value</returns>
    public abstract string? GetStringValue<TR>(TR reader, string columnName) where TR : DbDataReader;
    /// <summary>
    /// Set string value
    /// </summary>
    /// <param name="columnName">Column name</param>
    /// <param name="content">Content</param>
    /// <returns>Value</returns>
    public abstract string SetStringValue(string columnName, string? content);

    /// <summary>
    /// Get int value
    /// </summary>
    /// <param name="reader">Reader</param>
    /// <param name="columnName">Column name</param>
    /// <typeparam name="TR">Reader type</typeparam>
    /// <returns>Value</returns>
    public abstract int? GetIntValue<TR>(TR reader, string columnName) where TR : DbDataReader;
    /// <summary>
    /// Get int value
    /// </summary>
    /// <param name="columnName">Column name</param>
    /// <param name="content">Content</param>
    /// <returns>Value</returns>
    public abstract string SetIntValue(string columnName, int? content);

    /// <summary>
    /// Parse an integer
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed integer or null</returns>
    public abstract int? ParseInt(object? value);

    /// <summary>
    /// Get int value
    /// </summary>
    /// <param name="reader">Reader</param>
    /// <param name="columnName">Column name</param>
    /// <typeparam name="TR">Reader type</typeparam>
    /// <returns>Value</returns>
    public abstract DateTime? GetDateTimeValue<TR>(TR reader, string columnName) where TR : DbDataReader;
    /// <summary>
    /// Get int value
    /// </summary>
    /// <param name="columnName">Column name</param>
    /// <param name="content">Content</param>
    /// <returns>Value</returns>
    public abstract string SetDateTimeValue(string columnName, DateTime? content);



    public abstract void HandleException(Exception ex, string title);
}

public abstract class Database<TConn, TCom, TR> : Database
    where TConn : DbConnection
    where TCom : DbCommand
    where TR : DbDataReader
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
    protected abstract TConn GetConnection();

    /// <summary>
    /// Get command
    /// </summary>
    /// <param name="connection">Connection</param>
    /// <param name="query">Query</param>
    /// <returns></returns>
    protected abstract TCom GetCommand(TConn connection, string query);

    /// <summary>
    /// Try to open connection
    /// </summary>
    /// <returns>True if connection is successful, false otherwise</returns>
    public virtual void TryConnection()
    {
        ExecuteNonQuery(Builder.DefaultQuery);
    }

    public virtual TE Parser<TE>(Database db, DbDataReader reader) where TE : Entity, new()
    {
        var entity = new TE();

        if (entity.ParseEntity(db, reader))
        {
            return entity;
        }

        throw new DataException($"Parsing failed for entity : {entity.TableName}");
    }

    public override List<TE> GetEntities<TE>(string where, string? orderBy = null)
    {
        var entity = new TE();

        string query = $"SELECT ({entity.GetColumnNames()}) FROM {entity.TableName}";

        if ("" != where)
        {
            query += $" WHERE {where}";
        }

        if (null != orderBy)
        {
            query += $" ORDER BY {orderBy}";
        }

        return ExecuteReader(query, Parser<TE>);
    }

    public override int? CountEntities<TE>(string where)
    {
        var entity = new TE();

        string query = $"SELECT COUNT(*) FROM {entity.TableName}";

        if ("" != where)
        {
            query += $" WHERE {where}";
        }

        return ExecuteScalar(query, ParseInt);
    }


    public override TE GetEntity<TE>(string where, string? orderBy = null)
    {
        var result = GetEntities<TE>(where, orderBy);

        if (null == result || result.Count == 0)
        {
            throw new DataException($"No entity found when querying : {where}");
        }

        if (result.Count > 1)
        {
            throw new DataException($"Too many entities found when querying : {where}");
        }

        return result.First();
    }

    /// <summary>
    /// Execute a non-query
    /// </summary>
    /// <param name="query">Query</param>
    /// <exception cref="DataException"></exception>
    public override void ExecuteNonQuery(string query)
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
            HandleException(ex, MethodBase.GetCurrentMethod()!.Name);
        }
    }

    /// <summary>
    /// Execute a scalar query
    /// </summary>
    /// <param name="query">Query</param>
    /// <param name="parser">Parsing method</param>
    /// <returns>Integer result</returns>
    /// <exception cref="DataException"></exception>
    public override T ExecuteScalar<T>(string query, Func<object?, T> parser)
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
                    command.ExecuteScalar();
                    result = parser(this);

                    if (null == result)
                    {
                        throw new DataException("Query failed");
                    }
                    return result;
                }
            }
        }
        catch (DbException ex)
        {
            HandleException(ex, MethodBase.GetCurrentMethod()!.Name);
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
    public override List<T> ExecuteReader<T>(string query, Func<Database, DbDataReader, T> parser)
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
            HandleException(ex, MethodBase.GetCurrentMethod()!.Name);
        }
        catch (Exception ex)
        {
            HandleException(new Exception(ex.Message), MethodBase.GetCurrentMethod()!.Name +" failed at parsing");
        }

        return result;
    }

    public override void HandleException(Exception ex, string title)
    {
        Console.WriteLine(title + ": " + ex.Message);
    }
}