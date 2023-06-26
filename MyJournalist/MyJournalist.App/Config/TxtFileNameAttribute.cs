using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MyJournalist.App.Config;

public class TxtFileNameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid([AllowNull] object value, ValidationContext validationContext)
    {
        if (value is string fileName)
        {
            if (!fileName.EndsWith(".txt"))
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success!;
    }
}
