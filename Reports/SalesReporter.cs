using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reports
{
    public class SalesReporter : ISalesReporter
    {
        private SimpleBill simpleBill;

        public SimpleBill SimpleBill
        {
            get 
            {
                return simpleBill;
            }
            set
            {
                simpleBill = value;
            }
        }

        public SalesReporter(SimpleBill simpleBill)
        {
            this.simpleBill = simpleBill;
        }
        public SalesReporter()
        {
            this.simpleBill = null;
        }
        private double RoundUpToNearest(double passednumber)
        {
            return Math.Ceiling(passednumber * 20) / 20;
        }
        private double CalculateTax(int percentTax, double price, bool imported)
        {
            var tax = percentTax + ((imported) ? (int)Models.Tax.ImportationTax : 0);
            return this.RoundUpToNearest(price * tax / 100);
        }

        public SaleReport GenerateReport()
        {
            if (simpleBill == null)
                return null;

            var transactions = new List<SaleTransaction>();

            double total = 0, taxes = 0;

            foreach (var sale in simpleBill.Sales)
            {
                var transaction = new SaleTransaction();
                transaction.Name = sale.Product.Name;
                transaction.Price = sale.Product.Price;
                transaction.Quantity = sale.Quantity;
                transaction.Tax = CalculateTax(sale.Product.PercentTax, sale.Product.Price, sale.Product.Imported);

                double totalTaxes = transaction.Tax * transaction.Quantity;
                var sub_total = (transaction.Quantity * transaction.Price) + totalTaxes;
                transaction.Total = sub_total;
                total += sub_total;
                taxes += totalTaxes;

                transactions.Add(transaction);

            }

            var report = new SaleReport(simpleBill.Name, transactions, Math.Round(total, 2), Math.Round(taxes, 2));

            return report;
        }
    }
}
