using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Shortcuts;

[HarmonyPatch(typeof(Dialog_MapSearch), nameof(Dialog_MapSearch.PostOpen))]
public static class Dialog_MapSearch_PostOpen
{
    private static void Postfix(SortedList<string, Thing> ___searchResults)
    {
        ShortcutsMain.currentSelection = -1;
        QuickSearchWidget_OnGUI.divertedEvents[KeyCode.UpArrow] = () =>
        {
            if (___searchResults.Count == 0)
            {
                return;
            }

            switch (ShortcutsMain.currentSelection)
            {
                case -1:
                    ShortcutsMain.currentSelection = ___searchResults.Count - 1;
                    break;
                case > 0:
                    ShortcutsMain.currentSelection--;
                    break;
            }
        };
        QuickSearchWidget_OnGUI.divertedEvents[KeyCode.DownArrow] = () =>
        {
            if (___searchResults.Count == 0)
            {
                return;
            }

            if (ShortcutsMain.currentSelection == -1)
            {
                ShortcutsMain.currentSelection = 0;
            }
            else if (ShortcutsMain.currentSelection < ___searchResults.Count - 1)
            {
                ShortcutsMain.currentSelection++;
            }
        };
        QuickSearchWidget_OnGUI.divertedEvents[KeyCode.Return] = () =>
        {
            if (ShortcutsMain.currentSelection < 0 ||
                ShortcutsMain.currentSelection >= ___searchResults.Count)
            {
                return;
            }

            var thing = ___searchResults.ToArray()[ShortcutsMain.currentSelection].Value;
            Find.MainTabsRoot.EscapeCurrentTab();
            Find.Selector.Select(thing);
            Find.CameraDriver.JumpToCurrentMapLoc(thing.Spawned ? thing.Position : thing.PositionHeld);
        };
    }
}