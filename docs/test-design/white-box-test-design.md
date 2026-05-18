# White-Box Testing

This document describes the small white-box testing example used in the Cinema Ticket Reservation System.

White-box testing is included as a supporting technique in the project. The goal is not to create a large structural test suite, but to show how source code inspection can reveal a branch that should be exercised by a test.

## Purpose

The white-box test focuses on the internal control flow of one selected method:

```csharp
ReservationService.ReserveSeats(...)
```

This method was selected because it contains the main reservation control flow and several business-rule decisions.

## Selected Method

`ReserveSeats(...)` checks several conditions before creating a reservation:

1. The reservation must contain at least one seat.
2. The reservation must be made before the screening starts.
3. The customer must satisfy the movie age rating.
4. All requested seats must exist.
5. All requested seats must be available.
6. If all checks pass, the reservation is created.

## Structural Observation

During source code inspection, the following branch was identified:

```csharp
if (requestedSeats is null || requestedSeats.Count == 0)
{
    return ReservationResult.Fail(
        "NO_SEATS_REQUESTED",
        "A reservation must contain at least one seat.");
}
```

This branch represents the case where a reservation request contains no seats.

Although the rule is also described in the requirements, this specific test was added after inspecting the source code. Therefore, it is treated as a white-box/structural test.

## Added White-Box Unit Test

```csharp
[Fact]
public void ReserveSeats_WB_SEATCOUNT_01_NoSeatsRequested_ShouldRejectReservation()
{
    // Arrange
    var screening = TestCinemaData.CreateDefaultScreening();
    var requestedSeats = new List<string>();

    // Act
    var result = _reservationService.ReserveSeats(screening, requestedSeats, customerAge: 18);

    // Assert
    Assert.False(result.Success);
    Assert.Equal("NO_SEATS_REQUESTED", result.ErrorCode);
}
```

## Role in the Overall Test Strategy

The white-box test complements the requirement-based tests.

The black-box tests were derived from requirements and business rules. The white-box test was added by inspecting the implementation and identifying a branch that should be directly exercised.

This demonstrates how structural testing can strengthen a test suite by revealing implementation paths that were not selected in the original black-box test design.
