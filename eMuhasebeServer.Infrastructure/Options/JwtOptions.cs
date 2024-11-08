namespace eMuhasebeServer.Infrastructure.Options
{
    //JWT oluşturmak için gereken 3 alanın class ı oluşturuldu.
    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
    }
}
 