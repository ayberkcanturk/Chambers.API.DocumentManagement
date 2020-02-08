Feature: ListFiles
In order to give my customers
I want to give them an API endpoint to let them list their uploaded files.

@v1 @listfiles @ignore
Scenario: List files
Given I call the new document service API
When I call the API to get a list of documents
Then a list of PDFs’ is returned with the following properties: name, location, file-size


@v1 @listfiles @ignore
Scenario: Reorder files
Given I have a list of PDFs’
When I choose to re-order the list of PDFs’
Then the list of PDFs’ is returned in the new order for subsequent calls to the API