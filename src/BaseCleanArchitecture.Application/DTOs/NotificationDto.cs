using System.Collections.Generic;
using System.IO;
using BaseCleanArchitecture.Domain.Interfaces;

namespace BaseCleanArchitecture.Application.DTOs
{
    public class Notification : INotification
    {
        public string Template { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public List<string> Recipients { get; set; } = new List<string>();
        public List<string> RecipientsCopy { get; set; } = new List<string>();
        public List<string> RecipientsBlindCopy { get; set; } = new List<string>();
        public IDictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
        public MemoryStream? AttachmentFileStream { get; set; }
        public string AttachmentName { get; set; } = string.Empty;
        public string AttachmentMediaType { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public System.Net.Mail.MailPriority? Priority { get; set; }
    }

    public class NotificationContent
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string CopyToEmail { get; set; } = string.Empty;
        public List<string> Recipients { get; set; } = new List<string>();
        public string MessageSubject { get; set; } = string.Empty;
        public string MessageBody { get; set; } = string.Empty;
        public MemoryStream? AttachmentFileStream { get; set; }
        public string AttachmentName { get; set; } = string.Empty;
    }
} 