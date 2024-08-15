using TheatreBookingSystem.Events;

namespace TheatreBookingSystem.Tests.Mocks;

public class MockDomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; private set; }

    public Guid Id => Guid.NewGuid();

    public MockDomainEvent()
    {
        OccurredOn = DateTimeProvider.UtcNow;
    }
}
