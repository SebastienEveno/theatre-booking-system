namespace TheatreBookingSystem.Events;

public sealed record SeatRemovedDomainEvent : IDomainEvent
{
	public Guid Id { get; init; }
	public DateTime OccurredOn { get; init; }
	public Guid TheatreId { get; init; }
	public Guid SeatId { get; init; }

    public SeatRemovedDomainEvent(Guid theatreId, Guid seatId)
    {
		Id = Guid.NewGuid();
		OccurredOn = DateTime.UtcNow;
		TheatreId = theatreId;
		SeatId = seatId;
	}
}
