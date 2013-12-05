﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleUninstaller))]
    public class ModuleUninstaller : IModuleUninstaller
    {
        #region .ctor
        IAssemblyReferences _assemblyReferences;
        public ModuleUninstaller(IAssemblyReferences assemblyReferences)
        {
            this._assemblyReferences = assemblyReferences;
        }
        #endregion

        #region RunEvent
        public void RunEvent(string moduleName, ControllerContext controllerContext)
        {
            var moduleAction = ResolveModuleAction(moduleName);

            moduleAction.OnInstalling(controllerContext);
        }
        private IModuleEvents ResolveModuleAction(string moduleName)
        {
            return Kooboo.CMS.Common.Runtime.EngineContext.Current.TryResolve<IModuleEvents>(moduleName);
        }
        #endregion

        #region RemoveAssemblies
        public void RemoveAssemblies(string moduleName)
        {
            var binPath = Settings.BinDirectory;
            foreach (var item in GetAssemblyFiles(moduleName))
            {
                var binFile = Path.Combine(binPath, Path.GetFileName(item));
                var removable = _assemblyReferences.RemoveReference(binFile, moduleName);
                if (removable)
                {
                    if (File.Exists(binFile))
                    {
                        File.Delete(binFile);
                    }
                }
            }
        }

        #region GetAssemblyFiles
        private IEnumerable<string> GetAssemblyFiles(string moduleName)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            ModuleItemPath moduleBinPath = new ModuleItemPath(moduleName, "Bin");
            if (Directory.Exists(moduleBinPath.PhysicalPath))
            {
                return Directory.EnumerateFiles(moduleBinPath.PhysicalPath);
            }
            else
            {
                return new string[0];
            }
        }
        #endregion
        #endregion

        #region DeleteModuleArea

        public void DeleteModuleArea(string moduleName)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            Kooboo.IO.IOUtility.DeleteDirectory(modulePath.PhysicalPath, true);
        }
        #endregion
    }
}
