using Brrainz;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Shortcuts
{
	[DefOf]
	public static class Defs
	{
		public static KeyBindingDef ToggleGear;
		public static KeyBindingDef ToggleBio;
		public static KeyBindingDef ToggleNeeds;
		public static KeyBindingDef ToggleHealth;
	}

	public partial class ShortcutsMain : Mod
	{
		public ShortcutsMain(ModContentPack content) : base(content)
		{
			var harmony = new Harmony("net.pardeike.rimworld.mod.shortcuts");
			harmony.PatchAll();

			CrossPromotion.Install(76561197973010050);
		}
	}
}