namespace ERP.DATA;

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
    /// List of fields of this entity
    /// </summary>
    protected List<Field> Fields { get; private set; }

    public Entity()
    {
        Fields   = new List<Field>();
    }

    public virtual bool Insert<T>(T entity, Database database)
    {
        try
        {
            string query = "";
            database.ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
    public virtual bool Update<T>(T entity, Database database)
    {
        try
        {

        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
    public virtual bool Delete<T>(T entity, Database database)
    {
        try
        {

        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    public void Add(string field, bool isPrimaryKey = false)
    {
        Fields.Add(new Field(this, field, isPrimaryKey));
    }
}