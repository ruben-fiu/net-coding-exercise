using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CommonProduct : TaxableProduct
    {
        public CommonProduct(string name, double price, bool imported) : base(name, price, imported)
        {
        }

        protected override Tax GetPercentTax()
        {
            return Models.Tax.CommonTax;
        }
    }
}
