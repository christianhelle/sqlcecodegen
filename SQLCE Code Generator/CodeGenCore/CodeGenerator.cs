using System.Text;
using System.IO;
using System.Globalization;
using System.Collections.Generic;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public abstract class CodeGenerator
    {
        protected readonly StringBuilder code;

        protected CodeGenerator(SqlCeDatabase database)
        {
            Database = database;
            code = new StringBuilder();
            code.AppendLine();
        }

        public SqlCeDatabase Database { get; set; }
        public abstract void WriteHeaderInformation();
        public abstract void GenerateEntities();
        public abstract void GenerateEntities(EntityGeneratorOptions options);
        public abstract void GenerateDataAccessLayer();
        public abstract void GenerateDataAccessLayer(DataAccessLayerGeneratorOptions options);

        public string GetCode()
        {
            using (var writer = new StringWriter(code, CultureInfo.InvariantCulture))
                return writer.ToString();
        }

        public void ClearCode()
        {
            code.Remove(0, code.Length - 1);
        }
    }
}
