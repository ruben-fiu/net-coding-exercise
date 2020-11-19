using FluentValidation.Results;

namespace Validators
{
    public interface IGeneralValidator
    {
        ValidationResult Validate(object entity);
    }
}