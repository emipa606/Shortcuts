using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using static HarmonyLib.Code;

namespace Shortcuts
{

	public partial class ShortcutsMain
	{
		[HarmonyPatch]
		class Patch_Dialog_MapSearch
		{
			static int currentSelection = -1;

			static readonly MethodInfo mIsOver = AccessTools.Method(typeof(Mouse), nameof(Mouse.IsOver));
			static readonly MethodInfo mMyIsOver = SymbolExtensions.GetMethodInfo(() => IsOver(default, default));

			static bool IsOver(Rect rect, int i)
			{
				if (Mouse.IsOver(rect))
					return true;
				return i == currentSelection;
			}

			[HarmonyTranspiler]
			[HarmonyPatch(typeof(Dialog_MapSearch), nameof(Dialog_MapSearch.DoWindowContents))]
			static IEnumerable<CodeInstruction> Transpiler_DoWindowContents(IEnumerable<CodeInstruction> instructions)
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

			[HarmonyPrefix]
			[HarmonyPatch(typeof(Dialog_MapSearch), nameof(Dialog_MapSearch.DoWindowContents))]
			static void Prefix_DoWindowContents(SortedList<string, Thing> ___searchResults)
			{
				if (currentSelection == -1 && ___searchResults.Any())
					currentSelection = 0;
				if (currentSelection < 0)
					currentSelection = 0;
				if (currentSelection >= ___searchResults.Count)
					currentSelection = ___searchResults.Count - 1;
			}

			[HarmonyPostfix]
			[HarmonyPatch(typeof(Dialog_MapSearch), nameof(Dialog_MapSearch.PostOpen))]
			static void Postfix_PostOpen(SortedList<string, Thing> ___searchResults)
			{
				currentSelection = -1;
				Patch_QuickSearchWidget.divertedEvents[KeyCode.UpArrow] = () =>
				{
					if (___searchResults.Count == 0)
						return;
					if (currentSelection == -1)
						currentSelection = ___searchResults.Count - 1;
					else if (currentSelection > 0)
						currentSelection--;
				};
				Patch_QuickSearchWidget.divertedEvents[KeyCode.DownArrow] = () =>
				{
					if (___searchResults.Count == 0)
						return;
					if (currentSelection == -1)
						currentSelection = 0;
					else if (currentSelection < ___searchResults.Count - 1)
						currentSelection++;
				};
				Patch_QuickSearchWidget.divertedEvents[KeyCode.Return] = () =>
				{
					if (currentSelection >= 0 && currentSelection < ___searchResults.Count)
					{
						var thing = ___searchResults.ToArray()[currentSelection].Value;
						Find.MainTabsRoot.EscapeCurrentTab(true);
						Find.Selector.Select(thing);
						Find.CameraDriver.JumpToCurrentMapLoc(thing.Spawned ? thing.Position : thing.PositionHeld);
					}
				};
			}

			[HarmonyPostfix]
			[HarmonyPatch(typeof(Dialog_MapSearch), nameof(Dialog_MapSearch.PostClose))]
			static void Postfix_PostClose()
			{
				currentSelection = -1;
				Patch_QuickSearchWidget.divertedEvents.Remove(KeyCode.UpArrow);
				Patch_QuickSearchWidget.divertedEvents.Remove(KeyCode.DownArrow);
				Patch_QuickSearchWidget.divertedEvents.Remove(KeyCode.Return);
			}
		}
	}
}