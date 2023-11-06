using Application.Users.Commands;
using Application.Users.Queries;
using Ardalis.GuardClauses;
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
        Guard.Against.Null(logger);
        Guard.Against.Null(mediator);

        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost(Name = "")]
    public async Task<ActionResult<User>> AddUserAsync(CreateUserCommand command)
    {
        _logger.LogInformation("Presentation.Controllers");

        return Ok(await _mediator.Send(command));
    }

    [HttpDelete(Name = "")]
    public async Task<ActionResult<User>> DeleteUserAsync(DeleteUserCommand command)
    {
        _logger.LogInformation("Presentation.Controllers");

        return Ok(await _mediator.Send(command));
    }

    [HttpGet(Name = "")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
    {
        _logger.LogInformation("Presentation.Controllers");

        return Ok(await _mediator.Send(new GetAllUserQuery()));
    }
}
