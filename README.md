# Tombola Technical Challenge

Dear Tombola team,

Many thanks for the opportunity to complete this technical challenge and for being patient with me in delivering this to you during a rough time for my family, it's really much appreciated.

From the challenge provided, I have chosen to go soley with scenario 2. My goal for this solution was to build a lightweight, scalable, secure, and maintainable solution using industry standard tools and frameworks. Below I have outlined the key technologies used and the reasoning behind each choice. You can find the setup instructions in this GitHub repository -> Steup -> "Setup_Instructions_Readme"

## Visual Studio 2022
I have chosen this IDE due it's powerful features for .NET development, it's popularity, and due to my personal familiarity/experience with this development environment. Features such as robust debugging, IntelliSense, and integration with version control (Azure DevOps, Git) make it a great choice, in my opinion.

## ASP.NET (.NET 8)
There is a .NET 9 currently out but .NET 8 is the more stable and interoperable version at this time. So with that in mind and also .NET 8's dependency injection, routing, and Swagger support made it a great choice for building a supportable and scalable modern web app.

## JWT Tokens for authentication
For authentication, I considered OAuth (which I have used in previous projects, specifically for interacting with Trustpilot's API) or just using JWT tokens by themselves as they are both reliable and commonly used in the industry. I opted for using JWT tokens by themselves as JWT tokens are stateless, can be used for authentication + authorisation, and the token is self-contained with an expiration date/time. Which is perfect for the API we intend to build in this challenge and keeps things more simple.

## MSTest and Moq for Unit Testing
For unit testing, I used MSTest as the test framework and Moq to mock dependencies. I did consider using XUnit or NUnit frameworks instead of MSTest, but I stuck with MSTest as I have used it previously, it integrates well with Visual Studio, it's easy to understand, and works well. I have used Moq to create mock objects within my unit tests, and to take advantage of the dependency injection design patterns I have implemented within my solution to better test the controller, service and repository layers. For example, when testing the controller class methods, I have created mock Service objects within the unit tests to ensure the controller is calling the service correctly and handling the results.

## MySQL Server 9.2.0
For the database, I chose to go with MySQL Server (the latest stable version is 9.2.0) as it is lightweight, reliable, has compatibility with Entity Framework, can be used on multiple different operating system, and fits the requirement for "relational database schema to store the bean details from the JSON file". 

## Entity Framework 8 (with Pomelo 8 for MySQL)
I selected Entity Framework 8 (there is a version 9 but it isn't compatible with .NET 8) as the Object-Relational Mapping (ORM) tool to simplify database interactions and handle data access in a clean, consistent manner. To integrate with MySQL, I have Pomelo.EntityFrameworkCore.MySQL (version 8 for compatibility with EF 8 and .NET 8) as it is widely used and supported EF core provider (it is used in the EF model creations).

## Summary
The combination of these technologies provides a modern, supportable, scalable, secure and testable solution that fits the requirements of the challenge. I hope this meets your standards and that I get the chance to discuss it with you in person.

Kind regards,
David
