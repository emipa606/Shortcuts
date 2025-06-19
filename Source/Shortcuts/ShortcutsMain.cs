using HarmonyLib;
using Verse;

namespace Shortcuts;

public class ShortcutsMain : Mod
{
    public static int currentSelection = -1;

    public ShortcutsMain(ModContentPack content) : base(content)
    {
        new Harmony("net.pardeike.rimworld.mod.shortcuts").PatchAll();
    }
}