# User API Dev Guide
1. Update Connection string in appsetttings.json file
2. Run "Update-Database" command in package manager console

## Building
1. dotnet restore
2. dotnet build.
3. dotnet run

## Testing
1. Integration Testing - Swagger
2. Unit Testing - XUnit

## Deploying 
Commands
1. "docker-compose up" - Sql Server to publish database
2. run docker file - The publish Web API into docker

## Additional Information
1. CORS origin
2. Logging Microsoft
3. Swagger
4. Custom Middleware
5. JwtToken
6. Dependency Injection
7. AutoMapper
8. Versioning

Set API project as startup prject and run application for debugging.

API Request and Response format informtion is available in below swagger URL. 
1. Local - http://localhost:5059/swagger
2. Image - ![image](https://user-images.githubusercontent.com/21257352/204250312-e1e5a45b-8e31-4d75-ac35-a61c7663abc9.png)
 
