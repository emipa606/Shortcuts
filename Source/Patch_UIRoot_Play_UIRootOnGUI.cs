using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace Shortcuts
{

	public partial class ShortcutsMain
	{
		[HarmonyPatch(typeof(UIRoot_Play), nameof(UIRoot_Play.UIRootOnGUI))]
		class Patch_UIRoot_Play_UIRootOnGUI
		{
			static bool ToggleTabs()
			{
				if (Find.Selector.SingleSelectedThing is not Pawn pawn)
					return false;

				Func<InspectTabBase, bool> predicate = null;
				if (Defs.ToggleGear.KeyDownEvent)
					predicate = t => t is ITab_Pawn_Gear;
				if (Defs.ToggleBio.KeyDownEvent)
					predicate = t => t is ITab_Pawn_Character;
				if (Defs.ToggleNeeds.KeyDownEvent)
					predicate = t => t is ITab_Pawn_Needs;
				if (Defs.ToggleHealth.KeyDownEvent)
					predicate = t => t is ITab_Pawn_Health;

				if (predicate != null)
				{
					var mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
					var tab = mainTabWindow_Inspect.CurTabs.First(predicate);
					InspectPaneUtility.ToggleTab(tab, mainTabWindow_Inspect);
					return true;
				}

				return false;
			}

			public static void Postfix()
			{
				if (Event.current.type != EventType.KeyDown)
					return;
				if (ToggleTabs())
					return;
			}
		}
	}
}