namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class DataAccessLayerGeneratorOptions
    {
        public bool ThrowExceptions { get; set; }
        public bool DebuggerOutput { get; set; }
        public bool PerformanceMeasurementOutput { get; set; }
        public bool GenerateXmlDocumentation { get; set; }

        public bool GenerateSelectAll { get; set; }
        public bool GenerateSelectAllWithTop { get; set; }
        public bool GenerateSelectBy { get; set; }
        public bool GenerateSelectByWithTop { get; set; }
        public bool GenerateCreateIgnoringPrimaryKey { get; set; }
        public bool GenerateCreateUsingAllColumns { get; set; }
        public bool GenerateDelete { get; set; }
        public bool GenerateDeleteBy { get; set; }
        public bool GenerateDeleteAll { get; set; }
        public bool GenerateSaveChanges { get; set; }

        public DataAccessLayerGeneratorOptions()
        {
            ThrowExceptions = false;
            DebuggerOutput = false;
            PerformanceMeasurementOutput = false;
            GenerateXmlDocumentation = false;

            GenerateSelectAll = true;
            GenerateSelectAllWithTop = true;
            GenerateSelectBy = true;
            GenerateSelectByWithTop = true;
            GenerateCreateIgnoringPrimaryKey = true;
            GenerateCreateUsingAllColumns = true;
            GenerateDelete = true;
            GenerateDeleteBy = true;
            GenerateDeleteAll = true;
            GenerateSaveChanges = true;
        }
    }

    public class EntityGeneratorOptions
    {
        public bool ThrowExceptions { get; set; }
        public bool AutoPropertiesOnly { get; set; }
        public bool DebuggerOutput { get; set; }

        public EntityGeneratorOptions()
        {
            ThrowExceptions = true;
            AutoPropertiesOnly = false;
            DebuggerOutput = false;
        }
    }
}
