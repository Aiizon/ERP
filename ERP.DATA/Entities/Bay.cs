using ERP.DATA.Core;
using ERP.DATA.Fields;

namespace ERP.DATA.Entities;

public class Bay : Entity
{
    public sealed override string TableName { get; }
    public override string TableAlias { get; }

    public IntegerField Id { get; private set; }
    public StringField Name { get; private set; }
    public StringField Location { get; private set; }
    public UnitList<Bay> Units { get; private set; }

    public Bay()
    {
        TableName  = "bay";
        TableAlias = "b";
        Id         = new IntegerField(this, TableName, true, false);
        Name       = new StringField(this, TableName);
        Location   = new StringField(this, TableName);
        Units      = new UnitList<Bay>(this, new Dictionary<string, Field>
        {
            { "bay_id", Id }
        });
    }
}

public class BayKey : ForeignKey<Bay>
{
    public IntegerField BayId { get; private set; }

    public BayKey(Entity primary, string columnName = "bay_id") : base(primary)
    {
        BayId = new IntegerField(primary, columnName);
        AddField("id", BayId);
    }
}