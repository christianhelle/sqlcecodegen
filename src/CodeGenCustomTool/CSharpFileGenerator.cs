#region License
// The MIT License (MIT)
// 
// Copyright (c) 2009 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.CustomTool;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    public abstract class CSharpFileGenerator : IVsSingleFileGenerator
    {
        #region IVsSingleFileGenerator Members

        public string GetDefaultExtension()
        {
            return ".cs";
        }

        public virtual void Generate(string wszInputFilePath,
                                     string bstrInputFileContents,
                                     string wszDefaultNamespace,
                                     out IntPtr rgbOutputFileContents,
                                     out int pcbOutput,
                                     IVsGeneratorProgress pGenerateProgress)
        {
            var summaryData = Encoding.Default.GetBytes(GetDocumentation());
            rgbOutputFileContents = Marshal.AllocCoTaskMem(summaryData.Length);
            Marshal.Copy(summaryData, 0, rgbOutputFileContents, summaryData.Length);
            pcbOutput = summaryData.Length;
        }

        #endregion

        #region Documentation

        protected virtual string GetDocumentation()
        {
            return
                @"
/*
The Generated Code
Entity classes representing each table in the database are generated. The max length of string fields are checked and exceptions are thrown 
when these are exceeded. The generated data access methods use these entity classes as return values or as arguments. 
The data access code generated implements the repository pattern and is extensible, the interfaces and class definitions are defined as public and partial
but non of the methods are virtual or in other words can not be overridden.

The following methods are generated for retrieving data
ToList() - Retrieves all the records in a table as a List<T>
ToList(int count) - Retrieves records in a table filtered by the TOP statement as a List<T>
ToArray() - Retrieves all the records in a table as an array
ToArray(int count) - Retrieves records in a table filtered by the TOP statement as an array
SelectByXXX(xxx) - Retrieves records in a table filtered by a column. A SelectBy method is generated for each column in the table
Count() - Retrieves the number of records in a table

The following methods are generated for inserting data
Create(T) - Accepts an instance of T and inserts the data accordingly
Create(xxx) - Accepts every column in the table and inserts the data accordingly
Create(IEnumerable<T>) - Accepts a collection of T and inserts using an updatable result set for optimal performance

The following methods are generated for updating data
Update(T) - Accepts an instance of T and updates the data filtered by the table's primary key
UpdateByXXX(xxx) - Updates records in a table filtered by a column. A UpdateBy method is generated for each column in the table
Update(IEnumerable<T>) - Accepts a collection of T and updates the data using a pre-compiled SQL command for optimal performance

The following methods are generated for deleting data
Delete(T) - Accepts an instance of T and deletes the record by filtering on all columns
DeleteByXXX(xxx) - Deletes records in a table filtered by a column. A DeleteBy method is generated for each column in the table
Delete(IEnumerable<T>) - Accepts a collection of T and deletes every record using a pre-compiled SQL command for optimal performance
Purge() - Deletes all records in the table
Using the generated code
The Global Connection String

A static class called EntityBase contains the global connection string used through out the applications life cycle. 
This can be set manually through the applications start up process, the developer only has to do this once. 
If this global connection string is not set it will default to ""Data Source='database file name'"". 
The developer can also set the global connection string a the constructor method overload in the DataRepository class

Consuming data

Several classes and methods are generated and there main 2 ways of consuming the generated code for data access: with or without transactions. 
Too enable transactions just call the BeginTransaction method on the DataRepository, the transaction is by default rolled back unless Commit is called.
Without transactions changes are immediately persisted to the database.

Basic Usage

using (var repository = new DataRepository(""Data Source=TestDatabase.sdf""))
{
    var contact = new Contact
    {
        Name = ""Christian Helle"",
        Address = ""Somewhere"",
        City = ""Over"",
        PostalCode = ""The"",
        Country = ""Rainbow"",
        Email = ""christian.helle@yahoo.com"",
        Phone = ""1234567""
    };

    // Create
    repository.Contact.Create(contact);
    repository.Contact.Create(contact);

    // Select
    var contacts = repository.Contact.ToList();
    contacts = repository.Contact.SelectByName(""Christian Resma Helle"");
    contacts = repository.Contact.SelectByEmail(""christian.helle@yahoo.com"");

    // Count
    var count = repository.Contact.Count();

    // Update
    contact.Name = ""Christian Resma Helle"";
    repository.Contact.Update(contact);

    // Delete
    repository.Contact.Delete(contact);
    repository.Contact.Purge();
}


Using Transactions

using (IDataRepository repository = new DataRepository(""Data Source=TestDatabase.sdf""))
{
    // Start the transaction (only one active transaction is allowed per DataRepository instance)
    repository.BeginTransaction();

    var contact = new Contact
    {
        Name = ""Christian Helle"",
        Address = ""Somewhere"",
        City = ""Over"",
        PostalCode = ""The"",
        Country = ""Rainbow"",
        Email = ""christian.helle@yahoo.com"",
        Phone = ""1234567""
    };

    // Create
    repository.Contact.Create(contact);
    repository.Contact.Create(contact);

    // Select
    var contacts = repository.Contact.ToList();
    contacts = repository.Contact.SelectByName(""Christian Resma Helle"");
    contacts = repository.Contact.SelectByEmail(""christian.helle@yahoo.com"");

    // Count
    var count = repository.Contact.Count();

    // Update
    contact.Name = ""Christian Resma Helle"";
    repository.Contact.Update(contact);

    // Delete
    repository.Contact.Delete(contact);
    repository.Contact.Purge();

    // Commits the transaction
    repository.Commit();
}
*/";
        }

        #endregion
    }
}