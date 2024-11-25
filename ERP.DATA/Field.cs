using System.Data.Common;

namespace ERP.DATA;

public class Field
{
    /// <summary>
    /// Name of the column in the database
    /// </summary>
    public string ColumnName { get; private set; }
    
    /// <summary>
    /// Entity that this field belongs to
    /// </summary>
    public Entity Entity { get; private set; }

    /// <summary>
    /// Is this field a primary key?
    /// </summary>
    public bool IsPrimaryKey { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="columnName">Column name</param>
    /// <param name="isPrimaryKey">Is primary key?</param>
    public Field
    (
        Entity entity,
        string columnName,
        bool   isPrimaryKey = false
    )
    {
        Entity       = entity;
        ColumnName   = columnName;
        IsPrimaryKey = isPrimaryKey;
    }
}

public abstract class Field<T> : Field
{
    /// <summary>
    /// Value of the field
    /// </summary>
    public T Content { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="columnName">Column name</param>
    /// <param name="content">Content</param>
    /// <param name="isPrimaryKey">Is primary key?</param>
    public Field
    (
        Entity entity,
        string columnName,
        T      content,
        bool   isPrimaryKey = false
    ) : base(entity, columnName, isPrimaryKey)
    {
        Content = content;
    }

    public abstract bool Read<TR>(TR reader) where TR : DbDataReader;
}
