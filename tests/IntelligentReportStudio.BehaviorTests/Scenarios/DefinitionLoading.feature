Feature: Definition loading
  Scenario: A template without a schema version is accepted
    Given a report with root and report versions
    When the definition is loaded
    Then the definition can be copied independently
