using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TabzSteamRemover.Patches;

[HarmonyPatch(typeof(ServerSearcher))]
public static class ServerSearcherPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ServerSearcher.OnEnable))]
    public static bool OnEnablesPrefix(ServerSearcher __instance) {
        __instance.mMainMenuHandler = Object.FindObjectOfType<MainMenuHandler>();
        
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();
        
        
        if (roomList.Length == 0) { __instance.anim.state1 = true; return false; }
        
        __instance.anim.state1 = false;
        __instance.ClearServerList();
        
        foreach (RoomInfo roomInfo in roomList) {
            GameObject gameObject = Object.Instantiate(__instance.mServerCell, __instance.mServerListContent, true);
            SetServerTextValues setServerTextValues = gameObject.GetComponent<SetServerTextValues>();
            setServerTextValues.mServerNickName = roomInfo.Name;
            setServerTextValues.mServerText.text = $"{roomInfo.Name}      : {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
            gameObject.SetActive(true);
        }
        
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ServerSearcher.OnSubmit))]
    public static bool OnSubmitPrefix(ref BaseEventData data) {
        PhotonNetwork.JoinOrCreateRoom(data.selectedObject.GetComponent<SetServerTextValues>().ServerNickName, new RoomOptions {
            MaxPlayers = MainMenuHandler.MaxPlayers,
            CleanupCacheOnLeave = false
        }, null);
        return false;
    }
}