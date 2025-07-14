namespace BaseCleanArchitecture.Domain.Interfaces
{
    public interface IMailService
    {
        Task SendEmailNotificationAsync(INotification notification);
    }

    public interface INotification
    {
        string Template { get; set; }
        string Module { get; set; }
        List<string> Recipients { get; set; }
        List<string> RecipientsCopy { get; set; }
        List<string> RecipientsBlindCopy { get; set; }
        IDictionary<string, string> Params { get; set; }
        string Subject { get; set; }
    }
} 