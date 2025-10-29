using HarmonyLib;
using UnityEngine;

namespace TabzSteamRemover.Patches;

[HarmonyPatch(typeof(TABZChat))]
public class TabzChatPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(TABZChat.Gangstify))]
    public static bool SendChatMessagePrefix(TABZChat __instance, ref string __result) {
        __result = string.Equals(PhotonNetwork.playerName, "brodal") || Random.Range(0, 100) >= 98 ? "BRAPP!" : __instance.mGangstaPhrases[Random.Range(0, __instance.mGangstaPhrases.Length)];
        return false;
    }
}