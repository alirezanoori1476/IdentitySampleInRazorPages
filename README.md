# IdentitySampleInRazorPages

This is a sample project that demonstrates how to use Identity Server 4 with Razor Pages in .NET 7.

Identity Server 4 is an open source framework for implementing authentication and authorization using OpenID Connect and OAuth 2.0 protocols.

Razor Pages is a web development framework that simplifies building web UI using C# and HTML.

--------------------------------------------

Prerequisites
To run this project, you need the following tools:

.NET 7 SDK
SQL Server
Visual Studio or Visual Studio Code
  
--------------------------------------------

Installation
Follow these steps to install and run the project:

Clone this repository to your local machine.
Open the solution file IdentitySampleInRazorPages.sln in Visual Studio or Visual Studio Code.
If you have any package errors, restore the packages using the NuGet Package Manager or the dotnet CLI.
Check the connection string in the appsettings.json file and make sure it matches your SQL Server instance.
Update the database using the Entity Framework Core migrations. You can use the Package Manager Console or the dotnet CLI to run the command Update-Database.
Run the project using the F5 key or the dotnet run command.
Usage
Once the project is running, you can access it using a web browser at https://localhost:5001.

You can register a new user account or log in using an existing one.

You can also explore the different features and options of Identity Server 4, such as managing your profile, changing your password, logging out, etc.

--------------------------------------------

License
This project is licensed under the MIT License - see the LICENSE file for details.


--------------------------------------------

Author
This project was created by Alireza Noori: https://github.com/alirezanoori1476?tab=repositories.

If you like this project, please give it a star and share it with others. 😊
