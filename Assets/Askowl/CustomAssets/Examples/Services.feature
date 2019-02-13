Feature: CustomAsset Services

  Scenario: Successful Top-Down Service Call
    Given a mock state of "Success"
    And an add service on the math server
    When we add 21 and 22
    Then we will get a result of 43