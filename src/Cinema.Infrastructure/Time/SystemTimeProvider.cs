using Cinema.Core.Interfaces;

namespace Cinema.Infrastructure.Time;

public class SystemTimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.Now;
}