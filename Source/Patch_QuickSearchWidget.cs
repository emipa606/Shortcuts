using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shortcuts
{

	public partial class ShortcutsMain
	{
		[HarmonyPatch]
		class Patch_QuickSearchWidget
		{
			public static readonly Dictionary<KeyCode, Action> divertedEvents = [];

			[HarmonyPatch(typeof(QuickSearchWidget), nameof(QuickSearchWidget.OnGUI))]
			static void Prefix()
			{
				if (Event.current.type == EventType.KeyDown)
					if (divertedEvents.TryGetValue(Event.current.keyCode, out var action))
					{
						action();
						Event.current.Use();
					}
			}
		}
	}
}