Feature: PlanAJourneyTestFeature In order to test Plan A Journey functionality on TfL
As a SDET
I want to ensure functionality is working for planning a journey
Scenario: 1 Verify that a valid journey can be planned using the widget
Given I have navigated to TfL website
And I plan my journey from Luton Rail Station
And to London Bridge, London Bridge Station
When I plan my journey
Then I should see my journey plan in the Journey results page

Scenario: 2 Verify that the widget is unable to provide results when an invalid journey is planned
Given I have navigated to TfL website
And I plan my journey from Fake Location
And to Fake Location
When I plan my journey
Then I should see my journey is not planned in the Journey results page

Scenario: 3 Verify that the widget is unable to plan a journey if no locations are entered into the widget
Given I have navigated to TfL website
And I plan my journey from 
And to 
When I plan my journey
Then I should see location validation errors on the form

Scenario: 4 Verify change time link on the journey planner displays Arriving option and plan a journey based on arrival time
Given I have navigated to TfL website
And I change the time
Then I should see change time link diplays Arriving option
Given I select an arriving time
And I plan my journey from Luton Rail Station
And to London Bridge, London Bridge Station
When I plan my journey
Then I should see my journey plan in the Journey results page

Scenario: 5 On the Journey results page verify that a journey can be amended by using the Edit Journey button
Given I have navigated to TfL website
And I plan my journey from Luton Rail Station
And to London Bridge, London Bridge Station
When I plan my journey
Then I should see my journey plan in the Journey results page
Given I edit my journey
When I plan my journey
Then I should see my journey plan in the Journey results page

Scenario: 6 Verify that the Recents tab on the widget displays a list of recently planned journeys
Given I have navigated to TfL website
And I plan my journey from Luton Rail Station
And to London Bridge, London Bridge Station
When I plan my journey
Then I should see my journey plan in the Journey results page
Given I go to my recent journey plans
Then I should see my recent journey from Luton Rail Station to London Bridge, London Bridge Station