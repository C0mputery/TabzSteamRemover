using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace TabzSteamRemover.Patches;

[HarmonyPatch(typeof(PauseMenuHandler))]
public static class PauseMenuHandlerPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PauseMenuHandler.OnQuitClicked))]
    public static bool OnQuitClicked_Prefix() {
        if (!PhotonNetwork.LeaveRoom()) { PhotonNetwork.Disconnect(); }
        SceneManager.LoadScene(0);
        return false;
    }
}