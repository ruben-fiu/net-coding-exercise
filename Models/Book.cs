using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Book : ZeroTaxProduct
    {
        public Book()
        {
        }

        public Book(string name, double price, bool imported) : base(name, price, imported)
        {
        }

    }
}
