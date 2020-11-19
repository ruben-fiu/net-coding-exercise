using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class SimpleBill
    {
        private readonly List<Sale> sales;
        private readonly string name;

        public SimpleBill(string name)
        {
            this.name = name;
            this.sales = new List<Sale>();
        }
        public SimpleBill(string name, List<Sale> sales)
        {
            this.name = name;
            this.sales = sales;
        }

        public void AddSale(Sale sale)
        {
            sales.Add(sale);
        }

        public IReadOnlyList<Sale> Sales => sales;

        public string Name => name;
    }
}
