using System;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using AmongUsPlugin.Util;
using AmongUsPlugin.Util.Audio;

namespace AmongUsPlugin.Impl;

[Module("Among Us", ModuleCategory.Funny, "Plays the amogus sound whenever the story textbox says 'among us'")]
public unsafe class AmongUsModule : IModule
{
    private CachedSound amogus;
    
    public delegate AtkStage* DGetAtkStageSingleton();
    public static DGetAtkStageSingleton GetAtkStageSingleton;

    public AtkUnitBase* TalkUnit;
    public AtkTextNode* TextboxTextNode;

    private bool gotNode;
    private string lastText = "";
    private string triggerPhrase = "among us";

    private bool initedAtk;
    
    public AmongUsModule()
    {
        amogus = AudioHelper.Instance.CacheFromData("amogus.wav");
    }

    public void InitAtk()
    {
        var getSingletonAddr = Service.SigScanner.ScanText(
            "E8 ?? ?? ?? ?? 41 B8 01 00 00 00 48 8D 15 ?? ?? ?? ?? 48 8B 48 20 E8 ?? ?? ?? ?? 48 8B CF");
        GetAtkStageSingleton = Marshal.GetDelegateForFunctionPointer<DGetAtkStageSingleton>(getSingletonAddr);

        var stage = GetAtkStageSingleton();
        var depthLayerEight = &stage->RaptureAtkUnitManager->AtkUnitManager.DepthLayerEightList;
        
        PluginLog.Debug("amogus: got atk stage singleton ptr and we didnt crash yet");

        for (var i = 0; i < depthLayerEight->Count; i++)
        {
            var unit = &depthLayerEight->AtkUnitEntries[i];

            var name = Marshal.PtrToStringAnsi(new IntPtr(unit->Name));
            if (name is "Talk")
            {
                PluginLog.Debug($"amogus: found story textbox sig @ {(IntPtr)unit:X}");
                TalkUnit = unit;
            }
        }

        var uld = TalkUnit->UldManager;
        for (int i = 0; i < uld.NodeListCount; i++)
        {
            var node = uld.NodeList[i];
            if (node->NodeID == 3)
            {
                // enable real-time movement of UI node
                node->Flags_2 |= 0x0D;
                
                TextboxTextNode = (AtkTextNode*)node;
                gotNode = true;
            }
        }

        initedAtk = true;
    }

    public void OnEnable()
    {
        Service.Framework.Update += Update;
    }

    public void OnDisable()
    {
        Service.Framework.Update -= Update;
    }

    private void Update(Framework framework)
    {
        // atk bullshit time

        if (!Service.ClientState.IsLoggedIn)
            return; // we cant do shit if were not logged in, because the textbox wont have loaded
        
        if (!initedAtk)
            InitAtk();
        
        if (!gotNode) return; // fuck outta here to avoid crashing
        
        if ((((AtkResNode*)TextboxTextNode)->Flags & 0x20) == 0)
            return; // fuck outta here, the story textbox is invisible! no need

        var text = TextboxTextNode->NodeText.ToString().Replace("\n", " ");

        if (text.Contains(triggerPhrase) && !lastText.Equals(text))
        {
            TriggerAudio();
        }

        lastText = text;
    }

    public void TriggerAudio()
    {
        AudioHelper.Instance.PlayOneshot(amogus);
    }

    public void DrawUi()
    {
        var phrase = triggerPhrase;
        if (ImGui.InputText("Trigger phrase", ref phrase, 65536))
            triggerPhrase = phrase;
    }
}
