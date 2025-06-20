using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var matcher = new CodeMatcher(instructions)
            .MatchStartForward(Ldstr["DrawSearchResults"])
            .ThrowIfInvalid("Could not find 'DrawSearchResults'")
            .MatchEndForward(CodeMatch.LoadsConstant(0), CodeMatch.IsStloc())
            .ThrowIfInvalid("Could not find assignment to local int after 'DrawSearchResults'");

        var iVar = matcher.Instruction.opcode switch
        {
            var op when op == OpCodes.Stloc_0 => 0,
            var op when op == OpCodes.Stloc_1 => 1,
            var op when op == OpCodes.Stloc_2 => 2,
            var op when op == OpCodes.Stloc_3 => 3,
            var op when op == OpCodes.Stloc_S || op == OpCodes.Stloc =>
                matcher.Instruction.operand switch
                {
                    int val => val,
                    LocalBuilder lb => lb.LocalIndex,
                    _ => throw new InvalidOperationException(
                        $"Unrecognized operand type: {matcher.Instruction.operand?.GetType()}")
                },
            _ => throw new InvalidOperationException($"Unexpected opcode: {matcher.Instruction.opcode}")
        };


        matcher = matcher
            .MatchStartForward(Call[mIsOver])
            .ThrowIfInvalid("Could not find Mouse.IsOver call")
            .InsertAndAdvance(Ldloc[iVar])
            .SetOperandAndAdvance(mMyIsOver);

        return matcher.InstructionEnumeration();
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