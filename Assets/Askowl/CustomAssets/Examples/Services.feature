Feature: CustomAsset Services

  Scenario: Successful Top-Down Service Call
    Given a mock state of "Success"
    And an add service on the math server
    When we have a request with values 21 and 22
    And we call the add service
    Then we will get a result of 43