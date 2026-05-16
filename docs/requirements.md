# Cinema Ticket Reservation System - Requirements

## Functional Requirements

### FR1: Reserve seats for a screening
A customer must be able to reserve one or more available seats for a specific screening.

### FR2: Reject unavailable seats
The system must reject a reservation if one or more requested seats do not exist or are already reserved.

### FR3: Validate age restrictions
The system must reject a reservation if the customer's age is below the movie's minimum age rating.

### FR4: Validate reservation time
The system must reject reservations made after the screening has started.

### FR5: Calculate ticket price
The system must calculate the total ticket price based on the number of tickets and applicable discounts.

### FR6: Apply group discount
The system must apply a 10% group discount when the reservation contains at least 5 tickets.

### FR7: Cancel reservation
A customer must be able to cancel a reservation before the cancellation deadline.

### FR8: Reject late cancellation
The system must reject cancellation if the screening starts in less than 2 hours.

---

## Business Rules

### BR1: Age restriction
Customer age must be greater than or equal to the movie age rating.

### BR2: Seat existence
All requested seats must exist in the screening room.

### BR3: Seat availability
All requested seats must be available for the selected screening.

### BR4: Minimum seat count
A reservation must contain at least one seat.

### BR5: Reservation time
A reservation can only be created before the screening starts.

### BR6: Group discount
A reservation with 5 or more tickets receives a 10% discount.

### BR7: Cancellation deadline
A reservation can only be cancelled at least 2 hours before the screening starts.

---

## Non-functional Requirements

### NFR1: Testability
Business logic must be implemented in services that can be tested independently from the API layer.

### NFR2: Deterministic time handling
Time-dependent logic must use an injectable time provider instead of directly using `DateTime.Now`.

### NFR3: API validation
Invalid API requests must return meaningful error responses.