using TheatreBookingSystem.Events;

namespace TheatreBookingSystem.Tests;

public class SeatBookedDomainEventTests
{
	[Test]
	public void Constructor_ShouldInitializeWithGivenValues()
	{
		// Arrange
		var theatreId = Guid.NewGuid();
		var seatId = Guid.NewGuid();

		// Act
		var @event = new SeatBookedDomainEvent(theatreId, seatId);

		// Assert
		Assert.That(@event.TheatreId, Is.EqualTo(theatreId));
		Assert.That(@event.SeatId, Is.EqualTo(seatId));
	}
}
