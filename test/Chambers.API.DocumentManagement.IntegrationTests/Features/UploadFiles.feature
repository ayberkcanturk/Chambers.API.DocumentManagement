Feature: UploadFiles
In order to give my customers
I want to give them an API endpoint to let them upload their pdf files.

@v1 @uploadfiles
Scenario: Upload files pdf
Given I have a PDF to upload
When I send the PDF to the API https://localhost:5001/api/document/upload
Then it is uploaded successfully

@v1 @uploadfiles
Scenario: Upload files non-pdf
Given I have a non-pdf to upload
When I send the non-pdf to the API https://localhost:5001/api/document/upload
Then the API does not accept the file and returns the appropriate messaging and status

@v1 @uploadfiles @ignore
Scenario: Upload files over 5MB
Given I have a max pdf size of 5MB
When I send the pdf to the API https://localhost:5001/api/document/upload
Then the API does not accept the file and returns the appropriate messaging and status