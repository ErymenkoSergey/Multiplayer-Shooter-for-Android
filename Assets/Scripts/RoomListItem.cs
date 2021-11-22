using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
	[SerializeField] TMP_Text text;

	public RoomInfo info;

	public void SetUp(RoomInfo _info)
	{
		info = _info;
		text.text = _info.Name;
	}

	public void OnClick()
	{
		if (GameMeaning.teamID == 1 || GameMeaning.teamID == 2)
			Launcher.inst?.JoinRoom(info);
		else
			CheckFindRommTeams.inst?.OnClickCheckTeams();
	}
}