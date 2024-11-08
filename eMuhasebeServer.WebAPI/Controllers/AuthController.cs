using eMuhasebeServer.Application.Features.Auth.Login;
using eMuhasebeServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMuhasebeServer.WebAPI.Controllers
{
    [AllowAnonymous] //kullanıcı kontrolü bu sayfada olamsın
    public sealed class AuthController : ApiController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
        {
            //bütün endpoitlerde kullanılacak standart yapı, response result olarak cevap alıyor, result ın içinde statüs code var başarılı başarısız dönebiliyor normal response da bir değer varsa onu yolluyor cevap olarak
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
    }
}
