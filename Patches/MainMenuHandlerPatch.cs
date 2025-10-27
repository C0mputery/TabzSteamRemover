using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

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
    [HarmonyPatch(nameof(MainMenuHandler.Start))]
    public static bool StartPrefix(MainMenuHandler __instance) {
        GameObject nameObject = GameObject.Find("PlayerName");
        
        RectTransform rectTransform = nameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(750, rectTransform.sizeDelta.y);
        rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y, rectTransform.position.z - 0.35f);
        
        InputField inputField = nameObject.AddComponent<InputField>();
        inputField.textComponent = __instance.m_PlayerNameText;
        inputField.text = PlayerPrefs.GetString("PlayerName", "Default Name");
        inputField.characterLimit = 30;
        inputField.onEndEdit = new InputField.SubmitEvent();
        inputField.onEndEdit.AddListener(delegate(string value) {
            PlayerPrefs.SetString("PlayerName", value);
            PlayerPrefs.Save();
            PhotonNetwork.playerName = value;
        });

        GameObject newChildObject = new GameObject("background");
        RectTransform backgroundRectTransform = newChildObject.AddComponent<RectTransform>();
        backgroundRectTransform.SetParent(nameObject.transform, false);
        backgroundRectTransform.anchorMin = new Vector2(0, 0);
        backgroundRectTransform.anchorMax = new Vector2(1, 1);
        backgroundRectTransform.anchoredPosition = Vector2.zero;
        backgroundRectTransform.sizeDelta = new Vector2(15, 50);
        Image backgroundImage = newChildObject.AddComponent<Image>();
        backgroundImage.color = new Color(0f, 0f, 0f, 0.5f);

        if (__instance.FirstTime) { __instance.PromptForPlayerName(); }
        
        return false;
    }
}