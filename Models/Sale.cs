using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Sale
    {
        private ITaxableProduct product;
        private int quantity;

        public Sale(ITaxableProduct product, int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }

        public ITaxableProduct Product 
        { 
            get => product; 
            set => product = value; 
        }
        public int Quantity { 
            get => quantity; 
            set => quantity = value; 
        }
    }
}
