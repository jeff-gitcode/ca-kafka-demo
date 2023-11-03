using Application.Abstration;
using Domain;

namespace Application.Users.Commands
{
    public record CreateUserCommand(User user) : CommandBase<User>
    {
    }

    public class CreateUserCommandHandler : CommandHandler<CreateUserCommand, User>
    {
        public override Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
