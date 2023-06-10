using ImGuiNET;

namespace StellaronLite.Impl;

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
