using MediatR;

namespace TheatreBookingSystem.Events;

public interface IDomainEvent : INotification
{
	Guid Id { get; }
	DateTime OccurredOn { get; }
}
