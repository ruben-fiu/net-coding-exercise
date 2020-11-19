using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSalesTaxExercise
{
    public class InputSale
    {
        public int Cant { get; set; }

        public string ProductName { get; set; }

        public ProductType Type { get; set; }

        public double Price { get; set; }

        public bool Imported { get; set; }
    }
}
