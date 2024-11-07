using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Auth.Login
{
    //Giriş için kullanılacak request
    //Geriye Result Pattern içerisinde döner.
    public sealed record LoginCommand(
        string EmailOrUserName,
        string Password) : IRequest<Result<LoginCommandResponse>>;
}
