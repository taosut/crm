using System.Threading.Tasks;
using CRM.Contact.Contract.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRM.Contact.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IMediator _mediator;
        public ContactsController(ILogger<ContactsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("ping")]
        public ActionResult<string> Ping()
        {
            _logger.LogInformation("Calling ping...");
            return Ok("Pong");
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _mediator.Send(new FindAllContactsQuery());
            return Ok(result);
        }
    }
}