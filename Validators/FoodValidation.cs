using FluentValidation;
using Models;
using System;

namespace Validators
{
    public class FoodValidation : AbstractValidator<Food>
    {
        public FoodValidation()
        {
            RuleFor(product => product.Name).NotNull();
            RuleFor(product => product.Price).GreaterThanOrEqualTo(0);
        }
    }
}
