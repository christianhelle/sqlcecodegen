using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            code.AppendLine("\nnamespace " + Database.Namespace);
            code.AppendLine("{");

            foreach (var table in Database.Tables)
            {
                code.AppendLine("\t[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]");
                code.AppendLine("\tpublic class " + table.TableName + "EntityTest");
                code.AppendLine("\t{");

                foreach (var column in table.Columns)
                {
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

                code.AppendLine("\t}");
                code.AppendLine();
            }

            code.AppendLine("}");
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
