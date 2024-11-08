using eMuhasebeServer.Domain.Entities;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eMuhasebeServer.Infrastructure.Context
{
    //database bağlantısı EF kullanan ApplicationDbContext, identity kütüphanesini kullandığım için IdentityDbContext ekledim User ve role yapısı olmadığı için Role yapısı verildi. Kendi repository patternım dan gelen IUnitOfWork verdim. Kayıt işlemlerin unitofwork interface i kullanılacak.
    internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //eklenen bir entity varsa entity nin ayarlarını bu dosyada değil ayrı bir dosyada yapmak tercih edildiği için IEntityTypeConfiguration etmişleri otomatik bulmasını ve buraya dahil etmesini söyleyen assembly verilen metod çağrıldı.

            builder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);

            //identity de kullanılmaycak classlar db de oluşmasın diye ignore edildi.

            builder.Ignore<IdentityUserLogin<Guid>>();
            builder.Ignore<IdentityRoleClaim<Guid>>();
            builder.Ignore<IdentityUserToken<Guid>>();
            builder.Ignore<IdentityUserRole<Guid>>();
            builder.Ignore<IdentityUserClaim<Guid>>();
        }
    }
}
