using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DriveForum.Models
{
    public class EnglishStringSyntaxAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!Regex.IsMatch(value.ToString()!, @"^[a-zA-Z]+$"))
                {
                    return new ValidationResult("Используйте только кириллицу и цифры!");
                }
            }
            return ValidationResult.Success!;
        }
    }
}
