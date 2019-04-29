# Manufacturing API

This is a Proof of Concept of a typical e-commerce REST API, which is deployed to a public AWS Lambda accessible at 
the following location:

https://9k4lggrxmk.execute-api.eu-west-1.amazonaws.com/Prod/

The REST API is built using ASP.NET Core 2.1 (i.e. the latest version supported by AWS Lambda). Internally, it uses a 
DynamoDB database to store all data, and a CloudFormation template to create / update all the infrastructure.

## Consuming the API

Navigation through the API should mostly be self-explanatory, as every page directly provides you with the links you need to
use to navigate to child elements. Nevertheless, all the API endpoints are summarised in this section.

For clarity, the navigation links have been removed from all the response body examples.

### See the full list of customers

- **Endpoint:** `GET /Customers`
- **Observations:**
    - For the purposes of this demo, anyone can use this endpoint and see all properties of each customer
    - On a real production API, there should be restrictions in place to access sensitive data (e.g. by requiring 
      administrator privileges for certain properties or for the endpoint itself)

Response body example:

```
{
    "count": 2,
    "value": [
        {
            "customerId": "TEST",
            "contactName": "Michael Parker",
            "address": "New York"
        },
        {
            "customerId": "1",
            "contactName": "John Doe",
            "address": "Amsterdam"
        }
    ]
}
```

### Get data from a specific customer

- **Endpoint:** `GET /Customers/{customerId}`
- **Observations:**
    - For the purposes of this demo, there are only two customers available with IDs `TEST` and `1` respectively
    - Customers cannot be created, updated or deleted
    
Response body example:

```
{
    "customerId": "123",
    "contactName": "John Doe",
    "address": "Amsterdam"
}
```

### Get all orders from a specific customer

- **Endpoint:** `GET /Customers/{customerId}/Orders`
- **Response body:** A collection of JSON objects representing each order

Response body example:

```
{
    "count": 3,
    "value": [
        {
            "orderId": "1",
            "date": "2019/04/07",
            "products": [
                {
                    "productType": "canvas",
                    "quantity": 1
                },
                {
                    "productType": "cards",
                    "quantity": 1
                },
            ],
            "requiredMinWidth": 13.7
        },
        {
            "orderId": "2",
            "date": "2019/04/08",
            "products": [
                {
                    "productType": "mug",
                    "quantity": 4
                }
            ],
            "requiredMinWidth": 97
        },
        {
            "orderId": "3",
            "date": "2019/04/09",
            "products": [
                {
                    "productType": "calendar",
                    "quantity": 2
                }
            ],
            "requiredMinWidth": 20
        }
    ]
}
```

### Get data from a specific order

- **Endpoint:** `GET /Customers/{customerId}/Orders/{orderId}`
- **Response body:** A JSON object representing the order

Response body example:

```
{
    "orderId": "123",
    "date": "2019/04/09",
    "products": [
        {
            "productType": "mug",
            "quantity": 4
        }
    ],
    "requiredMinWidth": 97
}
```

### Create order for a given customer with an explicit order ID

- **Endpoint:** `PUT /Customers/{customerId}/Orders/{orderId}`
- **Headers:**
    - `Content-Type: application/json`
- **Request body:** A JSON object representing the order, including each product type and quantity
- **Response body:** A JSON object representing the order as it was saved in the database
- **Observations:**
    - There are no restrictions in terms of the order IDs that can be used
    - If an order with that ID already exists, **all** its contents will be replaced
    - The request body must contain at least one product
    - The `orderId` and / or `requiredMinWidth` properties will be ignored when specified as part of the request body
    - For the purposes of this demo, there are no error checks performed for the `date` field

Request body example:

```
{
    "date": "2019/04/09",
    "products": [
        {
            "productType": "mug",
            "quantity": 1
        }
    ]
}
```

Response body example:

