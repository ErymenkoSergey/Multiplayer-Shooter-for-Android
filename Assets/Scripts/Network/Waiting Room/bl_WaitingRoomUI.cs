﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class bl_WaitingRoomUI : bl_PhotonHelper
{
    [Header("References")]
    public GameObject Content;
    public GameObject WaitingPlayerPrefab;
    public GameObject LoadingMapUI;
    public GameObject StartScreen;
    public GameObject LeaveConfirmUI;
    public GameObject waitingRequiredPlayersUI;
    public RectTransform PlayerListPanel;
    public Text RoomNameText;
    public Text MapNameText;
    public Text GameModeText;
    public Text TimeText;
    public Text GoalText;
    public Text BotsText;
    public Text FriendlyFireText;
    public Text PlayerCountText;
    public Image MapPreview;
    public List<RectTransform> PlayerListHeaders = new List<RectTransform>();
    public Button[] readyButtons;

    private List<bl_WaitingPlayerUI> playerListCache = new List<bl_WaitingPlayerUI>();

    private void OnEnable()
    {
        Content.SetActive(false);
    }

    public void Show()
    {
        UpdateRoomInfoUI();
        InstancePlayerList();
        Content.SetActive(true);
        StartScreen.SetActive(true);
    }

    public void Hide()
    {
        LeaveConfirmUI.SetActive(false);
        StartScreen.SetActive(false);
        Content.SetActive(false);
        bl_LobbyUI.Instance.blackScreenFader.FadeOut(0.5f);
    }

    //internal void HidePanel()
    //{
    //    Content.SetActive(false);
    //    //Debug.Log("Admin create boool 0 " + isCreateRoom);
    //}

    public void InstancePlayerList()
    {
        playerListCache.ForEach(x => { if (x != null) { Destroy(x.gameObject); } });
        playerListCache.Clear();

        Player[] list = PhotonNetwork.PlayerList;
        List<Player> secondTeam = new List<Player>();
        bool otm = isOneTeamModeUpdate;
        PlayerListHeaders.ForEach(x => x.gameObject.SetActive(!otm));
        for (int i = 0; i < list.Length; i++)
        {
            if (otm)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (list[i].GetPlayerTeam() != Team.All)
                    {
                        list[i].SetPlayerTeam(Team.All);
                    }
                }
                SetPlayerToList(list[i]);
            }
            else
            {
                if (list[i].GetPlayerTeam() == Team.Team1)
                {
                    SetPlayerToList(list[i]);
                }
                else if (list[i].GetPlayerTeam() == Team.Team2)
                {
                    secondTeam.Add(list[i]);
                }
            }
        }
        if (!otm) { PlayerListHeaders[1].SetAsLastSibling(); }
        if (secondTeam.Count > 0)
        {
            for (int i = 0; i < secondTeam.Count; i++)
            {
                SetPlayerToList(secondTeam[i]);
            }
        }
        UpdatePlayerCount();
    }

    public void SetPlayerToList(Player player)
    {
        GameObject g = Instantiate(WaitingPlayerPrefab) as GameObject;
        bl_WaitingPlayerUI wp = g.GetComponent<bl_WaitingPlayerUI>();
        wp.SetInfo(player);
        g.transform.SetParent(PlayerListPanel, false);
        playerListCache.Add(wp);
    }

    //internal bool isCreateRoom;
    public void UpdateRoomInfoUI()
    {
        GameMode mode = GetGameModeUpdated;
        Room room = PhotonNetwork.CurrentRoom;
        RoomNameText.text = room.Name.ToUpper();
        string mapName = (string)room.CustomProperties[PropertiesKeys.SceneNameKey];
        bl_GameData.SceneInfo si = bl_GameData.Instance.AllScenes.Find(x => x.RealSceneName == mapName);

        //if (!isCreateRoom)
        //{
            //Debug.Log("Admin create boool 2 " + isCreateRoom);
            MapPreview.sprite = si.Preview;
            MapNameText.text = si.ShowName.ToUpper();
            int t = (int)room.CustomProperties[PropertiesKeys.TimeRoomKey];
            TimeText.text = (t / 60).ToString().ToUpper() + ":00";
            BotsText.text = string.Format("BOTS: {0}", (bool)room.CustomProperties[PropertiesKeys.WithBotsKey] ? "ON" : "OFF");
            FriendlyFireText.text = string.Format("FRIENDLY FIRE: {0}", (bool)room.CustomProperties[PropertiesKeys.RoomFriendlyFire] ? "ON" : "OFF");
            GoalText.text = (string)room.CustomProperties[PropertiesKeys.RoomGoal].ToString() + " " + GetGameModeUpdated.GetModeInfo().GoalName.ToUpper();

        //}
       // Debug.Log("Admin create boool 3 " + isCreateRoom);
        UpdatePlayerCount();
        GameModeText.text = mode.GetName().ToUpper();
        readyButtons[0].gameObject.SetActive(PhotonNetwork.IsMasterClient);
        readyButtons[1].gameObject.SetActive(!PhotonNetwork.IsMasterClient);
        
        readyButtons[1].GetComponentInChildren<Text>().text = bl_WaitingRoom.Instance.isLocalReady ? "CANCEL" : "READY";
        //if (isCreateRoom)
        //{
        //    HidePanel();
        //    Debug.Log("Admin create boool 333 " + isCreateRoom);
        //}
            
    }

    public void UpdatePlayerCount()
    {
        int required = GetGameModeUpdated.GetGameModeInfo().RequiredPlayersToStart;
        if (required > 1)
        {
            bool allRequired = (PhotonNetwork.PlayerList.Length >= required);
            readyButtons[0].interactable = (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= required);
            PlayerCountText.text = string.Format("{0} OF {2} PLAYERS ({1} MAX)", PhotonNetwork.PlayerList.Length, PhotonNetwork.CurrentRoom.MaxPlayers, required);
            waitingRequiredPlayersUI?.SetActive(!allRequired);
        }
        else
        {
            readyButtons[0].interactable = true;
            waitingRequiredPlayersUI?.SetActive(false);
            PlayerCountText.text = string.Format("{0} PLAYERS ({1} MAX)", PhotonNetwork.PlayerList.Length, PhotonNetwork.CurrentRoom.MaxPlayers);
        }
    }

    public void UpdateAllPlayersStates()
    {
        playerListCache.ForEach(x => { if (x != null) x.UpdateState(); });
    }

    public void SetLocalReady()
    {
        bl_WaitingRoom.Instance.SetLocalPlayerReady();
        readyButtons[1].GetComponentInChildren<Text>().text = bl_WaitingRoom.Instance.isLocalReady ? "CANCEL" : "READY";
    }

    public void MasterStartTheGame()
    {
        bl_WaitingRoom.Instance.StartGame();
    }

    public void LeaveRoom(bool comfirmed)
    {
        if (comfirmed)
        {
            bl_LobbyUI.Instance.blackScreenFader.FadeIn(0.5f);
            PhotonNetwork.LeaveRoom();
        }
        else
            LeaveConfirmUI.SetActive(true);
    }

    private static bl_WaitingRoomUI _instance;

    public static bl_WaitingRoomUI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<bl_WaitingRoomUI>();
            }
            return _instance;
        }
    }
}