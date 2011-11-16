using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;
using Microsoft.CustomTool;
using Microsoft.SqlServer.MessageBox;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    [Guid("5716BE0C-EEE4-418C-99A9-1DD56765F83B")]
    [ComVisible(true)]
    public class SQLCEMangoCodeGenerator : MultipleFileGenerator
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
            catch (Exception e)
            {
                var codeGen = new SQLCEMangoCodeGeneratorSingle();
                codeGen.Generate(wszInputFilePath, bstrInputFileContents, wszDefaultNamespace, out rgbOutputFileContents, out pcbOutput, pGenerateProgress);

                //var applicationException = new ApplicationException("Unable to generate code", e);
                //var messageBox = new ExceptionMessageBox(applicationException);
                //messageBox.Show(null);
                //throw;
            }
        }

        #region Documentation
        protected override string GetDocumentation()
        {
            return @"
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