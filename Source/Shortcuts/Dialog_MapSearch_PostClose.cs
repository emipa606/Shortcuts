using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace Shortcuts;

[HarmonyPatch(typeof(Dialog_MapSearch), nameof(Dialog_MapSearch.PostClose))]
public static class Dialog_MapSearch_PostClose
{
    private static void Postfix()
    {
        ShortcutsMain.currentSelection = -1;
        QuickSearchWidget_OnGUI.divertedEvents.Remove(KeyCode.UpArrow);
        QuickSearchWidget_OnGUI.divertedEvents.Remove(KeyCode.DownArrow);
        QuickSearchWidget_OnGUI.divertedEvents.Remove(KeyCode.Return);
    }
}