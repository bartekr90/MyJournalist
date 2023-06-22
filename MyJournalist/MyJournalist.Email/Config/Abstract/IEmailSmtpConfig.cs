using System.Net.Mail;

namespace MyJournalist.Email.Config.Abstract;

public interface IEmailSmtpConfig
{
    bool EnableSsl { get; set; }
    bool EnableSslLocal { get; set; }
    string HostSmtp { get; set; }
    string HostSmtpLocal { get; set; }
    int Port { get; set; }
    int PortLocal { get; set; }
    string SenderEmail { get; set; }
    string SenderEmailPassword { get; set; }
    bool UseDefaultCredentials { get; set; }
    bool UseLocalSettings { get; set; }

    SmtpClient GetClient();
}