using ERP.DATA.Core;
using ERP.DATA.Fields;

namespace ERP.DATA.Entities;

public class Unit : Entity
{
    public sealed override string TableName { get; }
    public override string TableAlias { get; }

    public IntegerField Id { get; private set; }
    public StringField Name { get; private set; }
    public BayKey Bay { get; private set; }

    public Unit()
    {
        TableName  = "bay";
        TableAlias = "b";
        Id         = new IntegerField(this, TableName, true, false);
        Name       = new StringField(this, TableName);
        Bay        = new BayKey(this);
    }
}

public class UnitList<TE> : List<TE, Unit>
    where TE : Entity, new()
{
    public UnitList(TE localEntity, IEnumerable<KeyValuePair<string, Field>> fields) : base(localEntity, fields)
    {
    }
}