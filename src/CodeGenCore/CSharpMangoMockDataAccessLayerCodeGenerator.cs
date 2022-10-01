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
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpMangoFakeDataAccessLayerCodeGenerator : CodeGenerator
    {
        public CSharpMangoFakeDataAccessLayerCodeGenerator(ISqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        public override void GenerateDataAccessLayer()
        {
            Trace.WriteLine("Generating Fake Repository Implementation");

            GenerateFakeImplementation();
        }

        private void GenerateFakeImplementation()
        {
            GenerateFakeDataRepository();

            foreach (var table in Database.Tables)
            {
                var FakeRepositories = GenerateFakeRepositories(table);
                AppendCode("Fake" + table.ClassName + "Repository", FakeRepositories);
            }
        }

        private void GenerateFakeDataRepository()
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            code.AppendLine("\tpublic partial class FakeDataRepository : IDataRepository");
            code.AppendLine("\t{");
            code.AppendLine();

            code.AppendLine("\t\tpublic FakeDataRepository()");
            code.AppendLine("\t\t{");
            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t\t\t" + table.ClassName + " = new Fake" + table.ClassName + "Repository();");
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

            AppendCode("FakeDataRepository", code);
        }

        private StringBuilder GenerateFakeRepositories(Table table)
        {
            var code = new StringBuilder();

            code.AppendLine("\nnamespace " + Database.DefaultNamespace);
            code.AppendLine("{");
            code.AppendLine("\tusing System.Linq;");
            code.AppendLine();
            code.AppendLine("\tpublic partial class Fake" + table.ClassName + "Repository : I" + table.ClassName + "Repository");
            code.AppendLine("\t{");
            code.AppendLine("\t\tpublic EntityDataContext DataContext { get; private set; }");
            code.AppendLine();

            DataAccessLayerGenerator generator = new CSharpFakeDataAccessLayerCodeGenerator(code, table, false);
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
