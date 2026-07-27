using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeaveApp.Business.Common;
using LeaveApp.Business.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace LeaveApp.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendNewLeaveRequestNotificationAsync(
            List<string> managerEmails,
            string employeeFullName,
            string leaveTypeName,
            DateTime startDate,
            DateTime endDate,
            string description)
        {
            if (managerEmails == null || managerEmails.Count == 0)
                return;

            var subject = $"Yeni İzin Talevi - {employeeFullName}";

            var body = $@"
        <h3>Yeni Bir İzin Talebi Oluşturuldu</h3>
        <p><strong>Personel:</strong> {employeeFullName}</p>
        <p><strong>İzin Türü:</strong> {leaveTypeName}</p>
        <p><strong>Tarih Aralığı:</strong> {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}</p>
        <p><strong>Açıklama</strong> {(string.IsNullOrWhiteSpace(description) ? "-" : description)}</p>
        <p>Talebi incelemek için lütfen sisteme giriş yapın.</p>
    ";

            foreach (var managerEmail in managerEmails)
            {
                await SendEmailAsync(managerEmail, subject, body);
            }
        }

        public async Task SendLeaveStatusUpdateNotificationAsync(
            string employeeEmail,
            string employeeFullName,
            string leaveTypeName,
            DateTime startDate,
            DateTime endDate,
            bool isApproved)
        {
            if (string.IsNullOrWhiteSpace(employeeEmail))
                return;

            var statusText = isApproved ? "Onaylandı" : "Reddedildi";
            var subject = $"İzin Talebiniz {statusText}";

            var body = $@"
                <h3>İzin Talebinizde Güncelleme Var</h3>
                <p>Sayın {employeeFullName}, </p>
                <p><strong>{leaveTypeName}</strong> türündeki, <strong>{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}</strong> tarihli izin talebiniz
                <strong>{statusText}</strong>.<p>
                <p>Detaylar için lütfen sisteme giriş yapın.
            ";

            await SendEmailAsync(employeeEmail, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                client.Timeout = 15000;
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Console.WriteLine($"Mail başarıyla gönderildi: {toEmail}");
            }
            catch (Exception ex)
            {
                //İleride buraya loglama eklenebilir.
                Console.WriteLine($"Mail gönderilemedi ({toEmail}): {ex.Message}");
            }
        }
    }
}
