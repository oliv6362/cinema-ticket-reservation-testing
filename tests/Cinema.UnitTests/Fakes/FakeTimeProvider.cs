using Cinema.Core.Interfaces;

namespace Cinema.UnitTests.Fakes;

public class FakeTimeProvider : ITimeProvider
{
    public DateTime Now { get; set; }
}