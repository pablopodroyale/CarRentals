using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Domain.Interfaces;

namespace CarRental.Infrastructure.Services
{
    public class EmailQueueService : IEmailQueueService
    {
        private readonly ConcurrentQueue<(string To, string Subject, string Body)> _emails = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void EnqueueEmail(string to, string subject, string body)
        {
            _emails.Enqueue((to, subject, body));
            _signal.Release();
        }

        public async Task<(string To, string Subject, string Body)> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _emails.TryDequeue(out var email);
            return email;
        }
    }
}
