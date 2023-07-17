using System.Threading.Tasks;
using N5.Core.Domain.Events;

namespace N5.Core.Messaging;

public interface IKafkaProducer
{
    Task PublishEvent(string topic, PermissionRequestedEvent permissionRequestedEvent);
}