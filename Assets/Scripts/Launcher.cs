using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
	public static Launcher inst;

	[SerializeField]
	TMP_InputField roomNameInputField;

	[SerializeField]
	TMP_Text errorText, roomNameText;

	[SerializeField]
	Transform roomListContent, playerListContent;

	[SerializeField]
	GameObject PlayerListItemPrefab, startGameButton, roomListItemPrefab;

	[SerializeField]
	GameObject panelControlsTeams;

	[SerializeField]
	Button startGame;

	private void Awake()
	{
		inst = this;
	}

	private void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
		startGame.onClick.AddListener(StartGameControll);
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinedLobby()
	{
		MenuManager.inst.OpenMenu("title");
	}

	public void CreateRoom()
	{
		if (string.IsNullOrEmpty(roomNameInputField.text))
			return;

		PhotonNetwork.CreateRoom(roomNameInputField.text);
		MenuManager.inst.OpenMenu("loading");
		SoundManager.inst.PlayButton();
	}

	public override void OnJoinedRoom()
	{
		SoundManager.inst.PlayButton();
		MenuManager.inst.OpenMenu("room");
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;

		Player[] players = PhotonNetwork.PlayerList;

		foreach (Transform child in playerListContent)
		{
            Destroy(child.gameObject);
        }

		for (int i = 0; i < players.Count(); i++)
			Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);

		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		SoundManager.inst.PlayButton();
		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room Creation Failed: " + message;
		MenuManager.inst.OpenMenu("error");
	}

	public async void StartGame()
	{
		if (GameMeaning.teamID == 1 || GameMeaning.teamID == 2)
		{
			SoundManager.inst.PlayButton();
			await Task.Delay(GameMeaning.TIMINGLOADGAMESTART);
			PhotonNetwork.LoadLevel(GameMeaning.SCENEFIRST);
			Cursor.visible = false;
		}
		else
			panelControlsTeams.SetActive(true);
	}

	public void StartGameControll()
	{
		panelControlsTeams.SetActive(false);
	}

	public async void LeaveRoom()
	{
		SoundManager.inst.PlayButton();
		await Task.Delay(GameMeaning.TIMINGLEAVEROOM);
		PhotonNetwork.LeaveRoom();
		MenuManager.inst.OpenMenu("loading");
	}

	public void JoinRoom(RoomInfo info)
	{
		SoundManager.inst.PlayButton();
		PhotonNetwork.JoinRoom(info.Name);
		MenuManager.inst.OpenMenu("loading");
	}

	public override void OnLeftRoom()
	{
		SoundManager.inst.PlayButton();
		MenuManager.inst.OpenMenu("title");
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach (Transform trans in roomListContent)
		{
			Destroy(trans.gameObject); 
		}

		for (int i = 0; i < roomList.Count; i++)
		{
			if (roomList[i].RemovedFromList)
				continue;
			Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}
}