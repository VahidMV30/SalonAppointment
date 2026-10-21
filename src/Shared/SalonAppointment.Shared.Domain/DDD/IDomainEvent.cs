namespace SalonAppointment.Shared.Domain.DDD;

public interface IDomainEvent : INotification
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public string EventType { get; }
}