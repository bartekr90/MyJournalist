namespace MyJournalist.Email.Config.Abstract
{
    public interface IEmailConfig
    {
        string DefaultEmailSubject { get; set; }
        string Recipient { get; set; }
        string ViewsFullPath { get; set; }
    }
}