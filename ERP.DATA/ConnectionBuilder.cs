using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace ERP.DATA;

public class ConnectionBuilder
{
    /// <summary>
    /// IP address or server name
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// Database type (e.g. SQL Server, MySQL, etc.)
    /// </summary>
    public string Database { get; set; }

    /// <summary>
    /// Database user name
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// Database password @todo: stop storing password in plain text
    /// </summary>
    private string Password { get; set; }

    /// <summary>
    /// Timeout for database connection (in seconds)
    /// </summary>
    public int Timeout { get; set; }

    /// <summary>
    /// Set to true if you want to use LDAP credentials
    /// </summary>
    public bool DoUseCredentials { get; set; }

    public string DefaultQuery { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="server">IP address or server name</param>
    /// <param name="database">Database type</param>
    /// <param name="defaultQuery">Default query used to try connection</param>
    public ConnectionBuilder
    (
        string server,
        string database,
        string defaultQuery
    )
    {
        Server = server;
        Database = database;
        User = string.Empty;
        Password = string.Empty;
        Timeout = 30;
        DoUseCredentials = false;
        DefaultQuery = defaultQuery;
    }

    /// <summary>
    /// Returns the connection string in SQL Server format
    /// </summary>
    /// <returns></returns>
    public string ToSQLConnectionString()
    {
        if (DoUseCredentials)
        {
            return $"Server={Server};Database={Database};Trusted_Connection=True;Connection Timeout={Timeout};";
        }
        return $"Server={Server};Database={Database};User Id={User};Password={Password};Connection Timeout={Timeout};";
    }

    /// <summary>
    /// Returns the database connection object for SQL Server
    /// </summary>
    /// <returns></returns>
    public SqlConnection ToSqlConnection()
    {
        return new SqlConnection(ToSQLConnectionString());
    }

    /// <summary>
    /// Returns the connection string in MySQL format
    /// </summary>
    /// <returns></returns>
    public string ToMysqlConnectionString()
    {
        return $"Server={Server};Database={Database};Uid={User};Pwd={Password};";
    }

    /// <summary>
    /// Returns the database connection object for MySQL
    /// </summary>
    /// <returns></returns>
    public MySqlConnection ToMySqlConnection()
    {
        return new MySqlConnection(ToMysqlConnectionString());
    }

    /// <summary>
    /// Returns the connection string in ODBC format
    /// </summary>
    /// <returns></returns>
    public string ToOdbcConnectionString()
    {
        return $"Driver={{SQL Server}};Server={Server};Database={Database};Uid={User};Pwd={Password};";
    }

    /// <summary>
    /// Returns the appropriate connection object based on the database type
    /// </summary>
    /// <returns></returns>
    public DbConnection? GetConnection()
    {
        switch (Database)
        {
            case "SQL Server":
                return ToSqlConnection();
            case "MySQL":
                return ToMySqlConnection();
            default:
                return null;
        }
    }
    
    // /// <summary>
    // /// Returns the appropriate connection object based on the database type
    // /// </summary>
    // /// <returns></returns>
    // public TConn GetConnection<TConn>() where TConn : DbConnection, new()
    // {
    //     var conn = new TConn();
    //     switch (conn)
    //     {
    //         case SqlConnection:
    //             return (TConn)(object)ToSqlConnection();
    //         case MySqlConnection:
    //             return (TConn)(object)ToMySqlConnection();
    //         default:
    //             return conn;
    //     }
    // }
}