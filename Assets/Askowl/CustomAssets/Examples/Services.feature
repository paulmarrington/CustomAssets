Feature: CustomAsset Services

  @TopDownSuccess
  Scenario: Successful Top-Down Service Call
    Given a top-down stack with 2 services
    And server success of "Pass,Fail"
    And an add service on the math server
    When we add 21 and 22
    Then we will get a result of 43

  @TopDownFailure
  Scenario: Top-Down Service Failure
    Given a top-down stack with 2 services
    And server success of "Fail,Fail"
    And an add service on the math server
    When we add 21 and 22
    Then we get a service error
    And a service message of "Service 2 Failed"

  @TopDownFallback
  Scenario: Top-Down Service Fallback Success
    Given a top-down stack with 2 services
    And server success of "Fail,Pass"
    And an add service on the math server
    When we add 21 and 22
    Then we will get a result of 43
