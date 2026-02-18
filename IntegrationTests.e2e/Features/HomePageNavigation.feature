Feature: Home page navigation

  Scenario: Navigate to Counter page
    Given the application is running
    When I click the Counter link
    And I click the increment button <clicks> times
    Then the counter should be <expected>

    Examples:
      | clicks | expected |
      | 1      | 1        |
      | 3      | 3        |
      | 5      | 5        |

  Scenario: Navigate to Weather page
    Given the application is running
    When I click the Weather link
    Then I should see the Weather page

  Scenario: Navigate to Claims page
    Given the application is running
    When I click the Claims link
    Then I should see the Claims page