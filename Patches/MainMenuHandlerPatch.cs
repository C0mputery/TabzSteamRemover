using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace TabzSteamRemover.Patches;

[HarmonyPatch(typeof(MainMenuHandler))]
public static class MainMenuHandlerPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenuHandler.PlayerName), MethodType.Getter)]
    public static bool GetPlayerNamePrefix(ref string __result) {
        __result = PlayerPrefs.GetString("PlayerName", "Default Name");
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenuHandler.JoinRandomRoom))]
    public static bool JoinRandomRoomPrefix() {
        PhotonNetwork.JoinRandomRoom();
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenuHandler.OnPhotonRandomJoinFailed))]
    public static bool OnPhotonRandomJoinFailedPrefix() {
        PhotonNetwork.JoinOrCreateRoom("Zombielator.com", new RoomOptions {
            MaxPlayers = MainMenuHandler.mMaxPlayers,
            CleanupCacheOnLeave = false,
        }, null);
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenuHandler.OnJoinedRoom))]
    public static bool OnJoinedRoomPrefix() {
        PhotonNetwork.LoadLevel(1);
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenuHandler.Awake))]
    public static bool AwakePrefix() {
        // Add Name Change UI
        return true;
    }
}