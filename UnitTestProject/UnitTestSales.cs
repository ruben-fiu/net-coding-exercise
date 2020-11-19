using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Reports;
using System;
using System.Collections.Generic;
using Validators;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestSales
    {
        private const string billName = "bill1";
        private const string bookName = "book1";
        private const string foodName = "food1";
        private const double priceGreaterThanZero = 5;
        private const double priceLowerThanZero = -5;
        private const string medicineName = "medicine1";
        private const bool NotImported = false;
        private const bool Imported = true;
        private const string commonProduct1 = "product1";
        private const string commonProduct2 = "product2";
        private TaxableProduct[] taxableProducts;
        private TaxableProduct[] taxableProductsWithNegativePrice;

        [TestInitialize]
        public void TestInitialize()
        {
            taxableProducts = new TaxableProduct[]
            {
                new Book(bookName, priceGreaterThanZero, Imported),
                new Food(foodName, priceGreaterThanZero, Imported),
                new Medicine(medicineName, priceGreaterThanZero, Imported),
                new CommonProduct(commonProduct1, priceGreaterThanZero, Imported)
            };
            taxableProductsWithNegativePrice = new TaxableProduct[]
            {
                new Book(bookName, priceLowerThanZero, Imported),
                new Food(foodName, priceLowerThanZero, Imported),
                new Medicine(medicineName, priceLowerThanZero, Imported),
                new CommonProduct(commonProduct1, priceLowerThanZero, Imported)
            };
        }

        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 0)]
        [DataRow(2, 0)]
        [DataRow(3, 0)]
        public void Sales_Of_Taxable_Products_With_Zero_Quantity_Should_Fail(int index, int quantity)
        {
            // Arrangement

            var product = taxableProducts[index];
            var validator = new Validator();
            Sale sale = new Sale(product, quantity);
            bool expecteResult = false;

            //Act
            var validationResult = validator.Validate(sale);
            bool currentResult = validationResult.IsValid;

            // Assert
            Assert.AreEqual(expecteResult, currentResult, "Validation Fail for 0 quantity.");
        }

        [DataTestMethod]
        [DataRow(0, -1)]
        [DataRow(1, -1)]
        [DataRow(2, -1)]
        [DataRow(3, -1)]
        public void Sales_Of_Taxable_Products_With_Negative_Quantity_Should_Fail(int index, int quantity)
        {
            // Arrangement

            var product = taxableProducts[index];
            var validator = new Validator();
            Sale sale = new Sale(product, quantity);
            bool expecteResult = false;

            //Act
            var validationResult = validator.Validate(sale);
            bool currentResult = validationResult.IsValid;

            // Assert
            Assert.AreEqual(expecteResult, currentResult, "Validation Fail for negative quantity.");
        }

        [DataTestMethod]
        [DataRow(0, 1)]
        [DataRow(1, 1)]
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        public void Sales_Of_Taxable_Products_With_Negative_Price_Should_Fail(int index, int quantity)
        {
            // Arrangement

            var product = taxableProductsWithNegativePrice[index];
            var validator = new Validator();
            bool expecteResult = false;

            //Act
            var validationResult = validator.Validate(product);
            bool currentResult = validationResult.IsValid;

            // Assert
            Assert.AreEqual(expecteResult, currentResult, "Validation Fail for negative price.");
        }

        [TestMethod]
        public void Bills_with_empty_names_should_fail()
        {
            // Arrangement

            var quantity = 1;
            var price = 5;

            var book = new Book(bookName, price, NotImported);
            var validator = new Validator();

            var bill = new SimpleBill("");
            bill.AddSale(new Sale(book, quantity));

            //Act

            var validationResult = validator.Validate(bill);

            // Assert
            Assert.IsFalse(validationResult.IsValid, validationResult.ToString());
        }          

        [TestMethod]
        public void When_selling_non_imported_book_food_and_medicine_no_taxes_will_be_paid()
        {
            //Arrange 

            var quantity = 1;
            var price = 5;

            var book = new Book(bookName, price, NotImported);
            var food = new Food(foodName, price, NotImported);
            var medicine = new Medicine(medicineName, price, NotImported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(book, quantity));
            bill.AddSale(new Sale(food, quantity));
            bill.AddSale(new Sale(medicine, quantity));

            var totalCost = 15;
            var totalTaxes = 0;
            var totalCostWithTaxes = totalCost + totalTaxes;
            var itemPrice = quantity * price;

            Stack<string> nameList = new Stack<string>();
            nameList.Push("medicine1");
            nameList.Push("food1");
            nameList.Push("book1");

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();


            // Assert
            var validationResult = validator.Validate(book);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(food);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(reporter);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(medicine);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(itemPrice, transaction.Total, "Price was stored incorrectly");
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }

        [TestMethod]
        public void When_selling_multiple_non_imported_book_food_and_medicine_no_taxes_will_be_paid()
        {
            //Arrange 

            var quantity = 10;
            var price = 1;

            var book = new Book(bookName, price, NotImported);
            var food = new Food(foodName, price, NotImported);
            var medicine = new Medicine(medicineName, price, NotImported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(book, quantity));
            bill.AddSale(new Sale(food, quantity));
            bill.AddSale(new Sale(medicine, quantity));

            var totalCost = 30;
            var totalTaxes = 0;
            var totalCostWithTaxes = totalCost + totalTaxes;
            var itemPrice = quantity * price;

            Stack<string> nameList = new Stack<string>();
            nameList.Push("medicine1");
            nameList.Push("food1");
            nameList.Push("book1");

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();


            // Assert
            var validationResult = validator.Validate(book);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(food);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(reporter);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(medicine);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(itemPrice, transaction.Total, "Price was stored incorrectly");
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }

        [TestMethod]
        public void When_selling_imported_book_food_and_medicine_taxes_will_be_paid()
        {
            // Arrange

            var quantity = 1;
            var price = 20;

            var book = new Book(bookName, price, Imported);
            var food = new Food(foodName, price, Imported);
            var medicine = new Medicine(medicineName, price, Imported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(book, quantity));
            bill.AddSale(new Sale(food, quantity));
            bill.AddSale(new Sale(medicine, quantity));

            var totalCost = 60;
            var totalTaxes = 3;
            var taxPercent = 0.05;
            var totalCostWithTaxes = totalCost + totalTaxes;
            var itemPrice = quantity * price + (quantity * price * taxPercent);

            Stack<string> nameList = new Stack<string>();
            nameList.Push("medicine1");
            nameList.Push("food1");
            nameList.Push("book1");

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();


            // Assert
            var validationResult = validator.Validate(book);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(food);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(reporter);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(medicine);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(itemPrice, transaction.Total, "Price was stored incorrectly");
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreNotEqual(totalCost, report.Total, "The taxes were not added to the total");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }

        [TestMethod]
        public void When_selling_multiple_imported_book_food_and_medicine_taxes_will_be_paid()
        {
            // Arrange

            var quantity = 10;
            var price = 1;

            var book = new Book(bookName, price, Imported);
            var food = new Food(foodName, price, Imported);
            var medicine = new Medicine(medicineName, price, Imported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(book, quantity));
            bill.AddSale(new Sale(food, quantity));
            bill.AddSale(new Sale(medicine, quantity));

            var totalCost = 30;
            var totalTaxes = 1.5;
            var taxPercent = 0.05;
            var totalCostWithTaxes = totalCost + totalTaxes;
            var itemPrice = quantity * price + (quantity * price * taxPercent);

            Stack<string> nameList = new Stack<string>();
            nameList.Push("medicine1");
            nameList.Push("food1");
            nameList.Push("book1");

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();


            // Assert
            var validationResult = validator.Validate(book);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(food);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(reporter);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(medicine);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(itemPrice, transaction.Total, "Price was stored incorrectly");
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreNotEqual(totalCost, report.Total, "The taxes were not added to the total");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }


        [TestMethod]
        public void When_selling_non_imported_common_products_10_perc_taxes_will_be_paid()
        {
            // Arrange

            var quantity = 1;
            var price = 20;

            var product1 = new CommonProduct(commonProduct1, price, NotImported);
            var product2 = new CommonProduct(commonProduct2, price, NotImported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(product1, quantity));
            bill.AddSale(new Sale(product2, quantity));

            var totalCost = 40;
            var totalTaxes = 4;
            var taxPercent = 0.10;
            var totalCostWithTaxes = totalCost + totalTaxes;
            var itemPrice = quantity * price + (quantity * price * taxPercent);

            Stack<string> nameList = new Stack<string>();
            nameList.Push("product2");
            nameList.Push("product1");

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();

            // Assert
            var validationResult = validator.Validate(product1);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(product2);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(itemPrice, transaction.Total, "Price was stored incorrectly");
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreNotEqual(totalCost, report.Total, "The taxes were not added to the total");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }

        [TestMethod]
        public void When_selling_multiple_non_imported_common_products_10_perc_taxes_will_be_paid()
        {
            // Arrange

            var quantity = 10;
            var price = 1;

            var product1 = new CommonProduct(commonProduct1, price, NotImported);
            var product2 = new CommonProduct(commonProduct2, price, NotImported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(product1, quantity));
            bill.AddSale(new Sale(product2, quantity));

            var totalCost = 20;
            var totalTaxes = 2;
            var taxPercent = 0.10;
            var totalCostWithTaxes = totalCost + totalTaxes;
            var itemPrice = quantity * price + (quantity * price * taxPercent);

            Stack<string> nameList = new Stack<string>();
            nameList.Push("product2");
            nameList.Push("product1");

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();

            // Assert
            var validationResult = validator.Validate(product1);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(product2);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(itemPrice, transaction.Total, "Price was stored incorrectly");
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreNotEqual(totalCost, report.Total, "The taxes were not added to the total");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }

        [TestMethod]
        public void When_selling_imported_common_products_15_perc_taxes_will_be_paid()
        {
            // Arrange

            var quantity = 1;
            var price = 20;

            var product1 = new CommonProduct(commonProduct1, price, Imported);
            var product2 = new CommonProduct(commonProduct2, price, Imported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(product1, quantity));
            bill.AddSale(new Sale(product2, quantity));
            var totalCost = 40;
            var totalTaxes = 6;
            var taxPercent = 0.15;
            var totalCostWithTaxes = totalCost + totalTaxes;
            var itemPrice = quantity * price + (quantity * price * taxPercent);

            Stack<string> nameList = new Stack<string>();
            nameList.Push("product2");
            nameList.Push("product1");

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();

            // Assert
            var validationResult = validator.Validate(product1);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(product2);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(itemPrice, transaction.Total, "Price was stored incorrectly");
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreNotEqual(totalCost, report.Total, "The taxes were not added to the total");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }

        [TestMethod]
        public void When_selling_multiple_imported_common_products_15_perc_taxes_will_be_paid()
        {
            // Arrange

            var quantity = 10;
            var price = 1;

            var product1 = new CommonProduct(commonProduct1, price, Imported);
            var product2 = new CommonProduct(commonProduct2, price, Imported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(product1, quantity));
            bill.AddSale(new Sale(product2, quantity));
            var totalCost = 20;
            var totalTaxes = 3;
            var taxPercent = 0.15;
            var totalCostWithTaxes = totalCost + totalTaxes;
            var itemPrice = quantity * price + (quantity * price * taxPercent);

            Stack<string> nameList = new Stack<string>();
            nameList.Push("product2");
            nameList.Push("product1");

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();

            // Assert
            var validationResult = validator.Validate(product1);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(product2);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(itemPrice, transaction.Total, "Price was stored incorrectly");
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreNotEqual(totalCost, report.Total, "The taxes were not added to the total");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }

        [TestMethod]
        public void Test_cases_using_input_1()
        {
            // Arrange
            var quantity = 1;
            List<double> price = new List<double>();
            price.Add(12.49);
            price.Add(14.99);
            price.Add(0.85);

            var book = new Book("Book", price[0], NotImported);
            var musicCD = new CommonProduct("Music CD", price[1], NotImported);
            var chocolateBar = new Food("Chocolate Bar", price[2], NotImported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(book, quantity));
            bill.AddSale(new Sale(musicCD, quantity));
            bill.AddSale(new Sale(chocolateBar, quantity));

            var totalCost = 28.33;
            var totalTaxes = 1.50;
            var count = 0;
            var totalCostWithTaxes = totalCost + totalTaxes;

            Stack<string> nameList = new Stack<string>();
            nameList.Push("Chocolate Bar");
            nameList.Push("Music CD");
            nameList.Push("Book");

            List<double> priceWithTaxes = new List<double>();
            priceWithTaxes.Add(12.49);
            priceWithTaxes.Add(16.49);
            priceWithTaxes.Add(0.85);

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();

            // Assert
            var validationResult = validator.Validate(book);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(musicCD);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(chocolateBar);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(priceWithTaxes[count], Math.Round(transaction.Total, 2), "Price was stored incorrectly");
                count += 1;
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreNotEqual(totalCost, report.Total, "The taxes were not added to the total");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }
        [TestMethod]
        public void Test_cases_using_input_2()
        {
            // Arrange

            var quantity = 1;
            List<double> price = new List<double>();
            price.Add(10);
            price.Add(47.50);


            var chocolateBar = new Food("Box of chocolates", price[0], Imported);
            var perfume = new CommonProduct("Bottle of Perfume", price[1], Imported);


            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(chocolateBar, quantity));
            bill.AddSale(new Sale(perfume, quantity));


            var totalCost = 57.50;
            var totalTaxes = 7.65;
            var count = 0;
            var totalCostWithTaxes = totalCost + totalTaxes;

            Stack<string> nameList = new Stack<string>();
            nameList.Push("Bottle of Perfume");
            nameList.Push("Box of chocolates");


            List<double> priceWithTaxes = new List<double>();
            priceWithTaxes.Add(10.50);
            priceWithTaxes.Add(54.65);

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();

            // Assert
            var validationResult = validator.Validate(chocolateBar);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(perfume);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(priceWithTaxes[count], Math.Round(transaction.Total, 2), "Price was stored incorrectly");
                count += 1;
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreNotEqual(totalCost, report.Total, "The taxes were not added to the total");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }

        [TestMethod]
        public void Test_cases_using_input_3()
        {

            // Arrange

            var quantity = 1;
            List<double> price = new List<double>();
            price.Add(27.99);
            price.Add(18.99);
            price.Add(9.75);
            price.Add(11.25);

            var perfume1 = new CommonProduct("Bottle of Perfume", price[0], Imported);
            var perfume2 = new CommonProduct("Bottle of Perfume", price[1], NotImported);
            var medicine = new Medicine("Packet of headache pills", price[2], NotImported);
            var chocolateBar = new Food("Box of chocolates", price[3], Imported);

            var bill = new SimpleBill(billName);
            bill.AddSale(new Sale(perfume1, quantity));
            bill.AddSale(new Sale(perfume2, quantity));
            bill.AddSale(new Sale(medicine, quantity));
            bill.AddSale(new Sale(chocolateBar, quantity));

            var totalCost = 67.98;
            var totalTaxes = 6.7;
            var count = 0;
            var totalCostWithTaxes = totalCost + totalTaxes;

            Stack<string> nameList = new Stack<string>();
            nameList.Push("Box of chocolates");
            nameList.Push("Packet of headache pills");
            nameList.Push("Bottle of Perfume");
            nameList.Push("Bottle of Perfume");

            List<double> priceWithTaxes = new List<double>();
            priceWithTaxes.Add(32.19);
            priceWithTaxes.Add(20.89);
            priceWithTaxes.Add(9.75);
            priceWithTaxes.Add(11.85);

            // Act

            var reporter = new SalesReporter(bill);
            var report = reporter.GenerateReport();
            var validator = new Validator();

            // Assert
            var validationResult = validator.Validate(perfume1);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(perfume2);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(chocolateBar);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());
            validationResult = validator.Validate(medicine);
            Assert.IsTrue(validationResult.IsValid, validationResult.ToString());

            report.Transactions.ForEach(transaction =>
            {
                Assert.AreEqual(nameList.Pop(), transaction.Name, "Names were stored incorrectly");
                Assert.AreEqual(quantity, transaction.Quantity, "Quantity was stored incorrectly");
                Assert.AreEqual(priceWithTaxes[count], Math.Round(transaction.Total, 2), "Price was stored incorrectly");
                count += 1;
            });
            Assert.AreEqual(bill.Name, report.Name, "The name is not equal");
            Assert.AreNotEqual(totalCost, report.Total, "The taxes were not added to the total");
            Assert.AreEqual(totalCostWithTaxes, report.Total, "The total is not correct");
            Assert.AreEqual(totalTaxes, report.Taxes, "The Tax is not correct");
        }

    }
}
