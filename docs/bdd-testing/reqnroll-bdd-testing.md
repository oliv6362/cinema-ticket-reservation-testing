# Reqnroll BDD Testing

This document describes the BDD/Reqnroll test setup used in the Cinema Ticket Reservation System.

BDD is included as a supporting technique in the project. The goal is not to create a large BDD suite, but to show how selected functional requirements can be expressed as readable Gherkin scenarios and executed as automated tests.

## Purpose

The Reqnroll tests demonstrate functional testing using business-readable scenarios.

The scenarios focus on reservation behavior that is also covered elsewhere in the project through requirements, black-box test design, unit tests, and API tests.

The purpose for BBD tests is to complement the unit and API tests rather replacing them.

## Location

```text
tests/Cinema.BddTests/
├── Features/
│   └── Reservation.feature
└── StepDefinitions/
    └── ReservationStepDefinitions.cs
```

## Covered Scenarios

The BDD feature file covers three selected scenarios:

| Scenario | Purpose |
|---|---|
| Customer successfully reserves an available seat | Verifies the happy path for seat reservation |
| Customer is too young for the movie | Verifies the age restriction rule |
| Seat is already reserved | Verifies rejection when a requested seat is unavailable |

## How It Works

The feature file describes behavior in Gherkin syntax:

```gherkin
Scenario: Customer is too young for the movie
    Given a screening exists with a movie age rating of 15
    And seat A1 is available
    And the current time is before the screening starts
    When a customer aged 14 reserves seat A1
    Then the reservation should be rejected with error code CUSTOMER_TOO_YOUNG
```

The step definition file maps the Gherkin steps to C# code.

The tests execute the real `ReservationService` from `Cinema.Core`, using:

- `FakeTimeProvider` to control the current time
- `PricingService` for price calculation
- in-memory `Screening` and `Seat` objects for test setup

This keeps the scenarios deterministic and independent from the API and system clock.
## Running the Tests

Run all tests from the solution root:

```bash
dotnet test
```

This runs both the unit tests and the Reqnroll BDD tests.

