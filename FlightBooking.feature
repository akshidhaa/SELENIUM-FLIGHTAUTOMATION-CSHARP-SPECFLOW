Feature: Flight Booking

Background:
    Given user opens flight booking site

Scenario Outline: Flight booking flow

    When user selects "<from>" as departure city
    And user selects "<to>" as destination city
    And user selects departure date "<date>"
    And user clicks search button
    Then system should process booking flow

Examples:

| from      | to        | date       |
|-----------|-----------|------------|
| Chennai   | Goa       | 25-06-2026 |
| Delhi     | Chennai   | 28-06-2026 |
| Bangalore | Hyderabad | 25-06-2026 |
| Mumbai    | Punjab    | 26-06-2026 |
