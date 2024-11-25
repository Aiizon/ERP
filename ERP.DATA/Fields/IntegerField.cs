using ERP.DATA.Core;

namespace ERP.DATA.Fields;

public class IntegerField : Field<int?>
{
    public IntegerField(Entity entity, string columnName, int? content, bool isPrimaryKey = false) : base(entity, columnName, content, isPrimaryKey)
    {
    }

    public override bool Read<TR>(TR reader)
    {
        Content = Database.GetIntValue(reader, ColumnName);
        return true;
    }

    public override string FormatValue()
    {
        return Database.SetIntValue(ColumnName, Content);
    }
}