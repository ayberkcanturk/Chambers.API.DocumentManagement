Feature: RemoveFile
In order to give my customers
I want to give them an API endpoint to let them remove their uploaded pdf files.

@v1 @removefile @ignore
Scenario: Upload files
Given I have selected a PDF from the list API that I no longer require
When I request to delete the PDF
Then the PDF is deleted and will no longer return from the list API and can no longer be downloaded from its location directly