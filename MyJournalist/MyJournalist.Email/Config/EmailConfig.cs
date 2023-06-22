using System.ComponentModel.DataAnnotations;
using MyJournalist.Email.Config.Abstract;

namespace MyJournalist.Email.Config;

public class EmailConfig : IEmailConfig
{
    [Required(ErrorMessage = "Recipient is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Recipient { get; set; }

    [DirectoryExists(ErrorMessage = "The directory does not exist and could not be created.")]
    public string ViewsFullPath { get; set; }

    [Required(ErrorMessage = "The Default Email Subject field is required.")]
    [StringLength(40, ErrorMessage = "The Default Email Subject must be at most 40 characters long.")]
    public string DefaultEmailSubject { get; set; }

    public EmailConfig()
    {
        Recipient = "";
        ViewsFullPath = "";
        DefaultEmailSubject = "";
    }
}
