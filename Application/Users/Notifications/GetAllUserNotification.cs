using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Notifications
{
    public record GetAllUserNotification(IEnumerable<User> users) : INotification;

    public class GetAllUserAuditLogHandler : INotificationHandler<GetAllUserNotification>
    {
        private readonly ILogger<GetAllUserAuditLogHandler> _logger;

        public GetAllUserAuditLogHandler(ILogger<GetAllUserAuditLogHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(GetAllUserNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get Users");
            return Task.CompletedTask;
        }
    }

}
