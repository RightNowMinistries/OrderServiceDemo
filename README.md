## RightNow Media Demo Service

[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=rnm-order-service-demo&metric=alert_status)](https://sonarcloud.io/dashboard/index/OrderServiceDemo)

The RightNow Media back-end platform consists of several web apis running .net core on linux-based containers. On average, the back end services are processing 40,000 web calls per minute, built to be highly scalable and high throughput (our target is to serve 99% of requests in less than 1 second. Our average response time on these requests is less than 100 ms).

This application is a model built structurally to look like our real services and to simulate what working in our code may look like. Many things are reduced down to help simplify the exercise, but if you are able to navigate around this code, you will likely feel right at home in our actual services.

## TODO

Look for comments throughout the code base with a 'TODO' note on them. Academically, this demo should provide you with an overview of how we structure our code and what we consider best practices (which we believe closely aligns to industry wide standards while also integrating into the real-world business problems our application stack must address).

Functionally, we expect you to complete the 'TODO' items in a manner consistent with best practices and the code you see. At a summary level, you have two endpoints to implement in the OrderController:

* The 'CancelOrder' endpoint should be implemented to change the status of the order provided to an OrderStatus of 'Cancelled'. If the order does not exist, the endpoint should return an error. In the event that the order exists but has already been cancelled, you should also throw an error.
* The 'DeleteOrder' endpoint should be implemented to completely delete the order provided from the repositories. Note that the Order Line Items will also need to be deleted.

Happy coding.

## How to Build

This project was built with Visual Studio 2017 (15.7.5) using .NET Core Runtime 2.1.5. If you are familiar with these tools, you should be able to open the solution, compile, and go.

For those who may be new to this ecosystem, you will need the following:

* The .NET Core SDK (SDK version 2.1.403 includes .NET Core Runtime 2.1.5). It can be downloaded [Here](https://github.com/dotnet/core/blob/master/release-notes/2.1/2.1.5/2.1.5.md)
* While we use Visual Studio 2017 on windows as our primary IDE, this sample should also execute fine using [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/)
* For those that would prefer to just use a text editor, that would be fine too. The application can be built / launched via the following set of commands from a cmd, powershell, or bash window. These commands are from the root directory of the code base. Note that depending on your OS, your slashes may vary.
```
dotnet restore
dotnet build
dotnet run --project .\src\OrderServiceDemo\OrderServiceDemo.csproj
```

## Project Overview

### Core

Our "Core" project serves as our contracts from a web api. Classes within this library should be versioned by namespace to match the version of a particular endpoint from which they are used.

All communication into and out of the web api should be in the form of 'Core' classes. Further, after deployment, all of these classes can be extended, but cannot be changed. Examples of permitted extension could be the addition of an optional field on a request (or a non-optional field whose default value provides identical behaviors). Non-permitted breaking changes would be type changes (an int changed to a string), removal of fields, or more drastic model changes that would not JSON Serialize/Deserialize exactly the same as before the change.

### Models

Our "Models" project services as our business object / POCO layer. Our processing code, database entities, etc. These are NOT versioned, but instead are the single version of the objects that our code base runs against. They are also considered an implementation detail to the web api, and as such are not exposed to code bases external to the project.

### Services

The "engine" for the web api, this layer is home to business logic and data access code, though these two things exist with logical separation of concerns.

#### Components

The business logic center of the application, these would be designed around either object management or business process (e.g. management of Speakers may be in a SpeakerService while a complex process like User Invites and Registration, which involves several objects, may be a general RegistrationService). The component should perform any business logic checks to data it is processing and would articulate problems back up to the web api layer in the form of strongly typed Exceptions (such as an "ThingNotFound" exception or a "UpdateNotAllowedException" ... something to indicate the reason for the error).

These services should be built using Constructor Injection of dependencies (following the best practice of depending on abstractions, not concretions) so that our 'Services' can be unit tested using Mocks of repository or integration services. For more reading on Dependency Injection in .net core, refer to [their documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).

#### Repositories

Generally following a repository pattern, these classes should be table-centric and should allow for basic CRUD (where appropriate) and paged or specialized retrieval (based on business requirements) of various objects. Only under extraordinary exception would any business logic ever find its way down into this layer of code.
