using FluentEmail.Core;
using MyJournalist.Email.Abstract;
using System.Text;

namespace MyJournalist.Email;

public class EmailService<T> : IEmailService<T>
{
    private readonly IFluentEmail _email;
    private readonly string _recipient;
    private readonly string _viewPath;
    private readonly string _defaultSubject;

    public EmailService(IFluentEmail email, IEmailConfig config, string viewsDir = "Views")
    {
        _email = email;
        _recipient = config.Recipient;
        _defaultSubject = config.DefaultEmailSubject;

        string defaultPath = Path.Combine(Directory.GetCurrentDirectory(), viewsDir);

        _viewPath = string.IsNullOrWhiteSpace(config.ViewsFullPath) ? defaultPath : config.ViewsFullPath;
    }

    public async Task<int> SendEmailAsync(T model, string subject = "", string plainTextBody = "", CancellationToken? stoppingToken = null)
    {
        StringBuilder sb = new StringBuilder();
        var type = model?.GetType().Name ?? "default";
        sb.Append(type);
        sb.Append(".cshtml");

        var fullPath = Path.Combine(_viewPath, sb.ToString());
        var sub = string.IsNullOrWhiteSpace(subject) ? _defaultSubject : subject;

        _email
        .To(_recipient)
        .Subject(sub)
        .UsingTemplateFromFile(fullPath, model)
        .PlaintextAlternativeBody(plainTextBody);

        var response = await _email.SendAsync(stoppingToken);

        if (response.Successful)
        {
            return 1;
            // LOGOWANIE
        }
        else
        {
            return -1;
            //LOGOWANIE
            foreach (var error in response.ErrorMessages)
            {
                // Console.WriteLine(error);
            }
        }
    }
}
