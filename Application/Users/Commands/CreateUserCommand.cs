using System.Windows.Input;
using Application.Abstration;
using Domain;

namespace Application.Users.Commands
{
    public record CreateUserCommand(User user) : ICommand<User>
    {
    }

    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
    {
        public Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
