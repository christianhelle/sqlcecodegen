namespace CodeGenCore
{
    public class CodeGeneratorFactory
    {
        private readonly Database database;

        public CodeGeneratorFactory(Database database)
        {
            this.database = database;
        }

        public CodeGenerator Create(string language)
        {
            if (string.Compare(language, "C#", true) == 0 || string.Compare(language, "CSharp", true) == 0)
                return new CSharpCodeGenerator(database);
            if (string.Compare(language, "VB.NET", true) == 0 ||
                string.Compare(language, "Visual Basic", true) == 0 ||
                string.Compare(language, "VisualBasic", true) == 0 ||
                string.Compare(language, "VB", true) == 0)
                return new VisualBasicCodeGenerator(database);
            return Create();
        }

        public static CodeGenerator Create(Database database, string language)
        {
            if (string.Compare(language, "C#", true) == 0 || string.Compare(language, "CSharp", true) == 0)
                return new CSharpCodeGenerator(database);
            if (string.Compare(language, "VB.NET", true) == 0 ||
                string.Compare(language, "Visual Basic", true) == 0 ||
                string.Compare(language, "VisualBasic", true) == 0 ||
                string.Compare(language, "VB", true) == 0)
                return new VisualBasicCodeGenerator(database);
            return Create(database);
        }

        public CodeGenerator Create()
        {
            return new CSharpCodeGenerator(database);
        }

        public static CodeGenerator Create(Database database)
        {
            return new CSharpCodeGenerator(database);
        }
    }
}