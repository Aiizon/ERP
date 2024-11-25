using ERP.DATA.Core;

namespace ERP.DATA.Fields;

public class StringField : Field<string?>
{
    public StringField(Entity entity, string columnName, string? content, bool isPrimaryKey = false) : base(entity, columnName, content, isPrimaryKey)
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