using TravelAgency.Interfaces.Repositories;
using TravelAgency.Interfaces.Services;
using TravelAgency.Repositories;
using TravelAgency.Services;

namespace TravelAgency.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTravelAgencyServices(
            this IServiceCollection services)
        {
            // Repository registrations
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<
                IDestinationRepository,
                DestinationRepository>();

            services.AddScoped<
                IPackageRepository,
                PackageRepository>();

            services.AddScoped<
                ICustomerRepository,
                CustomerRepository>();

            // Service registrations
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<
                IDestinationService,
                DestinationService>();

            services.AddScoped<
                IPackageService,
                PackageService>();

            services.AddScoped<
                ICustomerService,
                CustomerService>();

            return services;
        }
    }
}