
# NoteBlend - A web application for saving lecture notes

This is a semester project (PSI part 1)
Main idea was to make an application where students are able to save lecture notes as well as add comments, collaborate on and so on.

This is still work in progress!


## Section list
[Getting started: IDE setup](#getting-started:-ide-setup)

[Getting started: Cloning the project and starting it](#getting-started:-cloning-the-project-and-starting-it)

[.NET profiles: Rider](#.net-profiles:-rider)

[.NET profiles: VS Code](#.net-profiles:-vs-code)

[1. Database setup: PostgreSQL](#1.-database-setup:-postgresql)

[2. Database setup: Inside Rider](#2.-database-setup:-inside-rider)

[2. Database setup: Using a terminal (for VSC)](#2.-database-setup:-using-a-terminal-(for-vsc))

[API Reference](#api-reference)

[Objectives](#objectives)
## Getting started: IDE setup

Prefered IDE for this project is Rider (JetBrains)
[Download link](https://www.jetbrains.com/rider/)

## Getting started: Cloning the project and starting it

The following step is only for Windows:

- Make sure that you [git](https://git-scm.com/) installed on your machine

- Clone the repository using this command
```bash
git clone <repository link>
```

Project with all of it's branches should now be accessible. Now open the .sln file using an IDE.

## .NET profiles: Rider

Inside of Rider, there should be two preconfigured profiles called

- Project: http
- API Only

If profiles are not working as they should, be sure to check the configurations, mainly the "**Launch profile**"
![image](https://github.com/HubertasVin/PSI-1/assets/39692726/e02cb2e2-ddc2-4926-a2c9-964752214de1)

## .NET profiles: VS Code

To run this project, this command should be used:
```bash
dotnet run
```
with --launch-profile options:

- "Project"
- "API Only"

Looks like this in the end:
```bash
dotnet run --launch-profile "API Only"
```

## 1. Database setup: PostgreSQL

This project uses a locally stored PostgreSQL database (for now while the database structure is being perfected)

- Download [PostgreSQL](https://www.postgresql.org/download/)
- Launch pgAdmin 4
- Add a new server called NoteBlend (main user should be **postgres** and password **root**)
SQL server should now be running without any tables

## 2. Database setup: Inside Rider

- Make sure you have installed the Entity Framework Core UI

You can enter the plugin menu using **CTRL+SHIFT+X** shortcut

![image](https://github.com/HubertasVin/PSI-1/assets/39692726/286ffae0-d62f-43c1-888d-ff5f5eeeb283)

- Right click on the project, go to Tools -> Entity Framework Core -> Update database

![image](https://github.com/HubertasVin/PSI-1/assets/39692726/03a521fd-de22-405d-8af4-624f024c2a0e)

This should update the PostgreSQL database with all the required tables (if there are problems contact - [@Sc0rpie](https://github.com/Sc0rpie))

## 2. Database setup: Using a terminal (for VSC)

You can update the database using this command:
```bash
dotnet ef database update
```

Last created migration should be used automatically
## API Reference

There are only two API's available at the moment which are Subject and Topic APIs

#### Subject API

- Get all subjects
```http
  GET https://localhost:7015/subject/list
```

- Get subject by it's Id
```http
  GET https://localhost:7015/subject/get/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to fetch |

- Add subject to database
```http
  POST https://localhost:7015/subject/upload
```

Header should be set to **Content-type: application/json**
This API call accepts raw JSON data:
```json
{
    "subjectName": "subjectname"
}
```
#

#### Topic API

- Get a list of topics in a subject
```http
  GET https://localhost:7015/topic/list/${subjectID}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to fetch |

- Get topic by Id 
```http
  GET https://localhost:7015/topic/get/${ID}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to fetch |

- Add topic to subject
```http
  POST https://localhost:7015/topic/upload
```

Header should be set to **Content-type: application/json**
This API call accepts raw JSON data:
```json
{
    "topicName": "topicname",
    "subjectId": "id_of_subject"
}
```


## Objectives

Laboratory assignment 2 objectives:

- [ ] Relational database is used for storing data
- [ ] Create generic method, event or delegate; define at least 2 generic constraints
- [ ] Delegates usage
- [ ] Create at least 1 exception type and throw it; meaningfully deal with it; (most of the exceptions are logged to a file or a server)
- [ ] Lambda expressions usage
- [ ] Usage of threading via Thread class
- [ ] Usage of async/await
- [ ] Use at least 1 concurrent collection or Monitor
- [ ] Regex usage
- [ ] No instances are created using 'new' keyword, dependency injection is used everywhere
- [ ] Unit and integration tests coverage at least 20%

