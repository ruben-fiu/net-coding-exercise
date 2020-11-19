using FluentValidation;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Validators
{
    public class SaleValitation : AbstractValidator<Sale>
    {
        public SaleValitation()
        {
            RuleFor(sale => sale.Product).NotNull();
            RuleFor(sale => sale.Quantity).GreaterThan(0);
        }
    }
}
