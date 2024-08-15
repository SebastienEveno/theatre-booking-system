namespace TheatreBookingSystem.Domain;

public class Seat
{
	public bool IsBooked { get; private set; }
	public Guid Id { get; private set; }

	public Seat(Guid id)
	{
		Id = id;
		IsBooked = false;
	}

	public void Book()
	{
		if (IsBooked)
		{
			throw new InvalidOperationException("Seat already booked");
		}

		IsBooked = true;
	}

	public void Cancel()
	{
		if (!IsBooked)
		{
			throw new InvalidOperationException("Seat is not booked");
		}

		IsBooked = false;
	}
}
