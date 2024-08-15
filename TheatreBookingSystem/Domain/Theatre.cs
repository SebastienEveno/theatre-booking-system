using TheatreBookingSystem.Events;

namespace TheatreBookingSystem.Domain;

public class Theatre : Entity
{
	private List<Seat> _seats;

	public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();

	public Theatre()
	{
		_seats = new List<Seat>();
	}

	public Theatre(Guid id, List<Seat> seats) : base(id)
	{
		_seats = seats;
	}

	public void AddSeat(Seat seat)
	{
		if (_seats.Any(s => s.Id == seat.Id))
		{
			throw new InvalidOperationException("Seat already exists");
		}

		_seats.Add(seat);

		AddDomainEvent(new SeatAddedDomainEvent(Id, seat.Id));
	}

	public void RemoveSeat(Guid seatId)
	{
		var seat = _seats.FirstOrDefault(s => s.Id == seatId);
		
		if (seat == null)
		{
			throw new InvalidOperationException("Seat not found");
		}
		
		_seats.Remove(seat);

		AddDomainEvent(new SeatRemovedDomainEvent(Id, seat.Id));
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
		else if (@event is SeatAddedDomainEvent seatAddedDomainEvent)
		{
			var seat = _seats.SingleOrDefault(s => s.Id == seatAddedDomainEvent.SeatId);

			if (seat == null)
			{
				seat = new Seat(seatAddedDomainEvent.SeatId);

				_seats.Add(seat);
			}
		}
	}
}
