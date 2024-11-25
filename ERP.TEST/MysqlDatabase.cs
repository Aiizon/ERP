using System.Data.Common;
using ERP.DATA;
using MySql.Data.MySqlClient;

namespace ERP.TEST;

public class MysqlDatabase : Database<MySqlConnection, MySqlCommand, DbDataReader>
{
    public MysqlDatabase(ConnectionBuilder builder) : base(builder)
    {
        #region config
        Builder.Database = "MySQL";
        Builder.Server = "localhost";
        Builder.Timeout = 20;
        Builder.User = "wt";
        Builder.Password = "kq4DEpdU5b3jsgXT7xbqknxyzhpHFpmM";
        #endregion
    }

    public override MySqlConnection GetConnection()
    {
        return new MySqlConnection(Builder.ToMysqlConnectionString());
    }

    public override MySqlCommand GetCommand(MySqlConnection connection, string query)
    {
        return new MySqlCommand(query, connection);
    }

    public override List<TE> GetEntities<TE>(string where, Func<Database, DbDataReader, TE> parser)
    {
        string query = "";
        return new List<TE>();
    }
}