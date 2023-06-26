using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using MyJournalist.Email.Abstract;

namespace MyJournalist.Email.Config;

public class EmailSmtpConfig : IEmailSmtpConfig
{
    [Required(ErrorMessage = "HostSmtp is required")]
    [StringLength(100, ErrorMessage = "HostSmtp cannot be longer than 100 characters")]
    public string HostSmtp { get; set; }

    [Required(ErrorMessage = "EnableSsl is required")]
    public bool EnableSsl { get; set; }

    [Required(ErrorMessage = "Port is required")]
    [Range(0, 65535, ErrorMessage = "Port must be between 0 and 65535")]
    public int Port { get; set; }

    [Required(ErrorMessage = "SenderEmail is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string SenderEmail { get; set; }

    [Required(ErrorMessage = "SenderEmailPassword is required")]
    public string SenderEmailPassword { get; set; }

    [Required(ErrorMessage = "UseDefaultCredentials is required")]
    public bool UseDefaultCredentials { get; set; }

    [Required(ErrorMessage = "UseLocalSettings is required")]
    public bool UseLocalSettings { get; set; }

    [StringLength(100, ErrorMessage = "HostSmtpLocal cannot be longer than 100 characters")]
    public string HostSmtpLocal { get; set; }

    [Required(ErrorMessage = "EnableSsl is required")]
    public bool EnableSslLocal { get; set; }

    [Range(0, 65535, ErrorMessage = "PortLocal must be between 0 and 65535")]
    public int PortLocal { get; set; }


    public EmailSmtpConfig()
    {
        HostSmtp = "";
        HostSmtpLocal = "";
        SenderEmailPassword = "";
        SenderEmail = "";
    }
    public SmtpClient GetClient()
    {
        if (UseLocalSettings)
            return new SmtpClient()
            {
                Host = HostSmtpLocal,
                EnableSsl = EnableSslLocal,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = PortLocal
            };
        else
            return new SmtpClient()
            {
                Host = HostSmtp,
                EnableSsl = EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = Port,
                UseDefaultCredentials = UseDefaultCredentials,
                Credentials = new NetworkCredential(SenderEmail, SenderEmailPassword)
            };
    }

}
