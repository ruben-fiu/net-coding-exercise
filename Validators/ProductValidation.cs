using FluentValidation;
using Models;
using System;

namespace Validators
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(product => product.Name).NotNull();
            RuleFor(product => product.Price).GreaterThanOrEqualTo(0);
        }
    }
}
