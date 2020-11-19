namespace ConsoleAppSalesTaxExercise
{
    public interface ILogger
    {
        void debug(string message);
        void error(string message);
        void info(string message);
    }
}