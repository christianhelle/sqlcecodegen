#region License
// The MIT License (MIT)
// 
// Copyright (c) 2009 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    public abstract class MultipleFileGenerator : CSharpFileGenerator
    {
        protected void AddOutputToProject(string wszInputFilePath, Dictionary<string, StringBuilder> files,
                                          StringBuilder header)
        {
            int iFound;
            uint itemId;
            ProjectItem item;
            var pdwPriority = new VSDOCUMENTPRIORITY[1];

            Project project = null;
            var dte = (DTE) Package.GetGlobalService(typeof (DTE));
            var ary = (Array) dte.ActiveSolutionProjects;
            if (ary.Length > 0)
                project = (Project) ary.GetValue(0);
            var vsProject = VsHelper.ToVsProject(project);

            vsProject.IsDocumentInProject(wszInputFilePath, out iFound, pdwPriority, out itemId);

            if (iFound != 0 && itemId != 0)
            {
                IServiceProvider oleSp;
                vsProject.GetItemContext(itemId, out oleSp);
                if (oleSp != null)
                {
                    var sp = new ServiceProvider(oleSp);
                    item = sp.GetService(typeof (ProjectItem)) as ProjectItem;
                }
                else
                    throw new ApplicationException("Unable to retrieve Visual Studio ProjectItem");
            }
            else
                throw new ApplicationException("Unable to retrieve Visual Studio ProjectItem");

            foreach (var codeFile in files)
            {
                var path = wszInputFilePath.Substring(0, wszInputFilePath.LastIndexOf(Path.DirectorySeparatorChar));
                var strFile = Path.Combine(path, codeFile.Key);

                using (var stream = new StreamWriter(strFile, false))
                {
                    stream.WriteLine(header);
                    stream.WriteLine(codeFile.Value);
                }

                if (item != null)
                    item.ProjectItems.AddFromFile(strFile);
            }

            if (item == null) return;
            foreach (ProjectItem childItem in item.ProjectItems)
                if (!(childItem.Name.EndsWith(GetDefaultExtension()) || files.ContainsKey(childItem.Name)))
                    childItem.Delete();
        }
    }
}