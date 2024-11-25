using ERP.DATA.Core;

namespace ERP.DATA.Fields;

public class IntegerField : Field<int?>
{
    public IntegerField(Entity entity, string columnName, bool isPrimaryKey = false, bool doInsert = true) : base(entity, columnName, isPrimaryKey, doInsert)
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