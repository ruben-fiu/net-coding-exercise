using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public abstract class TaxableProduct : Product, ITaxableProduct
    {
        public TaxableProduct() : base()
        {

        }
        public TaxableProduct(string name, double price, bool imported) : base(name, price, imported)
        {
        }

        public int PercentTax
        {
            get => (int)GetPercentTax();
        }
        protected abstract Tax GetPercentTax();

    }
}
