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

	[Test]
	public void Cancel_ShouldSucceed_WhenSeatIsBooked()
	{
		// Arrange
		var seatId = Guid.NewGuid();
		var seat = new Seat(seatId);
		seat.Book(); // Book the seat initially

		// Act
		seat.Cancel();

		// Assert
		Assert.That(seat.IsBooked, Is.False, "Seat should be unbooked");
	}

	[Test]
	public void Cancel_ShouldThrowException_WhenSeatIsNotBooked()
	{
		// Arrange
		var seatId = Guid.NewGuid();
		var seat = new Seat(seatId); // Seat is not booked

		// Act & Assert
		var ex = Assert.Throws<InvalidOperationException>(() => seat.Cancel());
		Assert.That(ex.Message, Is.EqualTo("Seat is not booked"));
	}
}
