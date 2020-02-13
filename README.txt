Create an application ASP .NET and Azure infrastructure to get this done.
   
{
    "Key": <string>,
    "Email":<string:emailaddress>
    "Attributes": ["string", "string"]
}

1. Create an endpoint to process the above Json
-----> ready
2. The endpoint will be hit a millions times per day, it has to return just a success
	-> API: PostToMessageQueue
3. The data from the API has to be processed  3 different ways
4. One has to feed it to a queue which triggers an app to write to a SQL server table.
-----> I'm sorry, but I couldn't implement it as requested. I save to database directly (I don't know the azure function) -> API name: SaveTestWork
    4a. When storing attributes make sure we dont store duplicate attributes per email
-----> ready
5. One has to write it as a log in a blob file after hashing the email address which creates a new file every day.
-----> ready
6. One has to be written to a storage table based on a date based Key for easy fetching
7. Once all the processing is completed and the email gets 10 attributes attached to it. Send an email out using sendgrid api in azure to congratulate the email user and put the attributes in the email as  the body.
-----> ready
8. Create and endpoint to query the storage table to return that days requests as a csv
-----> ready API: GetTestCsv

Create a Postman collection to showcase these functionalities. Make sure the test is created in a separate resource group for easy deletion later on. 
-----> ready

Have all necessary code available in github for code review after the test has been finished.
