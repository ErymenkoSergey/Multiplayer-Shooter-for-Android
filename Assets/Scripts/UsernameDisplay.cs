using Photon.Pun;
using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
	[SerializeField] 
	PhotonView playerPV;
	[SerializeField] 
	TMP_Text text;
	private int teamId;

	private void Start()
	{

		if(playerPV.IsMine)
			gameObject.SetActive(false);

		teamId = GameMeaning.teamID;

		if (teamId == 1)
		    text.text = playerPV.Owner.NickName;

		if (teamId == 2)
			text.text = " ";
	}
}