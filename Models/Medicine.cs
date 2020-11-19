using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Medicine : ZeroTaxProduct
    {
        public Medicine()
        {
        }

        public Medicine(string name, double price, bool imported) : base(name, price, imported)
        {
        }

    }
}
