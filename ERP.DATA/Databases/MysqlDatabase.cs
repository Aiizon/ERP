using System.Data.Common;
using System.Reflection;
using ERP.DATA.Core;
using MySql.Data.MySqlClient;

namespace ERP.DATA.Databases;

public class MysqlDatabase : Database<MySqlConnection, MySqlCommand, DbDataReader>
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="builder">Connection builder</param>
    /// <param name="timeout">Timeout</param>
    /// <param name="user">Database user</param>
    /// <param name="password">Database password</param>
    public MysqlDatabase(ConnectionBuilder builder, int timeout, string user, string password) : base(builder)
    {
        // Set the builder's properties
        #region config
        Builder.User = user;
        Builder.Password = password;
        Builder.Timeout = timeout;
        #endregion
    }

    protected override MySqlConnection GetConnection()
    {
        return new MySqlConnection(Builder.ToMysqlConnectionString());
    }

    protected override MySqlCommand GetCommand(MySqlConnection connection, string query)
    {
        return new MySqlCommand(query, connection);
    }

    public override string? GetStringValue<TR>(TR reader, string columnName)
    {
        try
        {
            int index = reader.GetOrdinal(columnName);

            if (index >= 0)
            {
                return reader.GetString(index);
            }
            return null;
        }
        catch (Exception e)
        {
            HandleException(e, MethodBase.GetCurrentMethod()!.Name);
            return null;
        }
    }

    public override string SetStringValue(string columnName, string? content)
    {
        if (null == content)
        {
            return "NULL";
        }
        return $"'{content}'";
    }

    public override int? GetIntValue<TR>(TR reader, string columnName)
    {
        try
        {
            int index = reader.GetOrdinal(columnName);

            if (index >= 0)
            {
                if (reader.IsDBNull(index))
                {
                    return null;
                }
                return reader.GetInt32(index);
            }
            return null;
        }
        catch (Exception e)
        {
            HandleException(e, MethodBase.GetCurrentMethod()!.Name);
            return null;
        }
    }

    public override string SetIntValue(string columnName, int? content)
    {
        if (null == content)
        {
            return "NULL";
        }
        return content.Value.ToString();
    }

    public override DateTime? GetDateTimeValue<TR>(TR reader, string columnName)
    {
        try
        {
            int index = reader.GetOrdinal(columnName);

            if (index >= 0)
            {
                if (reader.IsDBNull(index))
                {
                    return null;
                }
                return reader.GetDateTime(index);
            }
            return null;
        }
        catch (Exception e)
        {
            HandleException(e, MethodBase.GetCurrentMethod()!.Name);
            return null;
        }
    }

    public override string SetDateTimeValue(string columnName, DateTime? content)
    {
        if (null == content)
        {
            return "NULL";
        }
        return $"'{content:yyyy-MM-dd HH:mm:ss}'";
    }
}