using System.Collections.Generic;
using System.Text;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public abstract class DataAccessLayerGenerator
    {
        protected StringBuilder code;
        protected Table table;

        protected DataAccessLayerGenerator(StringBuilder code, Table table)
        {
            this.code = code;
            this.table = table;
        }

        public string GetCode()
        {
            return code.ToString();
        }

        public abstract void GenerateSelectAll();
        public abstract void GenerateSelectBy();
        public abstract void GenerateSelectWithTop();
        public abstract void GenerateSelectByWithTop();
        public abstract void GenerateCreateIgnoringPrimaryKey();
        public abstract void GenerateCreateUsingAllColumns();
        public abstract void GenerateDelete();
        public abstract void GenerateDeleteBy();
        public abstract void GenerateDeleteAll();
        public abstract void GenerateSaveChanges();
        public abstract void GeneratePopulate();
        public abstract void GenerateCreate();
        public abstract void GenerateCount();

        protected void GenerateXmlDoc(int tabPrefixCount, string summary, params KeyValuePair<string, string>[] parameters)
        {
            for (int i = 0; i < tabPrefixCount; i++)
                code.Append("\t");
            code.AppendLine("/// <summary>");

            for (int i = 0; i < tabPrefixCount; i++)
                code.Append("\t");
            code.AppendLine("/// " + summary);

            for (int i = 0; i < tabPrefixCount; i++)
                code.Append("\t");
            code.AppendLine("/// </summary>");

            foreach (var parameter in parameters)
            {
                for (int i = 0; i < tabPrefixCount; i++)
                    code.Append("\t");
                code.AppendFormat("<param name=\"{0}\">{1}</param>", parameter.Key, parameter.Value);
                code.AppendLine();
            }
        }
    }
}
