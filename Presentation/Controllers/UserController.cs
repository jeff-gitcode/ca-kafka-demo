using Application.Users.Commands;
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

    [HttpPost(Name = "addUser")]
    public async Task<User> AddUserAsync(CreateUserCommand command)
    {
        _logger.LogInformation("Presentation.Controllers");

        return await _mediator.Send(command);
    }
}