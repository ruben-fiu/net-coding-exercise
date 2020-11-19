using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSalesTaxExercise
{
    public class ConsoleLogger : ILogger
    {
        public void info(string message)
        {
            Console.WriteLine("Info: {0}", message);
        }
        public void debug(string message)
        {
            Console.WriteLine("Debug: {0}", message);
        }
        public void error(string message)
        {
            Console.WriteLine("Error: {0}", message);
        }
    }
}
