using System.Net.Mail;
using System.Net;

namespace MyJournalist.Email;

public static class EmailSetup
{
    public static string HostSmtp { get; set; } = "localhost";
    public static bool EnableSsl { get; set; } = false;
    public static int Port { get; set; } = 25;
    public static string SenderEmail { get; set; } = "defaultSenderEmail";
    public static string SenderEmailPassword { get; set; } = "defaultSenderEmailPassword";
    public static bool DefaultCredentials { get; set; } = true;
   
    public static bool LocalSettings { get; set; } = true;
    public static string HostSmtpLocal { get; set; } = "localhost";
    public static bool EnableSslLocal { get; set; } = false;
    public static int PortLocal { get; set; } = 25;

    public static SmtpClient GetClient()
    {
        if (LocalSettings)
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
                UseDefaultCredentials = DefaultCredentials,
                Credentials = new NetworkCredential(SenderEmail, SenderEmailPassword)
            };
    }
}
