using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class UnitTestCodeGenerator : CodeGenerator
    {
        public UnitTestCodeGenerator(SqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        public override void GenerateEntities()
        {
            GenerateEntities(new EntityGeneratorOptions());
        }

        public override void GenerateEntities(EntityGeneratorOptions options)
        {
            Trace.WriteLine("Generating Entity Unit Tests");

            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");

            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]");
                code.AppendLine("\tpublic class " + table.TableName + "EntityTest");
                code.AppendLine("\t{");

                foreach (var column in table.Columns)
                {
                    GeneratePropertyTest(table, column);

                    if (column.Value.ManagedType.Equals(typeof(string)))
                        GenerateStringMaxLengthTest(table, column);
                }

                code.AppendLine("\t}");
                code.AppendLine();
            }

            code.AppendLine(@"
    internal static class RandomGenerator
    {
        const string PWD_CHARSET = ""abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVXYZ1234567890"";

        public static string GenerateString(int len)
        {
            if (len > 4000) len = 4000;
            var buffer = new byte[len * 2];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(buffer);

            using (var stream = new System.IO.MemoryStream(buffer, 0, buffer.Length, false, false))
            using (var reader = new System.IO.BinaryReader(stream))
            {
                var builder = new System.Text.StringBuilder(buffer.Length, buffer.Length);
                while (len-- > 0)
                {
                    var i = (reader.ReadUInt16() & 8) % PWD_CHARSET.Length;
                    builder.Append(PWD_CHARSET[i]);
                }
                return builder.ToString();
            }
        }
    }");
            code.AppendLine("}");
        }

        private void GeneratePropertyTest(Table table, KeyValuePair<string, Column> column)
        {
            Trace.WriteLine("Generating " + column.Value.Name + "Test()");

            code.AppendLine("\t\t[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]");
            code.AppendLine("\t\tpublic void " + column.Value.Name + "Test()");
            code.AppendLine("\t\t{");

            if (column.Value.ManagedType.Equals(typeof(string)))
                code.AppendLine("\t\t\tvar value = string.Empty;");
            else if (column.Value.ManagedType.IsArray)
                code.AppendLine("\t\t\tvar value = new " + column.Value.ManagedType.ToString().Replace("[]", "[1];"));
            else
                code.AppendLine("\t\t\tvar value = new " + column.Value.ManagedType + "();");

            code.AppendLine("\t\t\tvar target = new " + table.TableName + "();");
            code.AppendLine("\t\t\ttarget." + column.Value.Name + " = value;");
            code.AppendLine();
            code.AppendLine("\t\t\tMicrosoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(value, target." + column.Value.Name + ");");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GenerateStringMaxLengthTest(Table table, KeyValuePair<string, Column> column)
        {
            if (!column.Value.ManagedType.Equals(typeof(string)))
                return;

            Trace.WriteLine("Generating " + column.Value.Name + "MaxLengthTest()");

            code.AppendLine("\t\t[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]");
            code.AppendLine("\t\tpublic void " + column.Value.Name + "MaxLengthTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar value = RandomGenerator.GenerateString(" + column.Value.MaxLength.Value + ");");
            code.AppendLine("\t\t\tvar target = new " + table.TableName + "();");
            code.AppendLine("\t\t\ttarget." + column.Value.Name + " = value;");
            code.AppendLine();
            code.AppendLine("\t\t\tMicrosoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(value, target." + column.Value.Name + ");");
            code.AppendLine("\t\t}");
            code.AppendLine();

            if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 ||
                string.Compare(column.Value.DatabaseType, "text", true) == 0)
                return;

            Trace.WriteLine("Generating " + column.Value.Name + "MaxLengthArgumentExceptionTest()");

            code.AppendLine("\t\t[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]");
            //code.AppendLine("\t\t[Microsoft.VisualStudio.TestTools.UnitTesting.ExpectedException(typeof(System.ArgumentException))]");
            code.AppendLine("\t\tpublic void " + column.Value.Name + "MaxLengthArgumentExceptionTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\ttry");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tvar value = RandomGenerator.GenerateString(" + column.Value.MaxLength.Value + 1 + ");");
            code.AppendLine("\t\t\t\tvar target = new " + table.TableName + "();");
            code.AppendLine("\t\t\t\ttarget." + column.Value.Name + " = value;");
            code.AppendLine("\t\t\t\tMicrosoft.VisualStudio.TestTools.UnitTesting.Assert.Fail(\"ArgumentException expected\");");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\tcatch (System.ArgumentException) { }");
            code.AppendLine("\t\t\tcatch (System.Exception)");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tMicrosoft.VisualStudio.TestTools.UnitTesting.Assert.Fail(\"ArgumentException expected\");");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        public override void GenerateDataAccessLayer()
        {
            throw new NotImplementedException();
        }

        public override void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
