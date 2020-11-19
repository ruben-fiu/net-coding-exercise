namespace Models
{
    public interface ITaxableProduct
    {
        int PercentTax { get; }
        string Name { get; }

        double Price { get; }

        bool Imported { get; }
    }
}