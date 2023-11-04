using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Notifications
{
    public record CreateUserNotification(User user) : INotification;

    public class AuditLogHandler : INotificationHandler<CreateUserNotification>
    {
        private readonly ILogger<AuditLogHandler> _logger;

        public AuditLogHandler(ILogger<AuditLogHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CreateUserNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Added User");
            return Task.CompletedTask;
        }
    }
}
