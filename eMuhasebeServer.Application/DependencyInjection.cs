using eMuhasebeServer.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace eMuhasebeServer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //AutoMapper için DI
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            //CQRS pattern için eklenen MediaTR için DI, MediaTR eklenince behavior özelliği kullanılabiliyor.
            services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
                conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            //FluentValidation için DI
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
