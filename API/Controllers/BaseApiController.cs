using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Controllers
{
    //essse decorator permite que o envio de um objeto possa ser reconhecido como enviado pelo body
    //por isso não precisa colocar o decorator [FromBody] no controller CreateActivity
    //o atributo [ApiController] gera automaticamente respostas HTTP 400 se encontrar algum erro de validação
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext
            .RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }
    }
}