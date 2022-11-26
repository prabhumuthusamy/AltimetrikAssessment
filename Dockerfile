FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY TestProject.Data/*.csproj ./TestProject.Data/
COPY TestProject.DTO/*.csproj ./TestProject.DTO/
COPY TestProject.Service/*.csproj ./TestProject.Service/
COPY TestProject.WebAPI/*.csproj ./TestProject.WebAPI/

RUN dotnet restore

# Copy everything
#COPY . ./
COPY TestProject.Data/. ./TestProject.Data/
COPY TestProject.DTO/. ./TestProject.DTO/
COPY TestProject.Service/. ./TestProject.Service/
COPY TestProject.WebAPI/. ./TestProject.WebAPI/

# Restore as distinct layers
#RUN dotnet restore

# Build and publish a release
WORKDIR /app/TestProject.WebAPI
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/TestProject.WebAPI/out .
ENTRYPOINT ["dotnet", "TestProject.WebAPI.dll"]

#docker run command
#docker build -t prabhumuthusamy/altimetrik-api .
#docker run -p 8080:80 -d prabhumuthusamy/altimetrik-api
