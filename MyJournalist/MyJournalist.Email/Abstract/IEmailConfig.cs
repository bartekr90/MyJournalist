namespace MyJournalist.Email.Abstract
{
    public interface IEmailConfig
    {
        string DefaultEmailSubject { get; set; }
        string Recipient { get; set; }
        string ViewsFullPath { get; set; }
    }
}