using System.Data;

namespace ERP.DATA.Core;

public class List<TL, TF>
    where TL : Entity, new()
    where TF : Entity, new()
{
    /// <summary>
    /// Database that this list belongs to
    /// </summary>
    public Database Database => LocalEntity.Database;

    /// <summary>
    /// Entity that this list belongs to
    /// </summary>
    public TL LocalEntity { get; set; }

    /// <summary>
    /// List of fields of this list
    /// </summary>
    protected Dictionary<string, Field> LocalFields { get; private set; } = null!;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="localEntity">Entity</param>
    /// <param name="fields">List of key-value pairs corresponding to primary key fields</param>
    protected List(TL localEntity, IEnumerable<KeyValuePair<string, Field>> fields)
    {
        LocalEntity = localEntity;
        foreach (var field in fields)
        {
            AddField(field.Key, field.Value);
        }
    }

    /// <summary>
    /// Add a field to the list
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="field">Field</param>
    internal void AddField(string key, Field field)
    {
        if (LocalFields.ContainsKey(key))
        {
            throw new DataException($"This key already exists : {key}.");
        }
        LocalFields.Add(key, field);
    }

    /// <summary>
    /// Count the number of linked foreign entities
    /// </summary>
    /// <returns>Number of entities</returns>
    public virtual int? Count()
    {
        string where = GetWhere();

        return Database.CountEntities<TF>(where);
    }

    /// <summary>
    /// Get a list of linked foreign entities
    /// </summary>
    /// <returns>List of entities</returns>
    public virtual List<TF> Get()
    {
        string where = GetWhere();

        return Database.GetEntities<TF>(where);
    }

    private string GetWhere()
    {
        string where = "";

        foreach (var localField in LocalFields)
        {
            if (where != "")
            {
                where += " AND ";
            }
            where += $"{localField.Key} = {localField.Value.FormatValue()}";
        }

        return where;
    }
}