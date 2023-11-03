using Application.Users.Commands;
using Application.Users.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private IMediator _mediator;

    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost(Name = "")]
    public async Task<ActionResult<User>> AddUserAsync(CreateUserCommand command)
    {
        _logger.LogInformation("Presentation.Controllers");

        return Ok(await _mediator.Send(command));
    }

    [HttpGet(Name = "")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        _logger.LogInformation("Presentation.Controllers");

        return Ok(await _mediator.Send(new GetAllUserQuery()));
    }

}