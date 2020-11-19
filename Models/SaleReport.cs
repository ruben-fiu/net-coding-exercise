using System.Collections.Generic;

namespace Models
{
    public class SaleReport
    {
        private readonly string name;
        private readonly List<SaleTransaction> transactions;
        private readonly double total;
        private readonly double taxes;


        public SaleReport(string name, List<SaleTransaction> transactions, double total, double taxes)
        {
            this.name = name;
            this.transactions = transactions;
            this.total = total;
            this.taxes = taxes;
        }

        public List<SaleTransaction> Transactions => transactions;

        public string Name => name;

        public double Total => total;

        public double Taxes => taxes;
    }
}
