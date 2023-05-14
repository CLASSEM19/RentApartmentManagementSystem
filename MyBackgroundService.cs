using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using ApartmentRentManagementSystem.Interfaces.Services;
using System.Threading;
using ApartmentRentManagementSystem.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using ApartmentRentManagementSystem.Context;
using ApartmentRentManagementSystem.Implementations.Repositories;
using ApartmentRentManagementSystem.Interfaces.Repositories;
namespace ApartmentRentManagementSystem
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MyBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected async override Task ExecuteAsync(CancellationToken token)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var paymentDue = scope.ServiceProvider.GetRequiredService<IPaymentDueService>();
            var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
            var apartmentRepository = scope.ServiceProvider.GetRequiredService<IApartmentRepository>();
            var mail = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            await paymentDue.SendPaymentDueMessage();
            System.Console.WriteLine("Hello Wonderful");
            await Task.CompletedTask;
        }
    }
}