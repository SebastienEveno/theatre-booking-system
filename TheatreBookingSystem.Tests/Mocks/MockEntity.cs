﻿using TheatreBookingSystem.Domain;
using TheatreBookingSystem.Events;

namespace TheatreBookingSystem.Tests.Mocks;

public class MockEntity : Entity
{
	public DateTime LastEventOccurred { get; private set; }

	public MockEntity() : base() { }

	public MockEntity(Guid id) : base(id) { }

	public void AddDomainEvent()
	{
		var domainEvent = new MockDomainEvent();
		
		AddDomainEvent(domainEvent);
	}

	public override void Apply(IDomainEvent domainEvent)
	{
		if (domainEvent is MockDomainEvent mockDomainEvent)
		{
			LastEventOccurred = mockDomainEvent.OccurredOn;
		}
	}
}
