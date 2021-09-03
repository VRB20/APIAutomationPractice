Template for REST API Services Testing. 

This framework is implemented using C#, NUnit, httpclient and restsharp.

Input attributes are read from CSV File and is used to construct the JSON Body. Rest call will be made using either httpclient or restsharp.

Response is deserialized and compared with the expected results. If every thing is as per the expected results columns from the input file, the test will pass. If not, assertion will fail.

One of the most important feature of this framework is - The tests will be created based on the data from the input file. For example, if the input file has 10 tests, then ten tests will be created when you build the solution and shown in the test explorer window. If the input file has 5 tests, then only 5 tests will be displayed in the Test Explorer window.

The tests can be run in parallel. Have run 10,000 tests in less than 20 minutes against a few sample API testing sites.

This framework reduces the efforts required to test an API to a bare minimum.

1. The tester just needs to create the input file with the data required to construct the json body and then specify the list of attributes and their expected values. 
2. Place the input file in the Test Data folder.
3. Build the solution and can see the tests populated in the Test Explorer Window
4. Run all the tests. Sit back, rleax and enjoy while they are running :-)
5. If there are any issues, tester will be notified by the failure with sufficient information to analyse.

UNLESS THERE IS A CHANGE IN THE REQUEST PARAMETERS/ATTRIBUTES OR RESPONSE ATTRIBUTES, THERE IS NO UPDATE NEEDED FOR THE FRAMEWORK - MEANING - ZERO MAINTENANCE EFFORT.
TESTING CAN BE PERFORMED WITH JUST UPDATING THE INPUT CSV FILE TO SUIT THE BUSINESS REQUIREMENTS.







