using System.Data;

namespace ERP.DATA.Core;

public class ForeignKey
{
    /// <summary>
    /// Database that this foreign key belongs to
    /// </summary>
    public Database Database => Entity.Database;

    /// <summary>
    /// Entity that this foreign key belongs to
    /// </summary>
    public Entity Entity { get; set; }

    /// <summary>
    /// List of fields of this foreign key
    /// </summary>
    protected Dictionary<string, Field> Fields { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="entity">Entity</param>
    protected ForeignKey(Entity entity)
    {
        Entity = entity;
        Fields = new Dictionary<string, Field>();
    }

    /// <summary>
    /// Add a field to the foreign key
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="field">Field</param>
    internal void AddField(string key, Field field)
    {
        if (Fields.ContainsKey(key))
        {
            throw new DataException($"This key already exists : {key}.");
        }
        Fields.Add(key, field);
    }
}

public class ForeignKey<TF> : ForeignKey where TF : Entity, new()
{
    public ForeignKey(Entity entity): base(entity) { }

    /// <summary>
    /// Get the foreign entity
    /// </summary>
    /// <returns>Foreign entity</returns>
    public TF GetForeignEntity()
    {
        string where = GetWhere();

        return Database.GetEntity<TF>(where);
    }

    /// <summary>
    /// Set the foreign entity
    /// </summary>
    public void SetForeignEntity()
    {
        foreach (var field in Fields)
        {
            if (null == Entity)
            {
                field.Value.SetContent(null);
            }
            else
            {
                field.Value.SetContent(Entity.GetField(field.Key)?.GetContent());
            }
        }
    }

    private string GetWhere()
    {
        string where = "";

        foreach (var field in Fields)
        {
            if (where != "")
            {
                where += " AND ";
            }
            where += $"{field.Key} = {field.Value.FormatValue()}";
        }

        return where;
    }
}