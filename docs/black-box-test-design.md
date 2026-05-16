# Black-Box Test Design

## Purpose

This document describes how selected black-box testing techniques are used to derive test cases from the requirements of the Cinema Ticket Reservation System.

The purpose is not to test every possible input combination, but to demonstrate systematic test design using selected requirements.

## Role in the Project

Black-box testing is one of the main focus areas of the project. The test cases are derived from requirements and business rules, not from the implementation.

The selected techniques are:

| Technique | Used For |
|---|---|
| Equivalence partitioning | Seat availability |
| Boundary value testing | Age restrictions, cancellation deadline, group discount |

---

# 1. Age Restriction

## Related requirements

| ID | Description |
|---|---|
| FR3 | The system must reject a reservation if the customer's age is below the movie's minimum age rating. |
| BR1 | Customer age must be greater than or equal to the movie age rating. |

## Technique

Boundary value testing.

The movie age rating is an ordered value. The important boundary is the minimum allowed age.

Assume the movie age rating is `15`.

| Test Case ID | Customer Age | Expected Result |
|---|---:|---|
| BB-AGE-01 | 14 | Rejected |
| BB-AGE-02 | 15 | Accepted |
| BB-AGE-03 | 16 | Accepted |

---

# 2. Seat Availability

## Related requirements

| ID | Description |
|---|---|
| FR1 | A customer must be able to reserve one or more available seats for a specific screening. |
| FR2 | The system must reject a reservation if one or more requested seats do not exist or are already reserved. |
| BR2 | All requested seats must exist in the screening room. |
| BR3 | All requested seats must be available for the selected screening. |

## Technique

Equivalence partitioning.

Requested seats can be divided into classes that should be handled differently by the system.

| Partition | Example | Expected Result |
|---|---|---|
| Existing and available seat | A1 | Accepted |
| Existing but already reserved seat | A2 | Rejected |
| Non-existing seat | Z99 | Rejected |

## Test cases

| Test Case ID | Requested Seat | Seat State | Expected Result |
|---|---|---|---|
| BB-SEAT-01 | A1 | Exists and available | Accepted |
| BB-SEAT-02 | A2 | Exists but already reserved | Rejected |
| BB-SEAT-03 | Z99 | Does not exist | Rejected |

---

# 3. Cancellation Deadline

## Related requirements

| ID | Description |
|---|---|
| FR7 | A customer must be able to cancel a reservation before the cancellation deadline. |
| FR8 | The system must reject cancellation if the screening starts in less than 2 hours. |
| BR8 | A reservation can only be cancelled at least 2 hours before the screening starts. |

## Technique

Boundary value testing.

The cancellation deadline is an ordered time boundary. The important boundary is exactly 2 hours before the screening starts.

Assume the screening starts at `20:00`.

| Test Case ID | Cancellation Time | Time Before Screening | Expected Result |
|---|---|---:|---|
| BB-CANCEL-01 | 17:59 | 2h 1m | Accepted |
| BB-CANCEL-02 | 18:00 | 2h 0m | Accepted |
| BB-CANCEL-03 | 18:01 | 1h 59m | Rejected |

---

# 4. Group Discount

## Related requirements

| ID | Description |
|---|---|
| FR6 | The system must apply a 10% group discount when the reservation contains at least 5 tickets. |
| BR6 | A reservation with 5 or more tickets receives a 10% discount. |

## Technique

Boundary value testing.

The discount boundary is 5 tickets.

| Test Case ID | Ticket Count | Expected Discount |
|---|---:|---:|
| BB-DISCOUNT-01 | 4 | 0% |
| BB-DISCOUNT-02 | 5 | 10% |
| BB-DISCOUNT-03 | 6 | 10% |

---

# Summary

This black-box design intentionally focuses on selected important business rules instead of exhaustive test coverage. This supports the wide exam approach, where black-box testing is one part of a larger test strategy that also includes unit testing, design for testability, API testing, BDD, and white-box testing.