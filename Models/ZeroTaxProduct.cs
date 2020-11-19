using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ZeroTaxProduct : TaxableProduct
    {
        public ZeroTaxProduct() : base()
        { 

        }

        public ZeroTaxProduct(string name, double price, bool imported) : base(name, price, imported)
        {

        }

        protected override Tax GetPercentTax()
        {
            return Models.Tax.ZeroTax;
        }
    }
}
