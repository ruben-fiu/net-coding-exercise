using FluentValidation;
using Models;
using System;

namespace Validators
{
    public class BookValidation : AbstractValidator<Book>
    {
        public BookValidation()
        {
            RuleFor(book => book.Name).NotNull();
            RuleFor(book => book.Price).GreaterThanOrEqualTo(0);
        }
    }
}
