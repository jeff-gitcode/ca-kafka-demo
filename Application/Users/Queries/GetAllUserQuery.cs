using Application.Abstration;
using Domain;

namespace Application.Users.Queries
{
    public class GetAllUserQuery: IQuery<IEnumerable<User>>
    {
    }

    public class GetAllUserQueryHandler: IQueryHandler<GetAllUserQuery, IEnumerable<User>>
    {
        private readonly IUserRepository _repository;

        public GetAllUserQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> Handle(GetAllUserQuery request, CancellationToken cancellationToken) => await _repository.GetAll();
    }
}
