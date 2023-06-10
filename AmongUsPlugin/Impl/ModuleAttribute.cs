using System;

namespace AmongUsPlugin.Impl;

[AttributeUsage(AttributeTargets.Class)]
public class ModuleAttribute : Attribute
{
    public string Name;
    public string? Description;
    public ModuleCategory Category;

    public ModuleAttribute(string name, ModuleCategory cat, string? desc = null) :
        base()
    {
        Name = name;
        Description = desc;
        Category = cat;
    }
}
