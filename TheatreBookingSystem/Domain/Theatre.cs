using TheatreBookingSystem.Events;

namespace TheatreBookingSystem.Domain;

public class Theatre : Entity
{
	private List<Seat> _seats;

	public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();

	public Theatre(List<Seat> seats)
	{
		_seats = seats;
	}

	public Theatre(Guid id, List<Seat> seats) : base(id)
	{
		_seats = seats;
	}

	public void BookSeat(Guid seatId)
	{
		var seat = _seats.FirstOrDefault(s => s.Id == seatId);

		if (seat == null)
		{
			throw new InvalidOperationException("Seat not found");
		}

		seat.Book();

		AddDomainEvent(new SeatBookedDomainEvent(Id, seat.Id));
	}

	// Method for loading state without raising events
	public override void Apply(IDomainEvent @event)
	{
		if (@event is SeatBookedDomainEvent seatBookedDomainEvent)
		{
			var seat = _seats.SingleOrDefault(s => s.Id == seatBookedDomainEvent.SeatId);

			if (seat == null)
			{
				seat = new Seat(seatBookedDomainEvent.SeatId);

				_seats.Add(seat);
			}

			seat.Book();
		}
	}
}
