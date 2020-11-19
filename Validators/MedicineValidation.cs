using FluentValidation;
using Models;
using System;

namespace Validators
{
    public class MedicineValidation : AbstractValidator<Medicine>
    {
        public MedicineValidation()
        {
            RuleFor(product => product.Name).NotNull();
            RuleFor(product => product.Price).GreaterThanOrEqualTo(0);
        }
    }
}
