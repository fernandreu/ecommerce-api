## Testing the API locally

To test the API in your local machine, you will need to have a DynamoDB instance running first with the sharedDb
option set to true. This can be done easily with Docker; for the first time, you can use **either** of the 
following commands:

```
# Official Amazon image:
docker run -d --name dynamodb -p 8000:8000 amazon/dynamodb-local -jar DynamoDBLocal.jar -inMemory -sharedDb

# Image 
docker run -d --name dynamodb -p 8000:8000 dwmkerr/dynamodb -inMemory -sharedDb
```

To stop, restart or start again the local DynamoDB instance, you can use the corresponding command:

```
docker [stop|restart|start] dynamodb
```
