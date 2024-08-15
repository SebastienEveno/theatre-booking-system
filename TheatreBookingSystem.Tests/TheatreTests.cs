using TheatreBookingSystem.Domain;
using TheatreBookingSystem.Events;

namespace TheatreBookingSystem.Tests;

public class TheatreTests
{
	[Test]
	public void BookSeat_ShouldBookSeat_WhenSeatExistsAndIsNotBooked()
	{
		// Arrange
		var theatreId = Guid.NewGuid();
		var seatId = Guid.NewGuid();
		var seats = new List<Seat> { new Seat(seatId) };
		var theatre = new Theatre(theatreId, seats);

		// Act
		theatre.BookSeat(seatId);

		// Assert
		Assert.That(seats.First().IsBooked, Is.True);
		Assert.That(theatre.DomainEvents.Count, Is.EqualTo(1));
		Assert.That(theatre.DomainEvents.First(), Is.TypeOf(typeof(SeatBookedDomainEvent)));
		Assert.That(((SeatBookedDomainEvent)theatre.DomainEvents.First()).TheatreId, Is.EqualTo(theatreId));
		Assert.That(((SeatBookedDomainEvent)theatre.DomainEvents.First()).SeatId, Is.EqualTo(seatId));
	}

	[Test]
	public void BookSeat_ShouldThrowException_WhenSeatDoesNotExist()
	{
		// Arrange
		var theatreId = Guid.NewGuid();
		var seatId = Guid.NewGuid();
		var nonExistentSeatId = Guid.NewGuid();
		var seats = new List<Seat> { new Seat(seatId) };
		var theatre = new Theatre(theatreId, seats);

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() => theatre.BookSeat(nonExistentSeatId));
		Assert.That(exception.Message, Is.EqualTo("Seat not found"));
	}

	[Test]
	public void BookSeat_ShouldThrowException_WhenSeatIsAlreadyBooked()
	{
		// Arrange
		var theatreId = Guid.NewGuid();
		var seatId = Guid.NewGuid();
		var seats = new List<Seat> { new Seat(seatId) };
		var theatre = new Theatre(theatreId, seats);
		theatre.BookSeat(seatId); // Book the seat first

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() => theatre.BookSeat(seatId));
		Assert.That(exception.Message, Is.EqualTo("Seat already booked"));
	}

	[Test]
	public void Apply_ShouldUpdateSeatState_WhenEventIsApplied()
	{
		// Arrange
		var theatreId = Guid.NewGuid();
		var seatId = Guid.NewGuid();
		var seats = new List<Seat> { new Seat(seatId) };
		var theatre = new Theatre(theatreId, seats);
		var seatBookedEvent = new SeatBookedDomainEvent(theatreId, seatId);

		// Act
		theatre.Apply(seatBookedEvent);

		// Assert
		Assert.That(seats.First().IsBooked, Is.True);
	}

	[Test]
	public void ClearDomainEvents_ShouldRemoveAllDomainEvents()
	{
		// Arrange
		var theatreId = Guid.NewGuid();
		var seatId = Guid.NewGuid();
		var seats = new List<Seat> { new Seat(seatId) };
		var theatre = new Theatre(theatreId, seats);
		theatre.BookSeat(seatId);

		// Act
		theatre.ClearDomainEvents();

		// Assert
		Assert.That(theatre.DomainEvents, Is.Empty);
	}
}