using Models;

namespace ConsoleAppSalesTaxExercise
{
    public interface ISalesReportServices
    {
        SaleReport GenerateReport(InputBill bill);
    }
}