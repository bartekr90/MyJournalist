using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MyJournalist.Email.Config;

public class DirectoryExistsAttribute : ValidationAttribute
{
    public DirectoryExistsAttribute() : base("The directory does not exist and could not be created.")
    {
    }

    protected override ValidationResult IsValid([AllowNull] object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success!;

        var directoryPath = value.ToString();

        if (string.IsNullOrEmpty(directoryPath))
            return ValidationResult.Success!;

        if (Directory.Exists(directoryPath))
            return ValidationResult.Success!;
        else
            return new ValidationResult($"Failed to find directory: {directoryPath}");
    }
}