# Unit Testing

This document describes the unit testing setup used in the Cinema Ticket Reservation System.

Unit testing is one of the main focus areas of the project. The goal is to verify the core business logic independently from the API layer and infrastructure.

## Purpose

The unit tests verify selected reservation and pricing rules directly in `Cinema.Core`.

The tests are based on:

- Requirements
- Black-box test design
- One selected white-box/structural observation

## Location

```text
tests/Cinema.UnitTests/
├── Fakes/
│   └── FakeTimeProvider.cs
├── Services/
│   ├── PricingServiceTests.cs
│   └── ReservationServiceTests.cs
└── TestData/
    └── TestCinemaData.cs
```

## Tested Services

| Test Class | Service Under Test | Purpose |
|---|---|---|
| `ReservationServiceTests` | `ReservationService` | Tests reservation creation, seat validation, age restrictions, reservation time, cancellation deadline, and one white-box branch |
| `PricingServiceTests` | `PricingService` | Tests ticket price calculation and group discount behavior |

## Test Naming

The test names follow this structure:

```text
MethodUnderTest_TestId_Condition_ExpectedResult
```

Example:

```csharp
ReserveSeats_BB_AGE_01_CustomerYoungerThanMovieRating_ShouldRejectReservation
```

The prefixes mean:

| Prefix | Meaning |
|---|---|
| `BB` | Black-box test case derived from requirements and black-box test design |
| `WB` | White-box test case added after source code inspection |

## Running the Tests

Run all tests from the solution root:

```bash
dotnet test
```
