using Application.Abstration;
using Application.Users.Notifications;
using Ardalis.GuardClauses;
using Domain;
using MediatR;

namespace Application.Users.Commands
{
    public record CreateUserCommand(User user) : ICommand<User> { }

    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository _repository;
        private readonly IPublisher _publisher;

        public CreateUserCommandHandler(IUserRepository repository, IPublisher publisher)
        {
            Guard.Against.Null(repository);
            Guard.Against.Null(publisher);

            _repository = repository;
            _publisher = publisher;
        }

        public async Task<User> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await _repository.Add(request.user);

            await _publisher.Publish(new CreateUserNotification(result), cancellationToken);

            return result;
        }
    }
}
