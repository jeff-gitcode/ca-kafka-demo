using Application.Abstration;
using Application.Users.Notifications;
using Domain;
using MediatR;

namespace Application.Users.Queries
{
    public class GetAllUserQuery : IQuery<IEnumerable<User>> { }

    public class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, IEnumerable<User>>
    {
        private readonly IUserRepository _repository;
        private readonly IPublisher _publisher;

        public GetAllUserQueryHandler(IUserRepository repository, IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<IEnumerable<User>> Handle(
            GetAllUserQuery request,
            CancellationToken cancellationToken
        ) 
        {
            var result = await _repository.GetAll();

            await _publisher.Publish(new GetAllUserNotification(result), cancellationToken);

            return result;
        }
    }
}
