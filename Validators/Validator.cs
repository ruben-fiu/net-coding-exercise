using FluentValidation;
using FluentValidation.Results;
using System;
using System.Linq;
using System.Reflection;

namespace Validators
{
    public class Validator : IGeneralValidator
    {
        public ValidationResult Validate(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity");

            var typeOfAbstractValidator = typeof(AbstractValidator<>);
            var typeOfEntity = entity.GetType();
            var typeOfGenericAbstractValidator = typeOfAbstractValidator.MakeGenericType(typeOfEntity);

            var validatorClassByEntityClass = FindValidatorType(Assembly.GetExecutingAssembly(), typeOfGenericAbstractValidator);

            var typeOfValidationContext = typeof(ValidationContext<>);
            var typeOfGenericValidationContext = typeOfValidationContext.MakeGenericType(typeOfEntity);

            object[] GenericValidationContextParameters = { entity };
            var validationContextInstance = (IValidationContext)Activator.CreateInstance(typeOfGenericValidationContext, args: GenericValidationContextParameters);

            var validatorInstance = (IValidator)Activator.CreateInstance(validatorClassByEntityClass);

            return validatorInstance.Validate(validationContextInstance);
        }


        private Type FindValidatorType(Assembly assembly, Type evt)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            if (evt == null)
                throw new ArgumentNullException("evt");

            return assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(evt));
        }
    }
}
