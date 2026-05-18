# Black-Box Test Design

## Purpose

This document describes the black-box test design for the Cinema Ticket Reservation System.

The test cases are derived from the requirements and business rules, not from the implementation. The purpose is to demonstrate selected black-box testing techniques as part of a wider software quality strategy.

## Scope

This document focuses on five selected areas:

| Area | Related Requirements | Technique |
|---|---|---|
| Age restriction | FR3, BR1 | Boundary value testing |
| Seat availability | FR2, BR2, BR3 | Equivalence partitioning |
| Reservation time | FR4, BR5 | Boundary value testing |
| Cancellation deadline | FR8, BR7 | Boundary value testing |
| Group discount | FR6, BR6 | Boundary value testing |

---

## 1. Age Restriction

### Related requirement

- **FR3:** The system must reject a reservation if the customer's age is below the movie's minimum age rating.
- **BR1:** Customer age must be greater than or equal to the movie age rating.

### Technique

Boundary value testing is used because age is an ordered numeric value. The important boundary is the movie's minimum age rating.

Assume the movie has an age rating of `15`.

| Test Case ID | Customer Age | Movie Age Rating | Expected Result |
|---|---:|---:|---|
| BB-AGE-01 | 14 | 15 | Rejected |
| BB-AGE-02 | 15 | 15 | Accepted |
| BB-AGE-03 | 16 | 15 | Accepted |

---

## 2. Seat Availability

### Related requirement

- **FR2:** The system must reject a reservation if one or more requested seats do not exist or are already reserved.
- **BR2:** All requested seats must exist in the screening room.
- **BR3:** All requested seats must be available for the selected screening.

### Technique

Equivalence partitioning is used because requested seats can be divided into classes that should produce different system behavior.

| Test Case ID | Requested Seat | Seat State | Expected Result |
|---|---|---|---|
| BB-SEAT-01 | A1 | Exists and available | Accepted |
| BB-SEAT-02 | A2 | Exists but already reserved | Rejected |
| BB-SEAT-03 | Z99 | Does not exist | Rejected |

---

## 3. Reservation Time

### Related requirement

- **FR4:** The system must reject reservations made after the screening has started.
- **BR5:** A reservation can only be created before the screening starts.

### Technique

Boundary value testing is used because reservation time is an ordered time value. The important boundary is the screening start time.

Assume the screening starts at `20:00`.

| Test Case ID | Reservation Time | Screening Start | Expected Result |
|---|---|---|---|
| BB-TIME-01 | 19:59 | 20:00 | Accepted |
| BB-TIME-02 | 20:00 | 20:00 | Rejected |
| BB-TIME-03 | 20:01 | 20:00 | Rejected |

---

## 4. Cancellation Deadline

### Related requirement

- **FR8:** The system must reject cancellation if the screening starts in less than 2 hours.
- **BR7:** A reservation can only be cancelled at least 2 hours before the screening starts.

### Technique

Boundary value testing is used because the cancellation rule depends on an ordered time boundary.

Assume the screening starts at `20:00`.

| Test Case ID | Cancellation Time | Time Before Screening | Expected Result |
|---|---|---:|---|
| BB-CANCEL-01 | 17:59 | 2h 1m | Accepted |
| BB-CANCEL-02 | 18:00 | 2h 0m | Accepted |
| BB-CANCEL-03 | 18:01 | 1h 59m | Rejected |

---

## 5. Group Discount

### Related requirement

- **FR6:** The system must apply a 10% group discount when the reservation contains at least 5 tickets.
- **BR6:** A reservation with 5 or more tickets receives a 10% discount.

### Technique

Boundary value testing is used because the discount rule has a clear numeric boundary at 5 tickets.

| Test Case ID | Ticket Count | Expected Discount | Expected Result |
|---|---:|---:|---|
| BB-DISCOUNT-01 | 4 | 0% | No group discount |
| BB-DISCOUNT-02 | 5 | 10% | Group discount applied |
| BB-DISCOUNT-03 | 6 | 10% | Group discount applied |

---

## Summary

This black-box test design demonstrates how selected requirements can be transformed into systematic test cases using equivalence partitioning and boundary value testing.

