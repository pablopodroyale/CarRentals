using CarRental.Domain.Interfaces;

namespace CarRental.Infrastructure.Services
{
    public class EmailSenderService : IEmailSender
    {
        public Task SendAsync(string to, string subject, string body)
        {
            // Aquí iría la lógica real de envío de correo (SMTP, SendGrid, etc.)
            Console.WriteLine($" Email enviado a {to} con asunto '{subject}'");
            return Task.CompletedTask;
        }
    }
}
