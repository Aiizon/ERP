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
        Fields   = new List<Field>();
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

        }
        catch (Exception e)
        {
            return false;
        }
        return true;
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

        }
        catch (Exception e)
        {
            return false;
        }
        return true;
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

        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Add a field to the entity's field list
    /// </summary>
    /// <param name="field">Field</param>
    /// <param name="isPrimaryKey">Is primary key?</param>
    public void Add(string field, bool isPrimaryKey = false)
    {
        //@todo: create class extending Field
        //Fields.Add(new Field(this, field, isPrimaryKey));
    }
}