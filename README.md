# StoreAPI

StoreAPI is a web API that lets you perform CRUD operations over users from the store. Additionally, it also lets you record any purchases that any user performs.

# Before you run (applies for both API and integration tests)

Make sure that the MongoDbUri property is correctly set to a MongoDb connection string.
```
mongodb+srv://{user}:{password}@{host}
```
This property is set in `StoreAPI/appsettings.json` and `StoreAPI.Data.Test/appsettings.json`.
The application automatically sets up the database and the collection.

# Endpoints
| Endpoint Type | Endpoint Path | Description | Body |
| ------------- | ------------- | ----------- | ---- |
| POST | /Purchases | Creates a purchase record and updates the related user | CreatePurchaseRecordPayload |
| GET | /Users/`{nationalId}` | Retrieves an existing user based on its nationalId | - |
| PUT | /Users/`{nationalId}` | Updates an existing user based on its nationalId | UpdateUserPayload |
| DELETE | /Users/`{nationalId}` | Deletes an existing user based on its nationalId | - |
| GET | /Users | Retrieves a list of all the existing users | - |
| POST | /Users | Creates a new user | CreateUserPayload |
| GET | /Users/MoreThanAvgSpent | Retrieves a list of all the existing users that have spent more than what has been spent on average | - |

# Architecture

The application is divided in the following projects:
* **StoreAPI**: The API itself that contains the controllers from the Web API.
* **StoreAPI.Core**: The core contains the definitions of the model classes (along with related DTOs), interfaces that provide functionalities, and payloads required to perform different database actions.
* **StoreAPI.Data**: The data project contains the implementation of the repository (along with MongoDB driver related code), and a database configuration assistant that helps us set up the database along with its collections.
* **StoreAPI.Data.Test**: The integration tests associated with the database operations.

# Additional information
## Services
Since we need to retrieve data from the MongoDB database, we have implemented a repository class that is responsible for retrieving all the stuff from the database. Given that most of the operations we perform, such as filtering or grouping can be done also from the source of information, we decided not to create a service layer, thus having all the necessary business logic attached to our repository layer.
This led us to not have any unit tests, as we do not really have any business logic to test on any service, and thus, no need for any other libraries such as Moq.
Now, as a trade off, we have implemented integration tests using mocked data in order to ensure that all the functionalities provided by the repository layer work as expected.

## Database
Our database will consist mainly two collections: users and purchaseRecords. Documents available on purchaseRecords will contain a userNationalId which lets you know who purchased the record. Additionally, we have decided to denormalize the money spent by a user in order to be able to have a transaction that modifies more than one document (thus, user will have a moneySpent field) .

## Regarding MongoDB advanced features
I have tried to apply all the features that were suggested on the task.

* **Aggregation**: The method that retrieves users whose total money spent is higher than average use $group and $project in an aggregation pipeline (the code was created using MongoDB Compass).
* **Transactions**: Whenever a purchase is performed, one document needs to be created (the purchase record) and another one needs to be modified (user). To do this, we create a transaction and commit it if no exceptions are thrown.
* **Indexes**: We have used a unique index on the nationalId property of the user. We are also throwing 404 and 409 errors based on this index.
 
## Feedback
Any feedback in order to improve the solution that helps me gain further knowledge (in particular to MongoDB) is greatly appreciated.

## Last words
This was my first project where I worked with MongoDB as a developer. I did previously work with Azure CosmosDB but the SDK provided is way different than the MongoDB Driver. No matter the outcome for this task, it was a really interesting project that made me learn a **lot** regarding MongoDB.

