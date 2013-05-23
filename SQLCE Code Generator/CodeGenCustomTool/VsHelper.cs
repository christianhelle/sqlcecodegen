using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    public static class VsHelper
    {
        public static IVsHierarchy GetCurrentHierarchy(IServiceProvider provider)
        {
            var vs = (DTE) provider.GetService(typeof (DTE));
            if (vs == null) throw new InvalidOperationException("DTE not found.");
            return ToHierarchy(vs.SelectedItems.Item(1).ProjectItem.ContainingProject);
        }

        public static IVsHierarchy ToHierarchy(Project project)
        {
            if (project == null) throw new ArgumentNullException("project");
            string projectGuid = null;

            using (var projectReader = XmlReader.Create(project.FileName))
            {
                projectReader.MoveToContent();
                if (projectReader.NameTable != null)
                {
                    object nodeName = projectReader.NameTable.Add("ProjectGuid");
                    while (projectReader.Read())
                    {
                        if (Equals(projectReader.LocalName, nodeName))
                        {
                            projectGuid = projectReader.ReadElementContentAsString();
                            break;
                        }
                    }
                }
            }
            IServiceProvider serviceProvider =
                new ServiceProvider(project.DTE as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            return projectGuid != null ? VsShellUtilities.GetHierarchy(serviceProvider, new Guid(projectGuid)) : null;
        }

        public static IVsProject ToVsProject(Project project)
        {
            if (project == null) throw new ArgumentNullException("project");
            var vsProject = ToHierarchy(project) as IVsProject;
            if (vsProject == null)
            {
                throw new ArgumentException("Project is not a VS project.");
            }
            return vsProject;
        }

        public static Project ToDteProject(IVsHierarchy hierarchy)
        {
            if (hierarchy == null) throw new ArgumentNullException("hierarchy");
            object prjObject;
            if (hierarchy.GetProperty(0xfffffffe, -2027, out prjObject) >= 0)
                return (Project) prjObject;
            throw new ArgumentException("Hierarchy is not a project.");
        }

        public static Project ToDteProject(IVsProject project)
        {
            if (project == null) throw new ArgumentNullException("project");
            return ToDteProject(project as IVsHierarchy);
        }


        public static ProjectItem FindProjectItem(Project project, string file)
        {
            return FindProjectItem(project.ProjectItems, file);
        }

        public static ProjectItem FindProjectItem(ProjectItems items, string file)
        {
            var atom = file.Substring(0, file.IndexOf("\\") + 1);
            foreach (ProjectItem item in items)
            {
                //if ( item
                //if (item.ProjectItems.Count > 0)
                if (atom.StartsWith(item.Name))
                {
                    // then step in
                    var ritem = FindProjectItem(item.ProjectItems, file.Substring(file.IndexOf("\\") + 1));
                    if (ritem != null)
                        return ritem;
                }
                if (Regex.IsMatch(item.Name, file))
                {
                    return item;
                }
                if (item.ProjectItems.Count > 0)
                {
                    var ritem = FindProjectItem(item.ProjectItems, file.Substring(file.IndexOf("\\") + 1));
                    if (ritem != null)
                        return ritem;
                }
            }
            return null;
        }

        public static List<ProjectItem> FindProjectItems(ProjectItems items, string match)
        {
            var values = new List<ProjectItem>();

            foreach (ProjectItem item in items)
            {
                if (Regex.IsMatch(item.Name, match))
                {
                    values.Add(item);
                }
                if (item.ProjectItems.Count > 0)
                {
                    values.AddRange(FindProjectItems(item.ProjectItems, match));
                }
            }
            return values;
        }
    }
}