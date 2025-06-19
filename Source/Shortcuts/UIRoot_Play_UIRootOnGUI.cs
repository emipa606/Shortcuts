using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Shortcuts;

[HarmonyPatch(typeof(UIRoot_Play), nameof(UIRoot_Play.UIRootOnGUI))]
public static class UIRoot_Play_UIRootOnGUI
{
    private static readonly MethodInfo
        toggleTabMethodInfo = AccessTools.Method(typeof(InspectPaneUtility), "ToggleTab");

    private static void toggleTabs()
    {
        if (Find.Selector.SingleSelectedThing is not Pawn)
        {
            return;
        }

        Func<InspectTabBase, bool> predicate = null;
        if (Defs.ToggleGear.KeyDownEvent)
        {
            predicate = t => t is ITab_Pawn_Gear;
        }

        if (Defs.ToggleBio.KeyDownEvent)
        {
            predicate = t => t is ITab_Pawn_Character;
        }

        if (Defs.ToggleNeeds.KeyDownEvent)
        {
            predicate = t => t is ITab_Pawn_Needs;
        }

        if (Defs.ToggleHealth.KeyDownEvent)
        {
            predicate = t => t is ITab_Pawn_Health;
        }

        if (predicate == null)
        {
            return;
        }

        var mainTabWindowInspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
        var tab = mainTabWindowInspect.CurTabs.First(predicate);
        toggleTabMethodInfo.Invoke(null, [tab, mainTabWindowInspect]);
    }

    public static void Postfix()
    {
        if (Event.current.type != EventType.KeyDown)
        {
            return;
        }

        toggleTabs();
    }
}