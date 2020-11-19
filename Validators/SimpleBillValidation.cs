using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Models;

namespace Validators
{
    public class SimpleBillValidation : AbstractValidator<SimpleBill>
    {
        public SimpleBillValidation()
        {
            RuleFor(simpleBill => simpleBill.Name).NotNull().NotEmpty();
            RuleForEach(simpleBill => simpleBill.Sales).NotNull();
        }
    }
}
