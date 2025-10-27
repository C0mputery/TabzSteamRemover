using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TabzSteamRemover.Patches;

[HarmonyPatch(typeof(ServerSearcher))]
public static class ServerSearcherPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ServerSearcher.SearchForServers))]
    public static bool SearchForServersPrefix(ServerSearcher __instance) {
        // Hackerman code
        __instance.anim.state1 = true;
        RoomInfo[] roomList = PhotonNetwork.GetRoomList();
        if (roomList == null || roomList.Length == 0) {
            __instance.anim.state1 = true;
            return false;
        }
        __instance.anim.state1 = false;
        __instance.ClearServerList();
        foreach (RoomInfo roomInfo in roomList) {
            GameObject gameObject = Object.Instantiate(__instance.mServerCell, __instance.mServerListContent, true);
            gameObject.GetComponent<SetServerTextValues>().InitServerValues(new ServerSearcher.ServerCellParams {
                LobbyName = roomInfo.Name,
                PlayersInside = roomInfo.PlayerCount,
                PlayersMax = roomInfo.MaxPlayers
            });
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