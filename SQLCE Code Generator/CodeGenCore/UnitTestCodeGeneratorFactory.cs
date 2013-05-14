using System;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class UnitTestCodeGeneratorFactory
    {
        private readonly ISqlCeDatabase database;

        public UnitTestCodeGeneratorFactory(ISqlCeDatabase database)
        {
            this.database = database;
        }

        public CodeGenerator Create(string testfx, string target = null)
        {
            return Create(database, testfx, target);
        }

        public static CodeGenerator Create(ISqlCeDatabase database, string testfx, string target = null)
        {
            if (!string.IsNullOrEmpty(target) && target == "Mango")
                throw new NotSupportedException("Unit Test Framework not supported"); //return new CSharpMangoMockDataAccessLayerCodeGenerator(database);

            switch (testfx.ToLower())
            {
                case "mstest":
                    return new MSTestUnitTestCodeGenerator(database);
                case "nunit":
                    return new NUnitTestCodeGenerator(database);
                case "xunit":
                    return new XUnitTestCodeGenerator(database);
            }

            throw new NotSupportedException("Unit Test Framework not supported");
        }

        public CodeGenerator Create()
        {
            return Create(database);
        }

        public static CodeGenerator Create(ISqlCeDatabase database)
        {
            return new MSTestUnitTestCodeGenerator(database);
        }
    }
}
