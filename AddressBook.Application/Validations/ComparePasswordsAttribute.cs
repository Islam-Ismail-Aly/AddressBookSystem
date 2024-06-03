using System.ComponentModel.DataAnnotations;

namespace AddressBook.Application.Validations
{
    public class ComparePasswordsAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public ComparePasswordsAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = value as string;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
            {
                return new ValidationResult($"Property with name {_comparisonProperty} not found");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance) as string;

            if (!string.Equals(currentValue, comparisonValue))
            {
                return new ValidationResult("The password and confirmation password do not match.");
            }

            return ValidationResult.Success;
        }
    }
}
