using Microsoft.Extensions.DependencyInjection;
using Models;
using Reports;
using System;
using System.Collections.Generic;
using Validators;

namespace ConsoleAppSalesTaxExercise
{
    class Program
    {

        static string readNoEmptyString(string message)
        {
            var line = string.Empty;
            while (line == string.Empty)
            {
                Console.Write(message);
                line = Console.ReadLine();
            }
            return line;
        }
        static int readInteger(string message)
        {
            bool validInteger = false;
            int result = -1;

            while (!validInteger)
            {
                var line = readNoEmptyString(message);
                try
                {
                    result = int.Parse(line);
                    validInteger = true;

                }
                catch (Exception)
                {
                }
            }
            return result;
        }
        static double readDouble(string message)
        {
            bool validDouble = false;
            double result = -1;

            while (!validDouble)
            {
                var line = readNoEmptyString(message);
                try
                {
                    result = double.Parse(line);
                    validDouble = true;

                }
                catch (Exception)
                {
                }
            }
            return result;
        }
        static string readOptions(string message, List<string> options)
        {
            var line = string.Empty;
            while (!options.Exists( option => option.Equals(line)))
            {
                Console.Write(message);
                line = Console.ReadLine().Trim().ToLower();
            }
            return line;
        }

        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IGeneralValidator, Validator>()
                .AddTransient<ISalesReportServices, SalesReportServices>()
                .AddTransient<ISalesReporter, SalesReporter>()
                .AddSingleton<ILogger, ConsoleLogger>()
                .BuildServiceProvider();


            var inputBill = new InputBill();
            inputBill.Name = readNoEmptyString("Enter Bill name:");

            var otherSale = true;
            while (otherSale)
            {
                var inputSale = new InputSale();
                inputSale.ProductName = readNoEmptyString("Enter the product name:");
                inputSale.Price = readDouble("Enter the product price:");
                inputSale.Cant = readInteger("Enter product quantity:");
                var type = readOptions("Enter the product type[food, medicine, book, common]:", new List<string>() { "food", "medicine", "book", "common" });
                inputSale.Type = (type.Equals("food")) ? ProductType.Food: (type.Equals("medicine")) ? ProductType.Medicine: (type.Equals("book")) ? ProductType.Book: ProductType.Common;
                var importedProduct = readOptions("It's an imported products?[y/n]:", new List<string>() { "y", "n" });
                inputSale.Imported = importedProduct.Equals("y");
                inputBill.Sales.Add(inputSale);
                otherSale = readOptions("Other Sale?[y/n] :", new List<string>() { "y", "n" }).Equals("y");
            }

            var service = serviceProvider.GetService<ISalesReportServices>();
            SaleReport report;
            try
            {
                report = service.GenerateReport(inputBill);
            }
            catch (FluentValidation.ValidationException e)
            {
                Console.ReadLine();
                return;
            }

            Console.WriteLine("---------------------------------------------------------------------------------");
            Console.WriteLine(report.Name);
            foreach (var transaction in report.Transactions)
            {
                Console.WriteLine("{0} x {1} {2}$: {3}$", transaction.Quantity, transaction.Name, transaction.Price, transaction.Total);
            }
            Console.WriteLine("Sales Tax: {0}$ Total: {1}$", report.Taxes, report.Total);            
        }
    }
}
