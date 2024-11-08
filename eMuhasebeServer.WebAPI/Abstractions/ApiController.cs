using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMuhasebeServer.WebAPI.Abstractions
{
    //her controllerda kullanılan default yapı abstract class haline getirildi. mediatr kütüphanesi kullanıldığı için DI yapıldı.
    [Route("api/[controller]/[action]")]
    [ApiController]
    //eklenilen her sayfa kullancı girişi yapılarak kullanılacak
    [Authorize(AuthenticationSchemes = "Bearer")]
    public abstract class ApiController : ControllerBase
    {
        public readonly IMediator _mediator;

        protected ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
