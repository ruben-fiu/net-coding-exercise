using FluentValidation;
using Models;
using System;

namespace Validators
{
    public class CommonProductValidation : AbstractValidator<CommonProduct>
    {
        public CommonProductValidation()
        {
            RuleFor(product => product.Name).NotNull();
            RuleFor(product => product.Price).GreaterThanOrEqualTo(0);
        }
    }
}
