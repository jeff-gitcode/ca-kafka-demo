using Application.Abstration;
using Domain;

namespace Application.Users.Queries
{
    public class GetAllUserQuery: IQuery<IEnumerable<User>>
    {
    }

    public class GetAllUserQueryHandler: IQueryHandler<GetAllUserQuery, IEnumerable<User>>
    {
        public GetAllUserQueryHandler() { }

        public Task<IEnumerable<User>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
