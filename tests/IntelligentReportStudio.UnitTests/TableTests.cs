using IntelligentReportStudio;
using Xunit;

namespace IntelligentReportStudio.UnitTests;

public sealed class TableTests
{
    [Fact]
    public void ColumnsAndRowsEnforceTheirShape()
    {
        var table = new ReportTable("orders");
        table.AddColumn("id", ReportDataType.Integer);
        Assert.Throws<ArgumentException>(() => table.AddColumn("id", ReportDataType.Text));
        Assert.Throws<ArgumentException>(() => table.AddRow(1, 2));
        table.AddRow(1);
        Assert.Equal(1, table.RowCount);
        Assert.Throws<InvalidOperationException>(() => table.AddColumn("name", ReportDataType.Text));
    }
}
