# Tombola Technical Challenge

Dear Tombola team,

Many thanks for the opportunity to complete this technical challenge and for being patient with me in delivering this to you during a rough time for my family, it's really much appreciated.

From the challenge provided, I have chosen to go soley with scenario 2 as it fit better with my strengths. My goal for this solution was to build a lightweight, scalable, secure, and maintainable solution using industry standard tools and frameworks. Below I have outlined the key technologies used and the reasoning behind each choice:

## Visual Studio 2022
I have chosen this IDE due it's powerful features for .NET development, it's popularity, and due to my personal familiarity/experience with this development environment. Features such as robust debugging, IntelliSense, and integration with version control (Azure DevOps, Git) make it a great choice, in my opinion.

## ASP.NET (.NET 8)
There is a .NET 9 currently out but .NET is the more stable and interoperable version at this time. So with that in mind and also .NET 8's dependency injection, routing, and Swagger support made it a great choice for building a supportable and scalable modern web app.

## JWT Tokens for authentication
For authentication, I considered OAuth (which I have used in previous projects, specifically for interacting with Trustpilot's API) or just using JWT tokens by themselves as they are both reliable and commonly used in the industry. I opted for using JWT tokens by themselves as JWT tokens are stateless, can be used for authentication + authorisation, and the token is self-contained with an expiration date/time. Which is perfect for the API we intend to build in this challenge and keeps things more simple.

… to be continued
