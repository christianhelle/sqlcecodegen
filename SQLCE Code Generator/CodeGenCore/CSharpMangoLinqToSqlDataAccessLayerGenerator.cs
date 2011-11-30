using System;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class CSharpMangoLinqToSqlDataAccessLayerGenerator : DataAccessLayerGenerator
    {
        public CSharpMangoLinqToSqlDataAccessLayerGenerator(StringBuilder code, Table table) : base(code, table)
        {
        }

        #region Overrides of DataAccessLayerGenerator

        public override void GenerateSelectAll()
        {
            Code.AppendLine("\t\t#region SELECT *");
            Code.AppendLine();
            GenerateXmlDoc(2, "Retrieves all items as a generic collection");
            Code.AppendLine("\t\tpublic System.Collections.Generic.List<" + Table.ClassName + "> ToList()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tusing (var context = EntityDataContext())");
            Code.AppendLine("\t\t\t\treturn new context." + Table.ClassName + ".ToList();");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\tpublic " + Table.ClassName + "[] ToArray()");
            Code.AppendLine("\t\t{");
            Code.AppendLine("\t\t\tvar list = ToList();");
            Code.AppendLine("\t\t\treturn list != null ? list.ToArray() : null;");
            Code.AppendLine("\t\t}");
            Code.AppendLine();
            Code.AppendLine("\t\t#endregion");
            Code.AppendLine();
        }

        public override void GenerateSelectBy()
        {
        }

        public override void GenerateSelectWithTop()
        {
        }

        public override void GenerateSelectByWithTop()
        {
        }

        public override void GenerateCreateIgnoringPrimaryKey()
        {
        }

        public override void GenerateCreateUsingAllColumns()
        {
        }

        public override void GenerateDelete()
        {
        }

        public override void GenerateDeleteBy()
        {
        }

        public override void GenerateDeleteAll()
        {
        }

        public override void GenerateUpdate()
        {
        }

        public override void GeneratePopulate()
        {
        }

        public override void GenerateCreate()
        {
        }

        public override void GenerateCount()
        {
        }

        #endregion
    }
}
