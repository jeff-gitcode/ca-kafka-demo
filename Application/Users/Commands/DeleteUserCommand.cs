using Application.Abstration;
using Domain;

namespace Application.Users.Commands
{
    public record DeleteUserCommand(string id) : ICommand<User>
    {
    }

    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, User>
    {
        public Task<User> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
