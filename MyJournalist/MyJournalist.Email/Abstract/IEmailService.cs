namespace MyJournalist.Email.Abstract;

public interface IEmailService<T>
{
    Task<int> SendEmailAsync(T model, string subject = "", string plainTextBody = "", CancellationToken? stoppingToken = null);
}