using Models;

namespace Reports
{
    public interface ISalesReporter
    {
        SimpleBill SimpleBill { get; set; }

        SaleReport GenerateReport();
    }
}