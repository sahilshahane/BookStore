### Prerequisites

[.NET SDK (v8.0)](https://dotnet.microsoft.com/en-us/download)

### Installation

1. Clone the repository

```sh
git clone https://github.com/sahilshahane/BookStore
cd BookStore
```

2. Install C# Dependencies

```sh
dotnet restore
```

3.  Configuring the database `(optional step)`

    A default database is already provided but if new database is required then follow below steps:

    - Update the "ConnectionStrings.BookStore" property in [appsettings.json](/BookStore/appsettings.json)

    - Apply migrations to newly created database

      ```
      dotnet ef database update
      ```

### Starting the Web Server

```sh
dotnet run
```

### Features

- List books
- Create a book
- Delete a book
- Edit a book
- Searching book: by title or author
- Pagination : KeySet based

### Tech Stack

- [ASP.NET Core MVC](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0)
- [SQLite](https://www.sqlite.org/)
- [Entity Framework Core v8](https://learn.microsoft.com/en-us/ef/core/)
- [Bootstrap v5](https://getbootstrap.com/docs/5.3/getting-started/introduction/)
- [Jquery v3](https://releases.jquery.com/)
- [Razor views](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-8.0)
