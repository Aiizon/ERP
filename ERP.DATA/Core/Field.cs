using System.Data.Common;

namespace ERP.DATA.Core;

public abstract class Field
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
    /// Database that this field belongs to
    /// </summary>
    public Database Database => Entity.Database;

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

    /// <summary>
    /// Read data from the database
    /// </summary>
    /// <param name="reader">Reader method</param>
    /// <typeparam name="TR">Reader type</typeparam>
    /// <returns>True if read is successful, false otherwise</returns>
    public abstract bool Read<TR>(TR reader) where TR : DbDataReader;

    /// <summary>
    /// Write data to the database
    /// </summary>
    /// <param name="doWriteValue">Add the field's value</param>
    /// <param name="doWriteName">Add the field's name</param>
    /// <param name="doWriteAlias">Add the table's alias</param>
    /// <returns>Formatted string, or null if the parameters are wrong</returns>
    public string? Write(bool doWriteValue, bool doWriteName, bool doWriteAlias)
    {
        string result = "";

        if (doWriteAlias)
        {
            result += Entity.TableAlias + ".";
        }
        if (doWriteValue && doWriteName)
        {
            result += ColumnName + " = " + FormatValue();
        }
        else if (doWriteValue)
        {
            return FormatValue();
        }
        else if (doWriteName)
        {
            result += ColumnName;
        }
        else
        {
            return null;
        }

        return result;
    }

    public abstract string FormatValue();
}



// @todo: add more field types