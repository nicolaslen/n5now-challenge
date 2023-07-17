using System;

namespace N5.Core.Domain.Events;

public class PermissionRequestedEvent
{
    public Guid Id { get; }
    public string OperationName { get; }
    
    public PermissionRequestedEvent(string operationName)
    {
        Id = Guid.NewGuid();
        OperationName = operationName;
    }
}