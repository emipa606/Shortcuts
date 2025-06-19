using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using static HarmonyLib.Code;

namespace Shortcuts;

[HarmonyPatch(typeof(Dialog_Search<Thing>), nameof(Dialog_Search<Thing>.DoWindowContents))]
public static class Dialog_Search_DoWindowContens
{
    private static readonly MethodInfo mIsOver = AccessTools.Method(typeof(Mouse), nameof(Mouse.IsOver));
    private static readonly MethodInfo mMyIsOver = SymbolExtensions.GetMethodInfo(() => isOver(default, 0));

    private static bool isOver(Rect rect, int i)
    {
        if (Mouse.IsOver(rect))
        {
            return true;
        }

        return i == ShortcutsMain.currentSelection;
    }

    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions)
            .MatchStartForward(Ldstr["DrawSearchResults"])
            .MatchEndForward(CodeMatch.LoadsConstant(0), Stloc);
        var iVar = matcher.Operand;
        return matcher
            .MatchStartForward(Call[mIsOver])
            .InsertAndAdvance(Ldloc[iVar])
            .SetOperandAndAdvance(mMyIsOver)
            .InstructionEnumeration();
    }

    private static void Prefix(SortedList<string, Thing> ___searchResults)
    {
        if (ShortcutsMain.currentSelection == -1 && ___searchResults.Any())
        {
            ShortcutsMain.currentSelection = 0;
        }

        if (ShortcutsMain.currentSelection < 0)
        {
            ShortcutsMain.currentSelection = 0;
        }

        if (ShortcutsMain.currentSelection >= ___searchResults.Count)
        {
            ShortcutsMain.currentSelection = ___searchResults.Count - 1;
        }
    }
}