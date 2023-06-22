using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MyJournalist.Worker.Config;


public class TimeSpanRangeAttribute : ValidationAttribute
{
    public TimeSpan MinValue { get; set; }
    public TimeSpan MaxValue { get; set; }

    public TimeSpanRangeAttribute(string minValue, string maxValue)
    {
        MinValue = TimeSpan.Parse(minValue);
        MaxValue = TimeSpan.Parse(maxValue);
    }

    protected override ValidationResult IsValid([AllowNull] object value, ValidationContext validationContext)
    {
        if (value is TimeSpan timeSpanValue)
        {
            if (timeSpanValue < MinValue || timeSpanValue > MaxValue)
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success!;
    }

}
