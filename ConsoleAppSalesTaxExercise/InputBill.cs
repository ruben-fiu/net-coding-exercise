using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSalesTaxExercise
{
    public class InputBill
    {
        public InputBill()
        {
            Sales = new List<InputSale>();
        }

        public string Name { get; set; }
        public List<InputSale> Sales { get; set; }
    }
}
