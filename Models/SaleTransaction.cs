namespace Models
{
    public  class SaleTransaction
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
    }
}