Feature: Cinema ticket reservation

The cinema reservation system must allow customers to reserve available seats
while enforcing important business rules.

    Scenario: Customer successfully reserves an available seat
        Given a screening exists with a movie age rating of 15
        And seat A1 is available
        And the current time is before the screening starts
        When a customer aged 18 reserves seat A1
        Then the reservation should be accepted

    Scenario: Customer is too young for the movie
        Given a screening exists with a movie age rating of 15
        And seat A1 is available
        And the current time is before the screening starts
        When a customer aged 14 reserves seat A1
        Then the reservation should be rejected with error code CUSTOMER_TOO_YOUNG

    Scenario: Seat is already reserved
        Given a screening exists with a movie age rating of 15
        And seat A1 is already reserved
        And the current time is before the screening starts
        When a customer aged 18 reserves seat A1
        Then the reservation should be rejected with error code SEAT_ALREADY_RESERVED