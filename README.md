# Student Scheduler
## A student time management solution
The goal of this project is to create a centralised time management solution for students that will create a personalised
schedule based on all of the data about their upcoming assignments along with taking into consideration various constraints
across all modules. 

As well as creating study / work schedules, the platform provides students with a means to visualise all
of their upcoming work easily in one place while allowing them to alter or add custom notes to their work items, 
something Canvas in particular fails to do effectively.


## Tech Stack
![Tech Stack](https://skillicons.dev/icons?i=vue,dotnet,postgres)
- Frontend: Vue.js
- Backend: .NET 9 and Entity Framework
- Database: PostgreSQL
- Testing: XUnit and Moq

## Project Structure
DashboardWeb -> The frontend project
    - DashboardWeb/src contains the majority of the frontend source code

DashboardServer -> The backend server
    - Layered Architecture
        -> Controller Layer
        -> Service Layer
        -> Repository Layer (Data Access Layer)
    - DashboardServer/Models contains the data entities of the application
    - DashboardServer/Data contains the EntityFramework Core configuration

DashboardServer.Tests -> The testing project.
- To run tests, cd to the DashboardServer.Tests project and run 'dotnet test'.


## Usage
To run locally, firstly ensure you have .NET 9 and Node installed. Update appsettings.json to point to an instance of a PostgreSQL database.

Navigate to the DashboardServer and run
- dotnet restore
- dotnet ef database update
- dotnet run

Navigate into DashboardWeb and run
- npm install
- npm run dev
