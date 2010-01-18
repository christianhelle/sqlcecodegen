using System.Text;
using System.IO;
using System.Globalization;

namespace CodeGenCore
{
    public abstract class CodeGenerator
    {
        protected readonly StringBuilder code;

        protected CodeGenerator(Database database)
        {
            Database = database;
            code = new StringBuilder();
        }

        public Database Database { get; set; }
        public abstract void GenerateEntities();
        public abstract void GenerateDataAccessLayer();

        public string GetCode()
        {
            using (var writer = new StringWriter(code, CultureInfo.InvariantCulture))
                return writer.ToString();
        }
    }
}
