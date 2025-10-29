using HarmonyLib;

namespace TabzSteamRemover.Patches;

[HarmonyPatch(typeof(NetworkStatistics))]
public class NetworkStatisticsPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(NetworkStatistics.Start))]
    public static bool StartPrefix(NetworkStatistics __instance) {
        __instance.serverName = (string)PhotonNetwork.room.CustomProperties[MainMenuHandler.SERVER_NAME_PARAM];
        __instance.steamRoom = ServerSearcher.CurrentSelectedServer.ToString();
        __instance.photonRoom = PhotonNetwork.room.Name;
        return false;
    }
    
}