using System;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Logging;
using StellaronLite.Impl;
using StellaronLite.Util;
using StellaronLite.Windows;

namespace StellaronLite
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => Constants.STELLARON_PROJECT_NAME;

        public static DalamudPluginInterface PluginInterface { get; private set; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new(Constants.STELLARON_PROJECT_NAME);

        private MainWindow MainWindow { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            pluginInterface.Create<Service>();
            
            PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(PluginInterface);

            MainWindow = new MainWindow(this);

            WindowSystem.AddWindow(MainWindow);

            this.CommandManager.AddHandler(Constants.STELLARON_COMMAND_NAME, new CommandInfo(OnCommand)
            {
                HelpMessage = Constants.STELLARON_COMMAND_DESCRIPTION
            });

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            
            ModuleManager.ScanModules();
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            MainWindow.Dispose();
            AudioHelper.Teardown();
            
            this.CommandManager.RemoveHandler(Constants.STELLARON_COMMAND_NAME);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            MainWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            MainWindow.IsOpen = true;
        }
    }
}
