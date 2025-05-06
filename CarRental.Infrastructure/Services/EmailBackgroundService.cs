using CarRental.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarRental.Infrastructure.Services
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var queueService = scope.ServiceProvider.GetRequiredService<IEmailQueueService>();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            while (!stoppingToken.IsCancellationRequested)
            {
                var (to, subject, body) = await ((EmailQueueService)queueService).DequeueAsync(stoppingToken);

                try
                {
                    await emailSender.SendAsync(to, subject, body);
                }
                catch (Exception ex)
                {
                    // Manejo de errores y reintentos
                    Console.WriteLine($"Error al enviar correo a {to}: {ex.Message}");
                }
            }
        }
    }
}
