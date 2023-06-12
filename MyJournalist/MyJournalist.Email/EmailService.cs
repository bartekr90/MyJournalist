using FluentEmail.Core;
using Microsoft.Extensions.Configuration;

namespace MyJournalist.Email;

public class EmailService<T>
{
    private readonly IFluentEmail _email;
    private readonly IConfiguration _config;
    private readonly string _recipient;
    private readonly string _viewPath;

    public EmailService(IFluentEmail email, IConfiguration config, string viewsDir = "Views")
    {
        _email = email;
        _config = config;
        _recipient = _config.GetValue<string>("EmailSettings:Recipient") ?? "defaultRecipient";
        string defaultPath = Path.Combine(Directory.GetCurrentDirectory(), viewsDir);

        string? pathFromSettings = _config.GetValue<string>("DirSettings:ViewsPath");

        if (string.IsNullOrWhiteSpace(pathFromSettings))
            pathFromSettings = null;

        _viewPath = pathFromSettings ?? defaultPath;
    }

    public async Task<int> SendEmailAsync(T model, string subject, string plainTextBody = "", CancellationToken? stoppingToken = null)
    {
        var type = model?.GetType().Name;
        var viewName = type + ".cshtml" ?? "default.cshtml";
        var fullPath = Path.Combine(_viewPath, viewName);

        _email
        .To(_recipient)
        .Subject(subject)
        .Header("Type", type)
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
