using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace Shortcuts;

[HarmonyPatch(typeof(QuickSearchWidget), nameof(QuickSearchWidget.OnGUI))]
public static class QuickSearchWidget_OnGUI
{
    public static readonly Dictionary<KeyCode, Action> divertedEvents = [];

    private static void Prefix()
    {
        if (Event.current.type != EventType.KeyDown)
        {
            return;
        }

        if (!divertedEvents.TryGetValue(Event.current.keyCode, out var action))
        {
            return;
        }

        action();
        Event.current.Use();
    }
}