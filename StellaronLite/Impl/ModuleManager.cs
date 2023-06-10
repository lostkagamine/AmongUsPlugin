using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Logging;

namespace StellaronLite.Impl;

public class ModuleManager
{
    public class ModuleInformation
    {
        public string Name;
        public string? Description;
        public ModuleCategory Category;

        public bool Enable;
        
        public IModule Object;
    }
    
    private static ModuleManager? _instance;
    public static ModuleManager Instance = _instance ??= new();

    public static ModuleInformation Module;

    public static void ScanModules()
    {
        if (!Constants.STELLARON_MODULE.IsAssignableTo(typeof(IModule)))
            throw new Exception("Huh?");
        
        var attr = Constants.STELLARON_MODULE.GetCustomAttribute<ModuleAttribute>();
        
        if (attr == null)
            throw new Exception("Whar?");

        var obj = Activator.CreateInstance(Constants.STELLARON_MODULE);

        if (obj == null)
            throw new Exception("Qhat?");

        var info = new ModuleInformation()
        {
            Name = attr.Name,
            Description = attr.Description,
            Category = attr.Category,
            Enable = true,
            Object = (IModule)obj!
        };

        Module = info;
    }
}
