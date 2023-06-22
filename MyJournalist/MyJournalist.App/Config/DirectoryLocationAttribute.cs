using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MyJournalist.App.Config;

public class DirectoryLocationAttribute : ValidationAttribute
{
    public DirectoryLocationAttribute() : base("The directory does not exist and could not be created.")
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
