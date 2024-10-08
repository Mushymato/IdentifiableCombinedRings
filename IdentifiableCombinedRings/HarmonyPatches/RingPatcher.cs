﻿using HarmonyLib;

using Netcode;

using StardewValley.Objects;

namespace IdentifiableCombinedRings.HarmonyPatches;

/// <summary>
/// Class to hold patches on the Ring class.
/// </summary>
[HarmonyPatch(typeof(Ring))]
internal class RingPatcher
{
    /// <summary>
    /// Patches the DisplayName method.
    /// </summary>
    /// <param name="__instance">Combined ring to check.</param>
    /// <param name="__result">Output (the Display Name).</param>
    [HarmonyPostfix]
    [HarmonyPriority(Priority.High)]
    [HarmonyPatch(nameof(Ring.DisplayName), MethodType.Getter)]
    internal static void PostfixGetDisplayName(Ring __instance, ref string __result)
    {
        if (__instance is CombinedRing combinedRing)
        {
            NetList<Ring, NetRef<Ring>> combinedRings = combinedRing.combinedRings;
            if (combinedRings.Count <= 1)
            {
                return;
            }
            if (combinedRings.Count > 2 || combinedRings[0] is CombinedRing || combinedRings[1] is CombinedRing)
            {
                __result += I18n.Many();
                return;
            }

            __result = combinedRings[0].DisplayName + " & " + combinedRings[1].DisplayName;
        }
    }
}