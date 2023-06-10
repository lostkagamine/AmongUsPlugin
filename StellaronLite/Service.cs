using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.Gui;
using Dalamud.IoC;

namespace StellaronLite;

public class Service
{
    [PluginService]
    public static ChatGui ChatGui { get; set; }
    
    [PluginService]
    public static SigScanner SigScanner { get; set; }
    
    [PluginService]
    public static ClientState ClientState { get; set; }
    
    [PluginService]
    public static Framework Framework { get; set; }
}
