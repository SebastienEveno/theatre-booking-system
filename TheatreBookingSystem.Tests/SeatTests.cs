using TheatreBookingSystem.Domain;

namespace TheatreBookingSystem.Tests;

public class SeatTests
{
	[Test]
	public void Constructor_ShouldInitializeWithGivenIdAndNotBooked()
	{
		// Arrange
		var seatId = Guid.NewGuid();

		// Act
		var seat = new Seat(seatId);

		// Assert
		Assert.That(seat.Id, Is.EqualTo(seatId));
		Assert.That(seat.IsBooked, Is.False);
	}

	[Test]
	public void Book_ShouldSetIsBookedToTrue_WhenSeatIsNotAlreadyBooked()
	{
		// Arrange
		var seatId = Guid.NewGuid();
		var seat = new Seat(seatId);

		// Act
		seat.Book();

		// Assert
		Assert.That(seat.IsBooked, Is.True);
	}

	[Test]
	public void Book_ShouldThrowException_WhenSeatIsAlreadyBooked()
	{
		// Arrange
		var seatId = Guid.NewGuid();
		var seat = new Seat(seatId);
		seat.Book(); // Book the seat first

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() => seat.Book());
		Assert.That(exception.Message, Is.EqualTo("Seat already booked"));
	}
}
