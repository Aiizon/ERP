using ERP.DATA.Core;
using ERP.DATA.Fields;

namespace ERP.DATA.Entities;

public class Bay : Entity
{
    public override string TableName { get; }
    public override string TableAlias { get; }

    public IntegerField Id { get; private set; }
    public StringField Name { get; private set; }
    public StringField Location { get; private set; }

    public Bay()
    {
        TableName  = "bay";
        TableAlias = "b";

    }
}