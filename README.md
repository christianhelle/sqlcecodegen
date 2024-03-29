# SQL Compact Code Generator

Includes a stand alone GUI application and a Visual Studio Custom Tool for automatically generating a .NET data access layer code for objects in a SQL Server Compact Edition database.

**Features:**

*   Visual Studio 2008 and 2010 Custom Tool Support
*   Creates entity classes for each table in the database
*   Generates data access code that implements the Repository Pattern
*   Generates methods for Create, Read, Update and Delete operations
*   Generates SelectBy and DeleteBy methods for every column in every table
*   Generates a Purge method for every table to delete all records
*   Generates Count() method for retrieving the number of records in each table
*   Generates CreateDatabase() method for re-creating the database
*   Generates xml-doc code comments for entities and data access methods
*   Generates Entity Unit Tests
*   Generates Data Access Unit Tests
*   Generates .NET Compact and Full Framework compatible code
*   Support for SQL Compact Edition version 4.0
*   Multiple test framework code generation (MSTest, NUnit, xUnit)
*   Transaction support per DataRepository instance (Begin, Commit, Rollback)
*   Code generation options to enable/disable unit test code generation
*   Windows Phone 7 "Mango" support for generating a LINQ to SQL DataContext

**Coming Soon:**

*   Generate database maintenance code (clear database, shrink/compress database)
*   Support for multiple versions of SQL Server Compact Edition
*   VB.NET Code Support
*   Visual Studio Add-in
*   Visual Studio 2012 support

**Screenshots:**

Generating Entity Classes
![](images/sqlcecodegen-entities.png)

Generating Data Access methods that implement the Repository Pattern
![](images/sqlcecodegen-dal.png)

Generating Entity Unit Tests
![](images/sqlcecodegen-mock.png)

Export files
![](images/sqlcecodegen-export-files.png)

![](images/sqlcecodegen-export-output.png)

For tips and tricks on mobile development, check out my blog
[https://christianhelle.com](https://christianhelle.com)

Follow me on Twitter
[http://twitter.com/christianhelle](http://twitter.com/christianhelle)
