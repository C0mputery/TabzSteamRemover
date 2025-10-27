using HarmonyLib;
using UnityEngine;

namespace TabzSteamRemover.Patches;

[HarmonyPatch(typeof(SteamManager))]
public static class SteamManagerPatche {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(SteamManager.Awake))]
    public static bool AwakePrefix(SteamManager __instance) {
        Object.Destroy(__instance);
        return false;
    }
}