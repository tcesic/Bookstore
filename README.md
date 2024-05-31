# Bookstore

# Introduction
Bookstore API is a RESTful API built with .NET Core 6.0 for managing a collection of items. It includes Swagger for API documentation and testing.
It has different types authorization for different API calls - defined different policies for this purpose. 

Bookstore project uses DataAccess project for manipulation with database entites.
DataAccess is project for creating and seeding database and do all manipulations with EntityFrameforkCore 6.0.

# Features
CRUD operations for items
Search of items
JWT Authentication
OAuth - Client credentials and Implicit flow
Swagger Documentation
Docker support

# Prerequisites
.NET 6.0
SQL Server

# Installation and testing

1. Clone the repository:
git clone https://github.com/tcesic/Bookstore.git
cd Bookstore
2. Restore dependencies
3. Set up the database:
Update connection string in appsettings.json in main project and database with some mocked data would be created first time you run the app (Code First Approach)
{
  "ConnectionStrings": {
    "Default": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookStoreDb;Integrated Security=True"
}

4. Run the main project (set up to run https://localhost:7037 if busy you can change to port you want to use in launchSettings.json of main project)
5. IdentityServer4 is used and running on the same port as application.
6. Use the main project APIs 

- For client_credentials authorization you can click authorize on swagger and choose that type and select scope and authorize. It goes to url 
of Identity server and in response you got the tocken (Inspect browser and than copy jwt token). Than go to authorize on swagger again and autorize
with jwt bearer. Now you can use all APIs authorize on this way.
- For implicit flow credentials everything is set up so it need autorization that type and also swagger suport to choose that type,
but due to lack of time not enought time to create some page with login logout,.. so if you want to check api only you can comment Policy just 
to test search

# Docker 

There is support for docker (both docker and docker compose files). You need to have docker engine running on windows. 
Please change connection string in appsettings.json (use one with comment Docker).
You can run docker build and docker compose up and application is running. The only problem is that it is running only on http and we need https.
I added support for certificates in docker, so you just need to put them in certs folder of nginx and in nginx.conf to read them.


So for purpose of testing because docker not finished fully you need to run project locally from VS. 