```
{
    "orderId": "123",
    "date": "2019/04/09",
    "products": [
        {
            "productType": "mug",
            "quantity": 4
        }
    ],
    "requiredMinWidth": 97
}
```

## Testing the API locally

To test the API in your local machine, you will need to have a DynamoDB instance running first with the sharedDb
option set to true. This can be done easily with Docker; for the first time, you can use the following command:

```
docker run -d --name dynamodb -p 8000:8000 amazon/dynamodb-local -jar DynamoDBLocal.jar -inMemory -sharedDb
```

The same local DynamoDB instance can be started again later by running the corresponding Docker command:

```
docker start dynamodb
```

After this, open the project on Visual Studio 2017 and run the `IIS Express` profile. This should automatically
start IIS and open a new browser pointing to the local API url in http://localhost:55435/ .


## Unit / integration tests

The solution includes several unit, integration and functional tests under the corresponding `UnitTests`, 
`IntegrationTests` and `FunctionalTests` projects. These should be considered as a proof of concept, as 
they do not provide full code coverage of the entire web app.

The unit tests focus a key concept of the technical assessment: ensuring the minimum width required for
an order is calculated correctly.

The integration tests have access to both ASP.NET and AWS dependencies. Due to this, the example tests included 
have focused on ensuring that the different API endpoints can be called correctly and give the expected results.

The testing framework of choice was `xUnit` for both unit and integration tests.


## CI / CD

This project is integrated with Azure Pipelines:

[![Build Status](https://dev.azure.com/fernandreu-public/ECommerceAPI/_apis/build/status/fernandreu.ecommerce-api?branchName=master)](https://dev.azure.com/fernandreu-public/ECommerceAPI/_build/latest?definitionId=5&branchName=master)

On push, the app is built and tested. If these steps are successful, the lambda will be deployed to AWS automatically.

**Due to time constraints, only unit tests (and not integration tests) are automated through the pipeline.** For the
same reason, no automated testing of the web app in a production-like environment takes place before the final release
into production.


## DynamoDB architecture

Following [Amazon's best practises for DynamoDB](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/bp-general-nosql-design.html),
only one table has been used for this demo. The list of access patterns needed is significantly small, and hence the 
design of this single table can also be very simple. However, in anticipation of additional access patterns which 
might be implemented in future iterations of this demo, the following design has been adopted:

- Three main columns are used for all table entries: `PK` (partition key), `SK` (sort key) and `Data`
- `PK` and `SK` conform the compount primary key of the table
- `SK` and `Data` conform the compount primary key (partition and sort respectively) of a Global Secondary Index (GSI)
- Each customer constitutes an individual table entry:
    - The `PK` column holds the customer's ID preceded by a `CUSTOMER-` prefix (e.g. `CUSTOMER-1`)
    - The `SK` column holds the customer's contact name
    - The `Data` column holds the customer's address
- Each order constitutes an individual table entry:
    - The `PK` column holds the order's ID preceded by an `ORDER-` prefix (e.g. `ORDER-1`)
    - The `SK` column holds the ID of the customer who placed the order (also with the `CUSTOMER-` prefix)
    - The `Data` column holds the order's date
- Given that there is no need to query any property of the order's products, these have been stored as part of the
  orders and not as individual entries

When querying / manipulating entries in the table, the demo uses the Object Persistence Model from the AWS SDK. As part
of this, the format described above for customers and orders can be seen in the attributes of the `CustomerEntity` and
`OrderEntity` classes respectively.


## Further work

The following list just shows a few examples of possible additions to the demo but are considered out of scope at this
stage:

- Add sort / search options to the `GetAllCustomers` / `GetAllOrders` routes
- Enabe authentication / authorization
- Configure Api versioning
- Allow creating orders with `POST` actions without specifying their ID
- Allow partial updates of customer / order properties (e.g. changing order status from dispatched to delivered)
- Allow creating customers
- Send confirmation email to customers upon account creation / email address update (for example using [AWS SES](https://aws.amazon.com/ses/))
