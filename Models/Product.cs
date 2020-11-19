using System;

namespace Models
{
    public abstract class Product
    {
        private readonly string EMPTY_NAME = string.Empty;
        private readonly double EMPTY_PRICE = 0;
        private readonly bool NO_IMPORTED = false;

        private string name;
        private double price;
        private bool imported;

        public Product()
        {
            this.name = EMPTY_NAME;
            this.price = EMPTY_PRICE;
            this.imported = NO_IMPORTED;
        }
        public Product(string name, double price, bool imported)
        {
            this.name = name;
            this.price = price;
            this.imported = imported;
        }

        public string Name 
        { 
            get => name; 
            set => name = value; 
        }
        public double Price 
        { 
            get => price; 
            set => price = value; 
        }
        public bool Imported 
        { 
            get => imported; 
            set => imported = value; 
        }

    }
}
