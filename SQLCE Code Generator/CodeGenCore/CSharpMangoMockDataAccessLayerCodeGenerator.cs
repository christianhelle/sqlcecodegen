using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpMangoMockDataAccessLayerCodeGenerator : CodeGenerator
    {
        public CSharpMangoMockDataAccessLayerCodeGenerator(SqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        public override void GenerateDataAccessLayer()
        {
            GenerateDataAccessLayer(new DataAccessLayerGeneratorOptions());
        }

        public override void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options)
        {
            Trace.WriteLine("Generating Mock Repository Implementation");

            GenerateMockImplementation();
        }

        private void GenerateMockImplementation()
        {
            GenerateMockDataRepository();

            foreach (var table in Database.Tables)
            {
                var mockRepositories = GenerateMockRepositories(table);
                AppendCode("Mock" + table.ClassName + "Repository", mockRepositories);
            }
        }

        private void GenerateMockDataRepository()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");
            code.AppendLine("\tpublic partial class MockDataRepository : IDataRepository");
            code.AppendLine("\t{");
            code.AppendLine();

            code.AppendLine("\t\tpublic MockDataRepository()");
            code.AppendLine("\t\t{");
            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t\t\t" + table.ClassName + " = new Mock" + table.ClassName + "Repository();");
            }
            code.AppendLine("\t\t}");
            code.AppendLine();

            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t\tpublic I" + table.ClassName + "Repository " + table.ClassName + " { get; private set; }");
                code.AppendLine();
            }

            code.AppendLine("\t\tpublic void SubmitChanges()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t}");
            code.AppendLine();

            code.AppendLine("\t\tpublic void Dispose()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t}");
            code.AppendLine();

            code.AppendLine("\t}");
            code.AppendLine("}");

            AppendCode("MockDataRepository", code);
        }

        private StringBuilder GenerateMockRepositories(Table table)
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");
            code.AppendLine("\tusing System.Linq;");
            code.AppendLine();
            code.AppendLine("\tpublic partial class Mock" + table.ClassName + "Repository : I" + table.ClassName + "Repository");
            code.AppendLine("\t{");
            code.AppendLine("\t\tpublic EntityDataContext DataContext { get; private set; }");
            code.AppendLine();

            DataAccessLayerGenerator generator = new CSharpMockDataAccessLayerCodeGenerator(code, table);
            generator.GenerateSelectAll();
            generator.GenerateSelectWithTop();
            generator.GenerateSelectBy();
            generator.GenerateSelectByWithTop();
            generator.GenerateCreate();
            generator.GenerateCreateIgnoringPrimaryKey();
            generator.GenerateCreateUsingAllColumns();
            generator.GeneratePopulate();
            generator.GenerateDelete();
            generator.GenerateDeleteBy();
            generator.GenerateDeleteAll();
            generator.GenerateUpdate();
            generator.GenerateCount();

            code.AppendLine("\t}");
            code.AppendLine("}");

            return code;
        }
    }
}
