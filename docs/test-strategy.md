# Test Strategy

This document describes the overall testing strategy for the Cinema Ticket Reservation System.

## Purpose

The purpose of the test strategy is to show how the system is tested at different levels:

```text
Requirements
→ Test design
→ Unit tests
→ API tests
→ Supporting BDD tests
→ Supporting white-box test
```

The strategy combines requirement-based testing with selected structural testing and API-level verification.

## Main Focus Areas

| Area | Role in Project |
|---|---|
| Design for testability | Main focus |
| Unit test design | Main focus |
| Black-box testing | Main focus |
| API testing with Postman/Newman | Main focus |
| BDD/Reqnroll | Supporting example |
| White-box testing | Supporting example |

## Testing Levels

## 1. Requirements and Business Rules

The requirements define what the system must do.

They describe functional requirements, business rules, and non-functional requirements such as testability and deterministic time handling.

Document:

- [Requirements](requirements/requirements.md)

## 2. Black-Box Test Design

Black-box testing is used to derive selected test cases from requirements and business rules.

The project uses:

- Boundary value testing
- Equivalence partitioning

The selected areas are:

- Age restriction
- Seat availability
- Reservation time
- Cancellation deadline
- Group discount

Document:

- [Black-Box Test Design](test-design/black-box-test-design.md)

## 3. Unit Testing

Unit tests verify the core business logic directly in `Cinema.Core`.

This supports design for testability because the reservation rules can be tested without starting the API or using external infrastructure.

The unit tests cover:

- Age restriction
- Seat existence and availability
- Reservation time
- Cancellation deadline
- Group discount
- One white-box structural branch

Document:

- [Unit Testing](unit-testing/unit-testing.md)

## 4. API Testing

API testing verifies that the REST API exposes the reservation behavior correctly through HTTP endpoints.

The Postman collection checks:

- Successful reservation
- Already reserved seat
- Customer too young
- No seats requested
- Late cancellation

The collection can be run manually in Postman or automated from the terminal with Newman.

Document:

- [Postman API Testing](api-testing/postman-api-testing.md)

## 5. BDD/Reqnroll Testing

Behavior-driven development testing is used as a small supporting example of functional testing with business-readable scenarios.

The Reqnroll scenarios describe selected reservation behavior using Gherkin syntax.

Document:

- [Reqnroll BDD Testing](bdd/reqnroll-bdd-testing.md)

## 6. White-Box Testing

White-box testing is used as a small supporting example.

The selected method is:

```csharp
ReservationService.ReserveSeats(...)
```

A branch was identified through source code inspection and covered with a dedicated white-box unit test.

Document:

- [White-Box Test Design](test-design/white-box-test-design.md)

## Design for Testability

The system is structured so that the core business logic can be tested independently.

Important design choices include:

| Design Choice | Testability Benefit |
|---|---|
| Separate `Cinema.Core` project | Business rules can be tested without HTTP or infrastructure |
| `ITimeProvider` | Time-dependent logic can be controlled in tests |
| `FakeTimeProvider` | Unit tests can simulate specific times |
| `IReservationService` | API depends on an abstraction instead of the concrete service |
| `ICinemaStore` | API depends on a store abstraction instead of a concrete in-memory implementation |
| Result objects | Business failures can be asserted without relying on exceptions |

## Strategy Summary

The project uses multiple complementary testing techniques:

| Technique | What it verifies |
|---|---|
| Black-box test design | That tests are systematically derived from requirements |
| Unit testing | That core business rules work in isolation |
| API testing | That the REST API exposes the correct behavior and status codes |
| BDD/Reqnroll | That selected behavior can be described in readable business language |
| White-box testing | That source code inspection can reveal implementation branches worth testing |

