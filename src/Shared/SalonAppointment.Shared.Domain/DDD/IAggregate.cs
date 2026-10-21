namespace SalonAppointment.Shared.Domain.DDD;

public interface IAggregate<out T> : IAggregate, IEntity<T>
{
}

public interface IAggregate
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}