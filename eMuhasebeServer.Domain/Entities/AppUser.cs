using Microsoft.AspNetCore.Identity;

namespace eMuhasebeServer.Domain.Entities
{
    //identity kütüphanesi kullanıldı. User için inherit edildi. İçinde Id,UserName,Email,Password var. Orada olmayan FirstName ve LastName eklendi.
    public sealed class AppUser : IdentityUser<Guid> 
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        
        //bu class new lendiğinde instance oluşturulurken tam adı veren FullName oluşturuldu. Db de kolon olarak görünmez.
        public string FullName => string.Join(" ", FirstName, LastName);
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
    }
}
