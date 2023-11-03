using MediatR;

namespace Application.Abstration
{
    public abstract record Query<T> : IRequest<T>;

    public abstract class QueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : Query<TResponse>
    {
        public QueryHandler()
        {
        }

        public async Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request);
        }

        protected abstract Task<TResponse> HandleAsync(TQuery request);
    }
}
