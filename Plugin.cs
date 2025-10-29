using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;


namespace TabzSteamRemover;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
    private void Awake() {
        PhotonNetwork.autoJoinLobby = true;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_GUID);
    }
}