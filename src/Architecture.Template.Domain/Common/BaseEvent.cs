using MediatR;

namespace Domain.Common;
//TODO: Remove BaseEvent and use INotification directly
public abstract class BaseEvent : INotification
{
}