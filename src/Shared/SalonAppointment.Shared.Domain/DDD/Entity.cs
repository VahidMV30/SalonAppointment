namespace SalonAppointment.Shared.Domain.DDD;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}