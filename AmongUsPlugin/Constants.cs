using System;
using AmongUsPlugin.Impl;

namespace AmongUsPlugin;

public class Constants
{
    public static readonly string STELLARON_PROJECT_NAME = "Among Us Plugin";
    public static readonly string STELLARON_COMMAND_NAME = "/amongus";
    public static readonly string STELLARON_COMMAND_DESCRIPTION = "Opens the configuration interface.";
    public static readonly Type STELLARON_MODULE = typeof(AmongUsModule);
}
