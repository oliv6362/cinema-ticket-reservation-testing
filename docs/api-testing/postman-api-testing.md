# Postman API Testing

This document describes the automated API tests for the Cinema Ticket Reservation System.

The purpose of these tests is to verify that the REST API exposes the reservation business rules correctly through HTTP endpoints. The unit tests verify the core business logic directly, while the Postman tests verify the API behavior from the outside as an API client.

## Test Scope

The Postman collection focuses on selected reservation scenarios from the requirements and black-box test design.

Covered API scenarios:

| Scenario | Expected Result |
|---|---|
| Reset test data | Test data is reset before the run |
| Create reservation successfully | Reservation is created |
| Reserve an already taken seat | Request is rejected with conflict |
| Customer too young | Request is rejected as invalid |
| No seats requested | Request is rejected as invalid |
| Cancel reservation too late | Request is rejected with conflict |

The collection is intentionally limited in scope because the project uses a wide software quality approach. API testing is one part of the overall strategy, together with unit testing, black-box test design, design for testability, BDD, and selected white-box testing.

## Collection Structure

The Postman collection is named:

```text
CinemaReservationSystem
```

The collection is organized into these folders:

```text
1. Setup
2. Future Reservations
3. Late Reservations
```

## Collection Variables

The collection uses these variables:

| Variable | Purpose |
|---|---|
| `baseUrl` | Base URL for the running ASP.NET Core API |
| `futureScreeningId` | ID of the seeded future screening |
| `lateCancellationScreeningId` | ID of the seeded late-cancellation screening |
| `reservationId` | Reservation ID saved during the collection run |

Example values:

```text
baseUrl = http://localhost:5180
futureScreeningId = 11111111-1111-1111-1111-111111111111
lateCancellationScreeningId = 33333333-3333-3333-3333-333333333333
```

The `reservationId` variable is set dynamically during the test run by the successful reservation request.

Example Postman test script:

```javascript
const body = pm.response.json();
pm.collectionVariables.set("reservationId", body.reservationId);
```

## Running the API Tests with Newman

The collection can be executed from the terminal using Newman.

First, start the ASP.NET Core API:

```bash
dotnet run --project src/Cinema.Api
```

Then run the Postman collection:

```bash
npx newman run postman/CinemaReservationSystem.postman_collection.json
```

The API must be running before Newman is executed.

## Expected Result

A successful run should show that all requests and assertions pass.

Example expected result:

```text
requests:   7 executed, 0 failed
assertions: 15 executed, 0 failed
```

## Notes

The `/api/test-data/reset` endpoint exists only to support automated API testing in this exam project. It is not intended as production functionality.