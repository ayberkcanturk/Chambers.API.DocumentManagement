Feature: DownloadFile
In order to give my customers
I want to give them an API endpoint to let them download their uploaded files.


@v1 @downloadfile @ignore
Scenario: Download file
Given I have chosen a PDF from the list API
When I request the location for one of the PDF's
Then The PDF is downloaded