using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.SqlServerCe;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public abstract class CSharpUnitTestCodeGenerator : CSharpCodeGenerator
    {
        protected CSharpUnitTestCodeGenerator(SqlCeDatabase tableDetails)
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

            IncludeUnitTestNamespaces();
            code.AppendLine();

            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t" + GetTestClassAttribute());
                code.AppendLine("\tpublic class " + table.ClassName + "EntityTest");
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

            GenerateRandomGenerator();
            code.AppendLine("}");
        }

        protected abstract void IncludeUnitTestNamespaces();
        protected abstract string GetTestClassAttribute();
        protected abstract string GetTestMethodAttribute();
        protected abstract string GetTestInitializeAttribute();

        private void GenerateRandomGenerator()
        {
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
        }

        private void GeneratePropertyTest(Table table, KeyValuePair<string, Column> column)
        {
            Trace.WriteLine("Generating " + column.Value.FieldName + "Test()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void " + column.Value.FieldName + "Test()");
            code.AppendLine("\t\t{");

            if (column.Value.ManagedType.Equals(typeof(string)))
                code.AppendLine("\t\t\tvar value = string.Empty;");
            else if (column.Value.ManagedType.IsArray)
                code.AppendLine("\t\t\tvar value = new " + column.Value.ManagedType.ToString().Replace("[]", "[1];"));
            else
                code.AppendLine("\t\t\tvar value = new " + column.Value.ManagedType + "();");

            code.AppendLine("\t\t\tvar target = new " + table.ClassName + "();");
            code.AppendLine("\t\t\ttarget." + column.Value.FieldName + " = value;");
            code.AppendLine();
            code.AppendLine("\t\t\tAssert.AreEqual(value, target." + column.Value.FieldName + ");");
            code.AppendLine("\t\t}");
            code.AppendLine();

            Trace.WriteLine("Generating " + column.Value.FieldName + "NullTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void " + column.Value.FieldName + "NullTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar target = new " + table.ClassName + "();");
            code.AppendLine("\t\t\ttarget." + column.Value.FieldName + " = null;");
            code.AppendLine();
            code.AppendLine("\t\t\tAssert.AreEqual(null, target." + column.Value.FieldName + ");");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GenerateStringMaxLengthTest(Table table, KeyValuePair<string, Column> column)
        {
            if (!column.Value.ManagedType.Equals(typeof(string)))
                return;

            Trace.WriteLine("Generating " + column.Value.FieldName + "MaxLengthTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void " + column.Value.FieldName + "MaxLengthTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar value = RandomGenerator.GenerateString(" + column.Value.MaxLength.Value + ");");
            code.AppendLine("\t\t\tvar target = new " + table.ClassName + "();");
            code.AppendLine("\t\t\ttarget." + column.Value.FieldName + " = value;");
            code.AppendLine();
            code.AppendLine("\t\t\tAssert.AreEqual(value, target." + column.Value.FieldName + ");");
            code.AppendLine("\t\t}");
            code.AppendLine();

            if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 ||
                string.Compare(column.Value.DatabaseType, "text", true) == 0)
                return;

            Trace.WriteLine("Generating " + column.Value.FieldName + "MaxLengthArgumentExceptionTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            //code.AppendLine("\t\t[ExpectedException(typeof(System.ArgumentException))]");
            code.AppendLine("\t\tpublic void " + column.Value.FieldName + "MaxLengthArgumentExceptionTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\ttry");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tvar value = RandomGenerator.GenerateString(" + (column.Value.MaxLength.Value + 1) + ");");
            code.AppendLine("\t\t\t\tvar target = new " + table.ClassName + "();");
            code.AppendLine("\t\t\t\ttarget." + column.Value.FieldName + " = value;");
            code.AppendLine("\t\t\t\tAssert.Fail(\"ArgumentException expected\");");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t\tcatch (System.ArgumentException) { }");
            code.AppendLine("\t\t\tcatch (System.Exception)");
            code.AppendLine("\t\t\t{");
            code.AppendLine("\t\t\t\tAssert.Fail(\"ArgumentException expected\");");
            code.AppendLine("\t\t\t}");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        public override void GenerateDataAccessLayer()
        {
            GenerateDataAccessLayer(new DataAccessLayerGeneratorOptions());
        }

        public override void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options)
        {
            Trace.WriteLine("Generating Data Access Tests");

            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");

            IncludeUnitTestNamespaces();
            code.AppendLine();

            code.AppendLine("\t" + GetTestClassAttribute());
            code.AppendLine("\tpublic class DataAccessTestBase");
            code.AppendLine("\t{");
            code.AppendLine("\t\t" + GetTestInitializeAttribute());
            code.AppendLine("\t\tpublic void Initialize()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tEntityBase.ConnectionString = @\"" + Database.ConnectionString + "\";");
            code.AppendLine("\t\t}");
            code.AppendLine(@"

        protected static DataAccessRandomGenerator RandomGenerator = new DataAccessRandomGenerator();

        protected class DataAccessRandomGenerator
        {
            const string PWD_CHARSET = ""abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVXYZ1234567890"";

            public string GenerateString(int len)
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
            code.AppendLine("\t}");
            code.AppendLine();

            code.AppendLine("\t" + GetTestClassAttribute());
            code.AppendLine("\tpublic class EntityBaseTest : DataAccessTestBase");
            code.AppendLine("\t{");
            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void CreateCommandTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tAssert.IsNotNull(EntityBase.CreateCommand());");
            code.AppendLine("\t\t}");
            code.AppendLine();
            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void ConnectionIsOpenTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tvar expected = System.Data.ConnectionState.Open;");
            code.AppendLine("\t\t\tvar actual = EntityBase.Connection.State;");
            code.AppendLine("\t\t\tAssert.AreEqual(expected, actual);");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine();

            code.AppendLine("\t" + GetTestClassAttribute());
            code.AppendLine("\tpublic class DatabaseFileTest : DataAccessTestBase");
            code.AppendLine("\t{");
            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void CreateDatabaseTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tEntityBase.ConnectionString = @\"Data Source='" + new SqlCeConnectionStringBuilder(Database.ConnectionString).DataSource + "_\" + RandomGenerator.GenerateString(10) + \".sdf'\";");
            code.AppendLine("\t\t\tEntityBase.Connection.Dispose();");
            code.AppendLine("\t\t\tEntityBase.Connection = null;");
            code.AppendLine();
            code.AppendLine("\t\t\tvar actual = DatabaseFile.CreateDatabase();");
            code.AppendLine("\t\t\tAssert.AreNotEqual(0, actual);");
            code.AppendLine();
            code.AppendLine("\t\t\tEntityBase.ConnectionString = @\"" + Database.ConnectionString + "\";");
            code.AppendLine("\t\t\tEntityBase.Connection.Dispose();");
            code.AppendLine("\t\t\tEntityBase.Connection = null;");
            code.AppendLine("\t\t}");
            code.AppendLine("\t}");
            code.AppendLine();

            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t" + GetTestClassAttribute());
                code.AppendLine("\tpublic class " + table.ClassName + "DataAccessTest : DataAccessTestBase");
                code.AppendLine("\t{");

                GenerateCreateTest(table);
                GenerateCreateWithParametersTest(table);
                GenerateSelectAllTest(table);
                GenerateSelectAllWithTopTest(table);
                GenerateSelectByTest(table);
                GenerateSelectByWithTopTest(table);
                GenerateDelete(table);
                GenerateDeleteBy(table);
                GenerateDeleteAll(table);
                GenerateSaveChanges(table);
                GeneratePopulate(table);
                GenerateCount(table);

                code.AppendLine("\t}");
                code.AppendLine();
            }

            code.AppendLine("}");
            code.AppendLine();
        }

        private void GenerateCount(Table table)
        {
            Trace.WriteLine("Generating CountTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void CountTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tCreateTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = target.Count();");
            code.AppendLine("\t\t\tAssert.AreNotEqual(0, actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GeneratePopulate(Table table)
        {
            Trace.WriteLine("Generating PopulateTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void PopulateTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tPurgeTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = new System.Collections.Generic.List<" + table.ClassName + ">();");
            code.AppendLine("\t\t\tfor (int i = 0; i < 10; i++)");
            code.AppendLine("\t\t\t\tactual.Add(new " + table.ClassName);
            code.AppendLine("\t\t\t\t{");

            foreach (var column in table.Columns)
            {
                if (table.PrimaryKeyColumnName == column.Value.FieldName && column.Value.AutoIncrement.HasValue)
                    continue;
                if (column.Value.IsForeignKey)
                    continue;
                code.AppendFormat("\t\t\t\t\t{0} = {1},",
                                  column.Value.FieldName,
                                  column.Value.ManagedType.Equals(typeof(string))
                                      ? "RandomGenerator.GenerateString(" + column.Value.MaxLength + ")"
                                      : RandomGenerator.GenerateValue(column.Value.DatabaseType));
                code.AppendLine();
            }
            code.Remove(code.Length - 3, 2);
            code.AppendLine("\t\t\t\t});");
            code.AppendLine("\t\t\ttarget.Create(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GenerateSaveChanges(Table table)
        {
            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void UpdateTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = target.ToList();");
            code.AppendLine("\t\t\tvar item = actual[0];");
            code.AppendLine("\t\t\ttarget.Update(item);");
            code.AppendLine("\t\t}");
            code.AppendLine();

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void UpdateManyTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tPopulateTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = target.ToList();");
            code.AppendLine("\t\t\ttarget.Update(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GenerateDeleteAll(Table table)
        {
            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void PurgeTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\ttarget.Purge();");
            code.AppendLine("\t\t\tvar actual = target.ToList();");
            code.AppendLine("\t\t\tAssert.IsNull(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GenerateDeleteBy(Table table)
        {
            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                code.AppendLine("\t\t" + GetTestMethodAttribute());
                code.AppendLine("\t\tpublic void DeleteBy" + column.Value.FieldName + "Test()");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tPurgeTest();");
                code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
                code.AppendLine("\t\t\tvar actual = new " + table.ClassName);
                code.AppendLine("\t\t\t{");
                foreach (var col in table.Columns)
                {
                    if (table.PrimaryKeyColumnName == col.Value.Name && col.Value.AutoIncrement.HasValue)
                        continue;
                    if (col.Value.IsForeignKey)
                        continue;
                    code.AppendFormat("\t\t\t\t{0} = {1},",
                                      col.Value.FieldName,
                                      col.Value.ManagedType.Equals(typeof(string))
                                          ? "RandomGenerator.GenerateString(" + col.Value.MaxLength + ")"
                                          : RandomGenerator.GenerateValue(col.Value.DatabaseType));
                    code.AppendLine();
                }
                code.Remove(code.Length - 3, 2);
                code.AppendLine("\t\t\t};");
                code.AppendLine("\t\t\ttarget.Create(actual);");
                code.AppendLine("\t\t\ttarget.DeleteBy" + column.Value.FieldName + "(actual." + column.Value.FieldName + ");");
                code.AppendLine("\t\t}");
                code.AppendLine();
            }
        }

        private void GenerateDelete(Table table)
        {
            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void DeleteTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tPurgeTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = new " + table.ClassName);
            code.AppendLine("\t\t\t{");
            foreach (var column in table.Columns)
            {
                if (table.PrimaryKeyColumnName == column.Value.FieldName && column.Value.AutoIncrement.HasValue)
                    continue;
                if (column.Value.IsForeignKey)
                    continue;
                code.AppendFormat("\t\t\t\t{0} = {1},",
                                  column.Value.FieldName,
                                  column.Value.ManagedType.Equals(typeof(string))
                                      ? "RandomGenerator.GenerateString(" + column.Value.MaxLength + ")"
                                      : RandomGenerator.GenerateValue(column.Value.DatabaseType));
                code.AppendLine();
            }
            code.Remove(code.Length - 3, 2);
            code.AppendLine("\t\t\t};");
            code.AppendLine("\t\t\ttarget.Create(actual);");
            code.AppendLine("\t\t\ttarget.Delete(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void DeleteManyTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tPopulateTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = target.ToList();");
            code.AppendLine("\t\t\ttarget.Delete(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GenerateSelectByWithTopTest(Table table)
        {
            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                if (column.Value.IsForeignKey)
                    continue;

                Trace.WriteLine("Generating SelectBy" + column.Value.FieldName + "WithTopTest()");

                code.AppendLine("\t\t" + GetTestMethodAttribute());
                code.AppendLine("\t\tpublic void SelectBy" + column.Value.FieldName + "WithTopTest()");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tCreateTest();");
                code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
                code.AppendLine("\t\t\tvar record = target.ToList(1)[0];");
                code.AppendLine("\t\t\tvar actual = target.SelectBy" + column.Value.FieldName + "(record." + column.Value.FieldName + ", 10);");
                code.AppendLine();
                code.AppendLine("\t\t\tAssert.IsNotNull(actual);");
                code.AppendLine("\t\t\tCollectionAssert.AllItemsAreNotNull(actual);");
                code.AppendLine("\t\t}");
                code.AppendLine();
            }
        }

        private void GenerateSelectByTest(Table table)
        {
            foreach (var column in table.Columns)
            {
                if (string.Compare(column.Value.DatabaseType, "ntext", true) == 0 || string.Compare(column.Value.DatabaseType, "image", true) == 0)
                    continue;

                if (column.Value.IsForeignKey)
                    continue;

                Trace.WriteLine("Generating SelectBy" + column.Value.FieldName + "Test()");

                code.AppendLine("\t\t" + GetTestMethodAttribute());
                code.AppendLine("\t\tpublic void SelectBy" + column.Value.FieldName + "Test()");
                code.AppendLine("\t\t{");
                code.AppendLine("\t\t\tCreateTest();");
                code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
                code.AppendLine("\t\t\tvar record = target.ToList(1)[0];");
                code.AppendLine("\t\t\tvar actual = target.SelectBy" + column.Value.FieldName + "(record." + column.Value.FieldName + ");");
                code.AppendLine();
                code.AppendLine("\t\t\tAssert.IsNotNull(actual);");
                code.AppendLine("\t\t\tCollectionAssert.AllItemsAreNotNull(actual);");
                code.AppendLine("\t\t}");
                code.AppendLine();
            }
        }

        private void GenerateSelectAllWithTopTest(Table table)
        {
            Trace.WriteLine("Generating ToListWithTopTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void ToListWithTopTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tCreateTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = target.ToList(10);");
            code.AppendLine("\t\t\tAssert.IsNotNull(actual);");
            code.AppendLine("\t\t\tCollectionAssert.AllItemsAreNotNull(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();

            Trace.WriteLine("Generating ToArrayWithTopTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void ToArrayWithTopTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tCreateTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = target.ToArray(10);");
            code.AppendLine("\t\t\tAssert.IsNotNull(actual);");
            code.AppendLine("\t\t\tCollectionAssert.AllItemsAreNotNull(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GenerateSelectAllTest(Table table)
        {
            Trace.WriteLine("Generating ToListTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void ToListTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tCreateTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = target.ToList();");
            code.AppendLine("\t\t\tAssert.IsNotNull(actual);");
            code.AppendLine("\t\t\tCollectionAssert.AllItemsAreNotNull(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();

            Trace.WriteLine("Generating ToArrayTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void ToArrayTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tCreateTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = target.ToArray();");
            code.AppendLine("\t\t\tAssert.IsNotNull(actual);");
            code.AppendLine("\t\t\tCollectionAssert.AllItemsAreNotNull(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GenerateCreateWithParametersTest(Table table)
        {
            Trace.WriteLine("Generating CreateWithParametersTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void CreateWithParametersTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tPurgeTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.Append("\t\t\ttarget.Create(");

            foreach (var column in table.Columns)
            {
                if (table.PrimaryKeyColumnName == column.Value.FieldName && column.Value.AutoIncrement.HasValue)
                    continue;
                if (column.Value.IsForeignKey && column.Value.AllowsNull)
                    code.Append("null");
                else
                    code.Append(
                        column.Value.ManagedType.Equals(typeof(string))
                           ? "RandomGenerator.GenerateString(" + column.Value.MaxLength + ")"
                           : RandomGenerator.GenerateValue(column.Value.DatabaseType));
                code.Append(", ");
            }

            code.Remove(code.Length - 2, 2);
            code.Append(");");
            code.AppendLine();

            code.AppendLine("\t\t}");
            code.AppendLine();
        }

        private void GenerateCreateTest(Table table)
        {
            Trace.WriteLine("Generating CreateTest()");

            code.AppendLine("\t\t" + GetTestMethodAttribute());
            code.AppendLine("\t\tpublic void CreateTest()");
            code.AppendLine("\t\t{");
            code.AppendLine("\t\t\tPurgeTest();");
            code.AppendLine("\t\t\tI" + table.ClassName + "Repository target = new " + table.ClassName + "Repository();");
            code.AppendLine("\t\t\tvar actual = new " + table.ClassName);
            code.AppendLine("\t\t\t{");

            foreach (var column in table.Columns)
            {
                if (table.PrimaryKeyColumnName == column.Value.FieldName && column.Value.AutoIncrement.HasValue)
                    continue;
                if (column.Value.IsForeignKey && column.Value.AllowsNull)
                    continue;
                code.AppendFormat("\t\t\t\t{0} = {1},",
                                  column.Value.FieldName,
                                  column.Value.ManagedType.Equals(typeof(string))
                                    ? "RandomGenerator.GenerateString(" + column.Value.MaxLength + ")"
                                    : RandomGenerator.GenerateValue(column.Value.DatabaseType));
                code.AppendLine();
            }
            code.Remove(code.Length - 3, 2);
            code.AppendLine("\t\t\t};");
            code.AppendLine("\t\t\ttarget.Create(actual);");
            code.AppendLine("\t\t}");
            code.AppendLine();
        }
    }

    public static class RandomGenerator
    {
        public static object GenerateValue(string databaseType)
        {
            switch (databaseType.ToLower())
            {
                case "tinyint":
                    return "(System.Byte)new System.Random().Next(0, 255)";

                case "smallint":
                    return "(short)new System.Random().Next(1, 1000)";

                case "bigint":
                    return "(long)new System.Random().Next(1, 1000)";

                case "int":
                    return "new System.Random().Next(1, 1000)";

                case "binary":
                case "varbinary":
                case "image":
                case "rowversion":
                    return "new System.Byte[] { (System.Byte)new System.Random().Next(0, 255), (System.Byte)new System.Random().Next(0, 255) }";

                case "bit":
                    return "System.Convert.ToBoolean(new System.Random().Next(0, 1))";

                case "datetime":
                    return "System.DateTime.Now";

                case "float":
                case "real":
                    return "System.Convert.ToSingle(new System.Random().Next(1,1000))";

                case "double":
                    return "new System.Random().NextDouble()";

                case "money":
                case "smallmoney":
                case "numeric":
                case "decimal":
                    return "System.Convert.ToDecimal(new System.Random().Next(1,1000))";

                case "uniqueidentifier":
                    return "System.Guid.NewGuid()";

                default:
                    return null;
            }
        }
    }

    public class MsTestUnitTestCodeGenerator : CSharpUnitTestCodeGenerator
    {
        public MsTestUnitTestCodeGenerator(SqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        protected override void IncludeUnitTestNamespaces()
        {
            code.AppendLine("\tusing Microsoft.VisualStudio.TestTools.UnitTesting;");
        }

        protected override string GetTestClassAttribute()
        {
            return "[TestClass]";
        }

        protected override string GetTestMethodAttribute()
        {
            return "[TestMethod]";
        }

        protected override string GetTestInitializeAttribute()
        {
            return "[TestInitialize]";
        }
    }

    public class NUnitTestCodeGenerator : CSharpUnitTestCodeGenerator
    {
        public NUnitTestCodeGenerator(SqlCeDatabase tableDetails)
            : base(tableDetails)
        {
        }

        protected override void IncludeUnitTestNamespaces()
        {
            code.AppendLine("\tusing NUnit.Framework;");
        }

        protected override string GetTestClassAttribute()
        {
            return "[TestFixture]";
        }

        protected override string GetTestMethodAttribute()
        {
            return "[Test]";
        }

        protected override string GetTestInitializeAttribute()
        {
            return "[SetUp]";
        }
    }
}
