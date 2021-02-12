using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Controllers
{
    //essse decorator permite que o envio de um objeto possa ser reconhecido como enviado pelo body
    //por isso não precisa colocar o decorator [FromBody] no controller CreateActivity
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext
            .RequestServices.GetService<IMediator>();
    }
}