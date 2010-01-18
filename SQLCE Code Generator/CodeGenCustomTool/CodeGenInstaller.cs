using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.Collections;

namespace CodeGenCustomTool
{
    public class CodeGenInstaller : Installer
    {
        public override void Install(IDictionary savedState)
        {
            try
            {                
                base.Install(savedState);
            }
            catch (Exception e)
            {                
                Rollback(savedState);
            }
        }

        public override void Commit(IDictionary savedState)
        {
            try
            {
                base.Commit(savedState);
            }
            catch (Exception e)
            {
                Rollback(savedState);
            }
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            try
            {
            }
            catch (Exception e)
            {
                throw;
            }

            base.Uninstall(savedState);
        }
    }
}
