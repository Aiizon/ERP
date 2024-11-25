using ERP.DATA.Core;

namespace ERP.DATA.Fields;

public class StringField : Field<string?>
{
    public StringField(Entity entity, string columnName, bool isPrimaryKey = false, bool doInsert = true) : base(entity, columnName, isPrimaryKey, doInsert)
    {
    }

    public override bool Read<TR>(TR reader)
    {
        Content = Database.GetStringValue(reader, ColumnName);
        return true;
    }

    public override string FormatValue()
    {
        return Database.SetStringValue(ColumnName, Content);
    }
}