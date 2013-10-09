namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore
{
    public class DataAccessLayerGeneratorOptions
    {
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
            GenerateSelectByTwoColumns = false;
            GenerateSelectByThreeColumns = false;
            GenerateCreate = true;
            GenerateCreateIgnoringPrimaryKey = true;
            GenerateCreateUsingAllColumns = true;
            GeneratePopulate = true;
            GenerateDelete = true;
            GenerateDeleteBy = true;
            GenerateDeleteAll = true;
            GenerateSaveChanges = true;
            GenerateUpdate = true;
            GenerateCount = true;
        }

        public bool ThrowExceptions { get; set; }
        public bool DebuggerOutput { get; set; }
        public bool PerformanceMeasurementOutput { get; set; }
        public bool GenerateXmlDocumentation { get; set; }

        public bool GenerateSelectAll { get; set; }
        public bool GenerateSelectAllWithTop { get; set; }
        public bool GenerateSelectBy { get; set; }
        public bool GenerateSelectByWithTop { get; set; }
        public bool GenerateSelectByTwoColumns { get; set; }
        public bool GenerateSelectByThreeColumns { get; set; }
        public bool GenerateCreate { get; set; }
        public bool GenerateCreateIgnoringPrimaryKey { get; set; }
        public bool GenerateCreateUsingAllColumns { get; set; }
        public bool GeneratePopulate { get; set; }
        public bool GenerateDelete { get; set; }
        public bool GenerateDeleteBy { get; set; }
        public bool GenerateDeleteAll { get; set; }
        public bool GenerateSaveChanges { get; set; }

        public bool GenerateUpdate { get; set; }
        public bool GenerateCount { get; set; }
    }
}