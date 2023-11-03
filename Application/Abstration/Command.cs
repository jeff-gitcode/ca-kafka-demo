using Domain;
using MediatR;

namespace Application.Abstration
{
    public abstract record CommandBase<T> : IRequest<T>;
    public abstract record Command : CommandBase<Unit>;

    public abstract class CommandHandler<TCommand,TModel> : IRequestHandler<TCommand, TModel>
    where TCommand : CommandBase<TModel>
    where TModel: User
    {
        protected CommandHandler()
        {

        }

        public abstract Task<TModel> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
