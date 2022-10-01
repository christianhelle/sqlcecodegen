using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Design.Serialization;

namespace ChristianRHelle.VSPackage
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidVSPackagePkgString)]
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\10.0")]
    [ProvideObject(typeof(SQLCECodeGenerator))]
    [ProvideObject(typeof(SQLCECodeGeneratorMultiFile))]
    [ProvideObject(typeof(SQLCEMangoCodeGenerator))]
    [ProvideObject(typeof(SQLCEMangoCodeGeneratorMultiFile))]
    [ProvideObject(typeof(SQLCEMSTestCodeGenerator))]
    [ProvideObject(typeof(SQLCEMSTestCodeGeneratorMultiFile))]
    [ProvideObject(typeof(SQLCENUnitCodeGenerator))]
    [ProvideObject(typeof(SQLCENUnitCodeGeneratorMultiFile))]
    [ProvideObject(typeof(SQLCESqlMetalMangoCodeGenerator))]
    [ProvideObject(typeof(SQLCEXUnitCodeGenerator))]
    [ProvideObject(typeof(SQLCEXUnitCodeGeneratorMultiFile))]
    [ProvideGenerator(typeof(SQLCECodeGenerator), "SQLCECodeGenerator", "SQLCE Data Access Layer Single File Code Generator", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCECodeGeneratorMultiFile), "SQLCECodeGeneratorMultiFile", "SQLCE Data Access Layer Single File Code Generator", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCEMangoCodeGenerator), "SQLCEMangoCodeGenerator", "Windows Phone SQLCE Data Access Layer Single File Code Generator", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCEMangoCodeGeneratorMultiFile), "SQLCEMangoCodeGeneratorMultiFile", "Windows Phone SQLCE Data Access Layer Code Generator", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCEMSTestCodeGenerator), "SQLCEMSTestCodeGenerator", "SQLCE Data Access Layer Unit Test Single Code File Generator (MSTest)", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCEMSTestCodeGeneratorMultiFile), "SQLCEMSTestCodeGeneratorMultiFile", "SQLCE Data Access Layer Unit Test Single Code File Generator (MSTest)", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCENUnitCodeGenerator), "SQLCENUnitCodeGenerator", "SQLCE Data Access Layer Unit Test Single Code File Generator (NUnit)", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCENUnitCodeGeneratorMultiFile), "SQLCENUnitCodeGeneratorMultiFile", "SQLCE Data Access Layer Unit Test Single Code File Generator (NUnit)", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCESqlMetalMangoCodeGenerator), "SQLCESqlMetalMangoCodeGenerator", "Windows Phone SQLCE Data Access Layer Single File Code Generator using SqlMetal.exe", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCEXUnitCodeGenerator), "SQLCEXUnitCodeGenerator", "SQLCE Data Access Layer Unit Test Single Code File Generator (XUnit)", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    [ProvideGenerator(typeof(SQLCEXUnitCodeGeneratorMultiFile), "SQLCEXUnitCodeGeneratorMultiFile", "SQLCE Data Access Layer Unit Test Single Code File Generator (XUnit)", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", true)]
    public sealed class VisualStudio2010Package : Package
    {
        public VisualStudio2010Package()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", ToString()));
        }

        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", ToString()));
            base.Initialize();
        }
    }
}