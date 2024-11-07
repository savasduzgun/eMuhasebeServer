using eMuhasebeServer.Application.Features.Auth.Login;
using eMuhasebeServer.Domain.Entities;

namespace eMuhasebeServer.Application.Services
{
    public interface IJwtProvider
    {
        //JWT üretilecek imza
        Task<LoginCommandResponse> CreateToken(AppUser user);
    }
}
