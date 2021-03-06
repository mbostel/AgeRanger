# Age Ranger

CRUD Demo of simple people database using two front ends, a swappable data provider and a Web Api.

The main purpose of this demo was to show an architecture that would support scaled-up development. It demonstrates a Lean approach to coding that takes only what is needed - hence the stripped-down WebApi and Browser UI. The Web Api can easily be extended with further controllers - but why would you? The service does what it needs to. A different resource should have it's own (micro) service.

The Browser UI is perhaps the wekest architectural link, a single-page framework would have supported extensibility, but I have to admit to having no experience in this area - and I wanted to keep the Service in it's own project.
##### Issues
1. In the AgeGroup table the first group (toddler) has what appears to be a null MinAge value, but it can't be selected with an "is null" predicate, it can't be wrapped in an "ifnull(MinAge, 0)" and it isn't identified as <null> in SQLiteStudio. I haven't been able to discover any explanation that would account for the nature of this value. It's easy enough to work around, but it would normally have to be fixed. is this part of the test? If so, I fail to see the purpose.

2. The 'official' .Net ADO wrapper System.Data.SQLite NuGet package is installed into the SQLite data provider project, but fails with "unable to load dll sqlite.interop.dll" when trying to establish a connection. This is a common problem reported on Stack Overflow, there are various fixes - none of which seem to work for everyone. The dll is required in the output directory of the component that calls the data provider (in this case, the WebApi). It can't be added to the project references and it can't be included in the project and copied (Embedded Resource + Copy always), so I added a Post-Build process. This is not an acceptable solution since the dll will have to be updated manually each time a new version of the ADO wrapper is used. 

3. The Browser UI has a problem with Chrome & Fiddler. Fiddler insists on updating the Proxy settings and Chrome doesn't like this. Probably a simple fix, but simpler was to just use Explorer.

4. There's no WebApi integration tests because I needed a way to use a test database instead of the live one. This worked ok with the data provider integration tests, but a different approach was required for the Web Api - one that would have involved time that I didn't have to solve a problem that didn't really need fixing (since this test is largely about demonstrating ability).

## Architecture
The solution namespace base is "DebitSuccess.AgeRanger" 
##### Framework
Usually this folder would hold the Business logic projects, but we don't have much of that in this project. The API project contains the ContainerFactory - which provides concrete instances of classes, and all the interfaces.

The Api project is referenced by most of the other projects.
##### Logging
ILogger & IlogMessage

The Logging project has one Real logger (the UI_Logger). The others are just stub examples of other logging components that could be used in different projects by declaring them in the appropriate container bootstrap (the WebApi and each of the UIs calls their respective ContainerBootstrap methods in the startup). The UI_Logger uses a Message Relayer to support Binding within the Views.
##### Data Providers
IPeopleDataProvider

SQLServer is a stub. Swapping over to SQLServer would involve referencing the project in the WebApi and updating the reference in the (IOC) ContainerBootstrap.

The SQLite project contains mostly CRUD methods. There's a base class (DataProviderBase) that does all the generic stuff (ExecuteQuery, Connect, Disconnect etc.) and a PeopleProvider class that provides a Business-oriented set of methods. In most projects I would have separated the generic database-specific classes into a separate project.

There's some hard-coding going on that wouldn't be present in a real application.

##### Services
Just a single WebApi Rest service. I didn't want to tie together a UI with the service and I didn't see the point in creating an MVC project template, or even a Single-Page template. I know, points off. the WebApi should stand on it's own feet, and it doesn't need a ton of cruft to support what should be lean and elegant.
##### User Interfaces
The Browser UI is simple, just HTML, JQuery, Bootstrap. Didn't need anything else.
the Desktop UI is WPF with my own MVVM library.

## Getting Started
Pull the AgeRanger solution, Rebuild, Mark either of the two UI projects as your startup, and go. 

### Prerequisites
.Net Framework 4.6

### Installing
You should have everything you need included in the project.

## Running the tests
The tests have been configured to run automatically

### Break down into end to end tests
There's very little that can be unit-tested since there's so little Business logic, so most of the tests are in the Integration Test projects.

## Deployment
No setup projects have been included for this demo as I'm assuming that it will be run from within VS.

## Built With

* Microsoft.AspNet.WebApi
* NUnit
* Newtonsoft Json
* Bootstrap
* JQuery
* Unity

## Authors

* **Mark Bostel** - *All work, iclusing the Azoth.UI.MVVM library*

## License
Use it and Lose it.
