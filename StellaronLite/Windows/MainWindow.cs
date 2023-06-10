using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using ImGuiNET;
using ImGuiScene;
using StellaronLite.Impl;

namespace StellaronLite.Windows;

public class MainWindow : Window, IDisposable
{
    private static Vector4 GRAY = new Vector4(117.0f / 255.0f, 117.0f / 255.0f, 117.0f / 255.0f, 1.0f);
    
    private Plugin Plugin;

    private ModuleManager.ModuleInformation? Selected = null;

    public MainWindow(Plugin plugin) : base(
        Constants.STELLARON_PROJECT_NAME, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.Plugin = plugin;
    }

    public void Dispose()
    {
    }

    void DrawAbout()
    {
        ImGui.Text("Powered by Stellaron Lite - FINAL FANTASY XIV general-purpose development plugin");
        ImGui.Text("(c) 2023 Stelle Nightshade/Nightshade System");
        ImGui.Text("https://github.com/ry00001/StellaronLite");
    }

    void DrawModules()
    {
        // nop
    }

    void DrawMainPanel()
    {
        ModuleManager.Module.Object.DrawUi();
    }

    public override void Draw()
    {
        if (ImGui.BeginTabBar(Constants.STELLARON_PROJECT_NAME))
        {
            if (ImGui.BeginTabItem("Module"))
            {
                DrawMainPanel();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("About"))
            {
                DrawAbout();
                ImGui.EndTabItem();
            }
        }
    }
}
