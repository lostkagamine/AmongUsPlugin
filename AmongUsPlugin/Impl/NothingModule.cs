using ImGuiNET;

namespace AmongUsPlugin.Impl;

[Module("Nothing", ModuleCategory.Utility, "Does nothing.")]
public class NothingModule : IModule
{
    public void OnEnable()
    {
    }

    public void OnDisable()
    {
    }

    public void DrawUi()
    {
        ImGui.Text("Hi :)");
    }
}
