using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Food : ZeroTaxProduct
    {
        public Food()
        {
        }

        public Food(string name, double price, bool imported) : base(name, price, imported)
        {
        }

    }
}
