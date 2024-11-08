using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Infrastructure.Context;
using eMuhasebeServer.Infrastructure.Options;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace eMuhasebeServer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //db nin service registration işlemi, mssql kullanıldığı için UseSqlServer verildi. Appsettingjson da SqlServer key ine ait connectionstring verildi.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

            //Kullanılan repository patterndaki unitofwork ApplicationDbContext e direkt implement edildiği için DI yapıldı.
            services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

            //identity kütüphanesinin DI yapıldı ve kuralları yazıldı.
            services
                .AddIdentity<AppUser, IdentityRole<Guid>>(cfr =>
                {
                    cfr.Password.RequiredLength = 1;
                    cfr.Password.RequireNonAlphanumeric = false;
                    cfr.Password.RequireUppercase = false;
                    cfr.Password.RequireLowercase = false;
                    cfr.Password.RequireDigit = false;
                    cfr.SignIn.RequireConfirmedEmail = true;
                    cfr.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    cfr.Lockout.MaxFailedAccessAttempts = 3;
                    cfr.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //JwtOptions class ının potionspattern ile beraber Appsettingjson dosyasından configure ve bind edildiği kayıt
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            //JWT için yapılan ayarları alan service
            services.ConfigureOptions<JwtTokenOptionsSetup>();

            //uygulamaya authentication ve authorization ı ekleyen service registration
            services.AddAuthentication()
                .AddJwtBearer();
            services.AddAuthorizationBuilder();

            //Scrutor kütüphanesini kullanarak Dependency injectionları yani IUserRepository UserRepository diye yazılan ScopedLife, ScopedSingleton ya da transit yaşam döngüsünde yazılan DI ları otomatik yapmasını sağlayan kod. Scrutor bunları otomatik yapıyor bu sayede repository patternda IUserRepository UserRepository oluşturulduğu zaman burada service.addscoped diyerek DI yapmaya gerek kalmıyor.
            services.Scan(action =>
            {
                action
                .FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .AsImplementedInterfaces()
                .WithScopedLifetime();
            });

            return services;
        }
    }
}
