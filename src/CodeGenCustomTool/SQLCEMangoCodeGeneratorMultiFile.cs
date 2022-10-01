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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using Microsoft.CustomTool;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    [Guid("5716BE0C-EEE4-418C-99A9-1DD56765F83B")]
    [ComVisible(true)]
    public class SQLCEMangoCodeGeneratorMultiFile : MultipleFileGenerator
    {
        public override void Generate(string wszInputFilePath,
                                      string bstrInputFileContents,
                                      string wszDefaultNamespace,
                                      out IntPtr rgbOutputFileContents,
                                      out int pcbOutput,
                                      IVsGeneratorProgress pGenerateProgress)
        {
            try
            {
                var database = CodeGeneratorCustomTool.GetDatabase(wszDefaultNamespace, wszInputFilePath);
                var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
                codeGenerator.GenerateEntities();
                codeGenerator.GenerateDataAccessLayer();

                var header = new StringBuilder();
                codeGenerator.WriteHeaderInformation(header);

                var files = new Dictionary<string, StringBuilder>(codeGenerator.CodeFiles.Count);
                foreach (var codeFile in codeGenerator.CodeFiles)
                    files.Add(codeFile.Key + GetDefaultExtension(), codeFile.Value);

                AddOutputToProject(wszInputFilePath, files, header);

                base.Generate(wszInputFilePath,
                              bstrInputFileContents,
                              wszDefaultNamespace,
                              out rgbOutputFileContents,
                              out pcbOutput,
                              pGenerateProgress);
            }
            catch (Exception)
            {
                var codeGen = new SQLCEMangoCodeGenerator();
                codeGen.Generate(wszInputFilePath, bstrInputFileContents, wszDefaultNamespace, out rgbOutputFileContents,
                                 out pcbOutput, pGenerateProgress);

                //var applicationException = new ApplicationException("Unable to generate code", e);
                //var messageBox = new ExceptionMessageBox(applicationException);
                //messageBox.Show(null);
                //throw;
            }
        }

        #region Documentation

        protected override string GetDocumentation()
        {
            return
                @"
/*
The Generated Code
A LINQ to SQL Data Context class is generated that contains every table in the database and 2 constructor methods. The default constructor sets the connection to use ""Data Source=isostore:/SDF File"" while the other constructor accepts a connection string as an argument. The constructor checks if the database exists using the connection string, the database is created if it does not exist. The database is created by calling the CreateDatabase() method of the DataContext base class.
Using the Generated Windows Phone 7 ""Mango"" Data Context Code
using (var dataContext = new TestDatabaseDataContext())
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

    // Create record
    dataContext.Contact.InsertOnSubmit(contact);
    dataContext.SubmitChanges();

    // Retrieve records
    var contacts = dataContext.Contact.ToList();
}
*";
        }

        #endregion
    }
}