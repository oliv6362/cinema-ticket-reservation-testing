using Cinema.Core.Entities;

namespace Cinema.Core.Interfaces;

public interface ICinemaStore
{
    List<Screening> Screenings { get; }

    List<Reservation> Reservations { get; }

    void Reset();
}