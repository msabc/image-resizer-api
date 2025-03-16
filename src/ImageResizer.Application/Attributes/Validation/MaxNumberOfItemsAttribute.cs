using System.ComponentModel.DataAnnotations;

namespace ImageResizer.Application.Attributes.Validation
{
    public class MaxNumberOfItems(int maxNumberOfItems) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is List<string> list && list.Count > maxNumberOfItems)
                return new ValidationResult($"The list exceeds the maximum allowed number of {maxNumberOfItems} items.");

            return ValidationResult.Success;
        }
    }
}