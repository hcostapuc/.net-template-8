using Ardalis.GuardClauses;

namespace Domain.Events;
public sealed class WashOrderCreatedEvent(WashOrderEntity washOrderEntity) : BaseEvent
{
    public WashOrderEntity WashOrderEntity { get; } = washOrderEntity ?? 
                                                      Guard.Against.Null(washOrderEntity, nameof(washOrderEntity));
}