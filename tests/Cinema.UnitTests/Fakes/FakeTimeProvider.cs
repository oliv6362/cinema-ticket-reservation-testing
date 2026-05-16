using Cinema.Core.Interfaces;

namespace Cinema.UnitTests.Fakes;

/// <summary>
/// Fake implementation of <see cref="ITimeProvider"/> used by unit tests.
///
/// This allows tests to control the current time instead of depending on
/// the real system clock. It makes time-dependent tests deterministic,
/// especially for reservation time and cancellation deadline rules.
/// </summary>
public class FakeTimeProvider : ITimeProvider
{
    public DateTime Now { get; set; }
}