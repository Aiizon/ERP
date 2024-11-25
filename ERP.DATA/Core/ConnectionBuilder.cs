namespace ERP.DATA.Core;

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
    public string Password { private get; set; }

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
    /// Returns the connection string in MySQL format
    /// </summary>
    /// <returns></returns>
    public string ToMysqlConnectionString()
    {
        return $"Server={Server};Database={Database};Uid={User};Pwd={Password};";
    }

    /// <summary>
    /// Returns the connection string in ODBC format
    /// </summary>
    /// <returns></returns>
    public string ToOdbcConnectionString()
    {
        return $"Driver={{SQL Server}};Server={Server};Database={Database};Uid={User};Pwd={Password};";
    }
}