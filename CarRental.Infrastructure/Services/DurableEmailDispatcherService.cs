using CarRental.Domain.Interfaces;
using CarRental.Shared.DTOs.Email;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infrastructure.Services
{
    public class DurableEmailDispatcherService : IEmailDispatcherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DurableEmailDispatcherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendConfirmationEmailAsync(string to, string subject, string body)
        {
            var payload = new EmailRequestDto { To = to, Subject = subject, Body = body };
            var url = _configuration["DurableFunctions:StartUrl"]; // por ejemplo, http://localhost:7071/api/orchestrators/StartEmailOrchestration
            var response = await _httpClient.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();
        }
    }

}
