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
    public virtual bool OpenConnection()
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
}