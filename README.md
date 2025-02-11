## Prerequisites
Before run the application , ensure you have the following installed:
- [.NET Core SDK 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- [.NET Hosting Bundle](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-9.0.2-windows-hosting-bundle-installer) for prod environment
- [MySQL Server](https://dev.mysql.com/downloads/mysql) 
  
## Database Setup
run the attached file **migration.sql** before running the application

## Building

To build the project run:
```bash
dotnet build
```

## Running the Application
running thes commands as following
- navigate to /ITIDA-Task-Backend then run this command
```bash
dotnet run
```
- navigate to ITIDATask.Test to run test proj then run this command
```bash
dotnet test
```
- The backend API is documented using Swagger. You can access the API documentation at:  (http://localhost:5296/swagger/index.html) Dev Environment only 

## Publish Files
publish file is also attached for prod environment 
- to run it need to run the Executable file **ITIDATask.exe**   
