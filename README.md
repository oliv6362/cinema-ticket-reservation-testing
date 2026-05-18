# Cinema Ticket Reservation Testing

**Software Quality** exam project for a Cinema Ticket Reservation System.

The project demonstrates a wide testing strategy using requirements, black-box test design, unit testing, design for testability, API testing with Postman/Newman, BDD/Reqnroll, and selected white-box testing.

## Architecture

```text
cinema-ticket-reservation-testing/
├── docs/
├── postman/
├── src/
│   ├── Cinema.Api/
│   ├── Cinema.Core/
│   └── Cinema.Infrastructure/
└── tests/
    ├── Cinema.UnitTests/
    └── Cinema.BddTests/
```

| Path                         | Responsibility                                                                                                              |
| ---------------------------- | --------------------------------------------------------------------------------------------------------------------------- |
| `docs/`                      | Project documentation, including requirements, test strategy, test design, unit testing, API testing, and BDD documentation |
| `postman/`                   | Postman collection used for automated API testing with Newman                                                               |
| `src/Cinema.Api/`            | REST API controllers, DTOs, HTTP response mapping, and application startup configuration                                    |
| `src/Cinema.Core/`           | Core business logic, entities, interfaces, services, results, and business rules                                            |
| `src/Cinema.Infrastructure/` | Infrastructure implementations such as the in-memory cinema store and system time provider                                  |
| `tests/Cinema.UnitTests/`    | Unit tests for core services such as `ReservationService` and `PricingService`                                              |
| `tests/Cinema.BddTests/`     | Reqnroll BDD tests using Gherkin scenarios and step definitions                                                             |

## Documentation

- [Test Strategy](docs/test-strategy.md)
- [Requirements](docs/requirements/requirements.md)
- [Black-Box Test Design](docs/test-design/black-box-test-design.md)
- [White-Box Test Design](docs/test-design/white-box-test-design.md)
- [Unit Testing](docs/unit-testing/unit-testing.md)
- [Postman API Testing](docs/api-testing/postman-api-testing.md)
- [Reqnroll BDD Testing](docs/bdd/reqnroll-bdd-testing.md)

## Build

Restore and build:

```bash
dotnet restore
dotnet build
```

## Running the Project

```bash
dotnet run --project src/Cinema.Api
```

Open Swagger:

```text
http://localhost:<port>/swagger
```

## Running the Unit and BDD Tests

```bash
dotnet test
```

## Running API Tests with Newman

Start the API first, then run:

```bash
npx newman run postman/CinemaReservationSystem.postman_collection.json
```

