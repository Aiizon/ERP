using System.Reflection;

namespace ERP.DATA.Core;

public abstract class Entity
{
    /// <summary>
    /// Database that this entity belongs to
    /// </summary>
    public required Database Database { get; set; }

    /// <summary>
    /// Name of the table in the database
    /// </summary>
    public abstract string TableName { get; }

    /// <summary>
    /// Alias of the table in the database
    /// </summary>
    public abstract string TableAlias { get; }

    /// <summary>
    /// List of fields of this entity
    /// </summary>
    protected List<Field> Fields { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public Entity()
    {
        Fields = new List<Field>();
    }

    /// <summary>
    /// Insert an entity into the database
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="database">Database</param>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <returns>True if insert was successful, false otherwise</returns>
    public virtual bool Insert<T>(T entity, Database database)
    {
        try
        {
            string fieldExpr = "";
            string valueExpr = "";
            foreach (Field field in Fields)
            {
                if (!field.DoInsert)
                {
                    continue;
                }
                if (fieldExpr != "")
                {
                    fieldExpr += ",";
                    valueExpr += ",";
                }
                fieldExpr += field.ColumnName;
                valueExpr += field.FormatValue();
            }
            string query = $"INSERT INTO {TableName}( {fieldExpr} ) VALUES( {valueExpr} )";
            Database.ExecuteNonQuery(query);
            return true;
        }
        catch (Exception e)
        {
            Database.HandleException(e, MethodBase.GetCurrentMethod()!.Name);
            return false;
        }
    }

    /// <summary>
    /// Update an entity in the database
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="database">Database</param>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <returns>True if insert was successful, false otherwise</returns>
    public virtual bool Update<T>(T entity, Database database)
    {
        try
        {
            string setExpr = "";
            string whereExpr = "";

            foreach (Field field in Fields)
            {
                if (!field.DoInsert)
                {
                    continue;
                }
                if (field.IsPrimaryKey)
                {
                    if (whereExpr != "")
                    {
                        whereExpr += " AND ";
                    }
                    whereExpr += $"{field.ColumnName} = {field.FormatValue()}";
                }
                else
                {
                    if (setExpr != "")
                    {
                        setExpr += ",";
                    }
                    setExpr += $"{field.ColumnName} = {field.FormatValue()}";
                }
            }

            string query = $"UPDATE {TableName} SET {setExpr} WHERE {whereExpr}";
            Database.ExecuteNonQuery(query);
            return true;
        }
        catch (Exception e)
        {
            Database.HandleException(e, MethodBase.GetCurrentMethod()!.Name);
            return false;
        }
    }

    /// <summary>
    /// Delete an entity from the database
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="database">Database</param>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <returns>True if insert was successful, false otherwise</returns>
    public virtual bool Delete<T>(T entity, Database database)
    {
        try
        {
            string whereExpr = "";

            foreach (Field field in Fields.Where(f => f.IsPrimaryKey))
            {
                if (!field.DoInsert)
                {
                    continue;
                }
                whereExpr += $"{field.ColumnName} = {field.FormatValue()}";
            }

            string query = $"DELETE FROM {TableName} WHERE {whereExpr}";
            Database.ExecuteNonQuery(query);
            return true;
        }
        catch (Exception e)
        {
            Database.HandleException(e, MethodBase.GetCurrentMethod()!.Name);
            return false;
        }
    }

    /// <summary>
    /// Add a field to the entity's field list
    /// </summary>
    /// <param name="field">Field</param>
    /// <param name="isPrimaryKey">Is primary key?</param>
    public void Add(string field, bool isPrimaryKey = false)
    {
        //@todo: add field type to params
        //Fields.Add(new Field(this, field, isPrimaryKey));
    }
}