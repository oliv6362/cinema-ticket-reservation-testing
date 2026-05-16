namespace Cinema.Core.Interfaces;

public interface ITimeProvider
{
    DateTime Now { get; }
}