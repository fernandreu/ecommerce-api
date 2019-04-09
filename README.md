# Manufacturing API

This is a REST API demo developed as part of the *.NET Software Engineer Technical Assignment (Manufacturing)* for albelli.  

## General overview

## Consuming the API

The API is deployed to a public AWS Lambda accessible at the following location:

https://jzjexhjx9i.execute-api.eu-west-1.amazonaws.com/Prod/

Navigation through the API should mostly be self-explanatory, as every page directly provides you with the links you need to
use to navigate to child elements. Nevertheless, all the API endpoints are summarised below.

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
            "contactName": "Fernando Andreu",
            "address": "Glasgow"
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
    - The `orderId` and / or `requiredMinWidth` properties will be ignored when specified as part of the request body

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

### Unit / integration tests

The solution includes several unit and integration tests under the `ManufacturingAPI.UnitTests` and 
`ManufacturingAPI.IntegrationTests` projects respectively. These should be considered as a proof of concept, as 
they do not provide full code coverage of the entire web app.

The unit tests focus a key concept of the technical assessment: ensuring the minimum width required for
an order is calculated correctly.

The integration tests have access to both ASP.NET and AWS dependencies. Due to this, the example tests included 
have focused on ensuring that the different API endpoints can be called correctly and give the expected results.

The testing framework of choice was `xUnit` for both unit and integration tests.

## CI / CD

This project is integrated with Azure Pipelines:

[![Build Status](https://dev.azure.com/fernandreu-public/ManufacturingAPI/_apis/build/status/ManufacturingAPI-CI?branchName=master)](https://dev.azure.com/fernandreu-public/ManufacturingAPI/_build/latest?definitionId=4&branchName=master)

On push, the app is built and tested. If these steps are successful, the lambda will be deployed to AWS automatically.

**Due to time constraints, only unit tests (and not integration tests) are automated through the pipeline.** For the
same reason, no automated testing of the web app in a production-like environment takes place before the final release
into production.
