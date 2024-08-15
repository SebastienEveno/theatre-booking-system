using TheatreBookingSystem.Domain;
using TheatreBookingSystem.Events;

namespace TheatreBookingSystem.Tests;

public class TheatreTests
{
	private Guid _theatreId;
	private Guid _seatId;

	[SetUp]
	public void SetUp()
	{
		_theatreId = Guid.NewGuid();
		_seatId = Guid.NewGuid();
	}

	[Test]
	public void AddSeat_Should_Add_Seat_When_Seat_Does_Not_Exist()
	{
		// Arrange
		var theatre = new Theatre(_theatreId, new List<Seat>());
		var seat = new Seat(_seatId);

		// Act
		theatre.AddSeat(seat);

		// Assert
		Assert.That(theatre.Seats.ToList(), Does.Contain(seat));

		Assert.That(theatre.DomainEvents.Count, Is.EqualTo(1));
		Assert.That(theatre.DomainEvents.First(), Is.TypeOf(typeof(SeatAddedDomainEvent)));
		Assert.That(((SeatAddedDomainEvent)theatre.DomainEvents.First()).TheatreId, Is.EqualTo(_theatreId));
		Assert.That(((SeatAddedDomainEvent)theatre.DomainEvents.First()).SeatId, Is.EqualTo(_seatId));
	}

	[Test]
	public void AddSeat_Should_Throw_Exception_When_Seat_Already_Exists()
	{
		// Arrange
		var seat = new Seat(_seatId);
		var theatre = new Theatre(_theatreId, new List<Seat> { seat });

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() => theatre.AddSeat(new Seat(_seatId)));
		Assert.That(exception.Message, Is.EqualTo("Seat already exists"));

		Assert.That(theatre.DomainEvents, Is.Empty);
	}

	[Test]
	public void RemoveSeat_Should_Remove_Seat_When_Seat_Exists()
	{
		// Arrange
		var seat = new Seat(_seatId);
		var theatre = new Theatre(_theatreId, new List<Seat> { seat });

		// Act
		theatre.RemoveSeat(_seatId);

		// Assert
		Assert.That(theatre.Seats.Any(s => s.Id == _seatId), Is.False);

		Assert.That(theatre.DomainEvents.Count, Is.EqualTo(1));
		Assert.That(theatre.DomainEvents.First(), Is.TypeOf(typeof(SeatRemovedDomainEvent)));
		Assert.That(((SeatRemovedDomainEvent)theatre.DomainEvents.First()).TheatreId, Is.EqualTo(_theatreId));
		Assert.That(((SeatRemovedDomainEvent)theatre.DomainEvents.First()).SeatId, Is.EqualTo(_seatId));
	}

	[Test]
	public void RemoveSeat_Should_Throw_Exception_When_Seat_Does_Not_Exist()
	{
		// Arrange
		var theatre = new Theatre();

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() => theatre.RemoveSeat(Guid.NewGuid()));
		Assert.That(exception.Message, Is.EqualTo("Seat not found"));

		Assert.That(theatre.DomainEvents, Is.Empty);
	}

	[Test]
	public void BookSeat_ShouldBookSeat_WhenSeatExistsAndIsNotBooked()
	{
		// Arrange
		var seats = new List<Seat> { new Seat(_seatId) };
		var theatre = new Theatre(_theatreId, seats);

		// Act
		theatre.BookSeat(_seatId);

		// Assert
		Assert.That(seats.First().IsBooked, Is.True);
		Assert.That(theatre.DomainEvents.Count, Is.EqualTo(1));
		Assert.That(theatre.DomainEvents.First(), Is.TypeOf(typeof(SeatBookedDomainEvent)));
		Assert.That(((SeatBookedDomainEvent)theatre.DomainEvents.First()).TheatreId, Is.EqualTo(_theatreId));
		Assert.That(((SeatBookedDomainEvent)theatre.DomainEvents.First()).SeatId, Is.EqualTo(_seatId));
	}

	[Test]
	public void BookSeat_ShouldThrowException_WhenSeatDoesNotExist()
	{
		// Arrange
		var seats = new List<Seat> { new Seat(_seatId) };
		var theatre = new Theatre(_theatreId, seats);

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() => theatre.BookSeat(Guid.NewGuid()));
		Assert.That(exception.Message, Is.EqualTo("Seat not found"));

		Assert.That(theatre.DomainEvents, Is.Empty);
	}

	[Test]
	public void BookSeat_ShouldThrowException_WhenSeatIsAlreadyBooked()
	{
		// Arrange
		var seats = new List<Seat> { new Seat(_seatId) };
		var theatre = new Theatre(_theatreId, seats);
		theatre.BookSeat(_seatId); // Book the seat first

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() => theatre.BookSeat(_seatId));
		Assert.That(exception.Message, Is.EqualTo("Seat already booked"));
	}

	[Test]
	public void Apply_ShouldUpdateSeatState_WhenEventIsApplied()
	{
		// Arrange
		var seats = new List<Seat> { new Seat(_seatId) };
		var theatre = new Theatre(_theatreId, seats);
		var seatBookedEvent = new SeatBookedDomainEvent(_theatreId, _seatId);

		// Act
		theatre.Apply(seatBookedEvent);

		// Assert
		Assert.That(seats.First().IsBooked, Is.True);

		Assert.That(theatre.DomainEvents, Is.Empty);
	}

	[Test]
	public void ClearDomainEvents_ShouldRemoveAllDomainEvents()
	{
		// Arrange
		var seats = new List<Seat> { new Seat(_seatId) };
		var theatre = new Theatre(_theatreId, seats);
		theatre.BookSeat(_seatId);

		// Act
		theatre.ClearDomainEvents();

		// Assert
		Assert.That(theatre.DomainEvents, Is.Empty);
	}
}