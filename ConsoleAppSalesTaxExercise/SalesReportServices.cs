using Models;
using Reports;
using System;
using System.Collections.Generic;
using System.Text;
using Validators;

namespace ConsoleAppSalesTaxExercise
{
    public class SalesReportServices : ISalesReportServices
    {
        private readonly ISalesReporter _reporter;
        private readonly IGeneralValidator _validator;
        private readonly ILogger _logger;

        public SalesReportServices(ISalesReporter reporter, IGeneralValidator validator, ILogger logger)
        {
            _reporter = reporter;
            _validator = validator;
            _logger = logger;
        }

        public SaleReport GenerateReport(InputBill bill)
        {
            FluentValidation.Results.ValidationResult validationResult;
            var simpleBill = new SimpleBill(bill.Name);
            foreach (var inputSale in bill.Sales)
            {

                switch (inputSale.Type)
                {

                    case ProductType.Book:
                        Book newProduct = new Book(inputSale.ProductName, inputSale.Price, inputSale.Imported);
                        validationResult = _validator.Validate(newProduct);
                        if (!validationResult.IsValid)
                        {
                            foreach (var failure in validationResult.Errors)
                            {
                                _logger.error("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                            }
                            throw new FluentValidation.ValidationException(validationResult.ToString());
                        }
                        Sale sale = new Sale(newProduct, inputSale.Cant);

                        validationResult = _validator.Validate(sale);
                        if (!validationResult.IsValid)
                        {
                            foreach (var failure in validationResult.Errors)
                            {
                                _logger.error("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                            }
                            throw new FluentValidation.ValidationException(validationResult.ToString());
                        }
                        simpleBill.AddSale(sale);
                        break;
                    case ProductType.Food:
                        Food newFoodProduct = new Food(inputSale.ProductName, inputSale.Price, inputSale.Imported);
                        validationResult = _validator.Validate(newFoodProduct);
                        if (!validationResult.IsValid)
                        {
                            foreach (var failure in validationResult.Errors)
                            {
                                _logger.error("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                            }
                            throw new FluentValidation.ValidationException(validationResult.ToString());
                        }
                        Sale saleFood = new Sale(newFoodProduct, inputSale.Cant);

                        validationResult = _validator.Validate(saleFood);
                        if (!validationResult.IsValid)
                        {
                            foreach (var failure in validationResult.Errors)
                            {
                                _logger.error("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                            }
                            throw new FluentValidation.ValidationException(validationResult.ToString());
                        }
                        simpleBill.AddSale(saleFood);
                        break;
                    case ProductType.Medicine:
                        Medicine newMedicineProduct = new Medicine(inputSale.ProductName, inputSale.Price, inputSale.Imported);
                        validationResult = _validator.Validate(newMedicineProduct);
                        if (!validationResult.IsValid)
                        {
                            foreach (var failure in validationResult.Errors)
                            {
                                _logger.error("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                            }
                            return null;
                        }
                        Sale saleMedicine = new Sale(newMedicineProduct, inputSale.Cant);

                        validationResult = _validator.Validate(saleMedicine);
                        if (!validationResult.IsValid)
                        {
                            foreach (var failure in validationResult.Errors)
                            {
                                _logger.error("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                            }
                            return null;
                        }
                        simpleBill.AddSale(saleMedicine);
                        break;
                    default:
                        CommonProduct newCommonProduct = new CommonProduct(inputSale.ProductName, inputSale.Price, inputSale.Imported);
                        validationResult = _validator.Validate(newCommonProduct);
                        if (!validationResult.IsValid)
                        {
                            foreach (var failure in validationResult.Errors)
                            {
                                _logger.error("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                            }
                            return null;
                        }
                        Sale saleCommon = new Sale(newCommonProduct, inputSale.Cant);

                        validationResult = _validator.Validate(saleCommon);
                        if (!validationResult.IsValid)
                        {
                            foreach (var failure in validationResult.Errors)
                            {
                                _logger.error("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                            }
                            return null;
                        }
                        simpleBill.AddSale(saleCommon);
                        break;
                }

            }

            validationResult = _validator.Validate(simpleBill);

            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    _logger.error("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
                return null;
            }

            _reporter.SimpleBill = simpleBill;
            return _reporter.GenerateReport();
        }
    }
}
