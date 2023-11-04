using System.Windows.Input;
using Application.Abstration;
using Domain;

namespace Application.Users.Commands
{
    public record CreateUserCommand(User user) : ICommand<User> { }

    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository _repository;

        public CreateUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken
        ) => await _repository.Add(request.user);
    }
}
