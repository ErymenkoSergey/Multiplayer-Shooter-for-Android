using UnityEngine;
using Photon.Pun;


[RequireComponent (typeof(PhotonView))]
public class RoleDistribution : MonoBehaviour
{
    PlayerController player;
    PhotonView PV;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        PV = GetComponent<PhotonView>();
    }

    public void ManiacTeam()
    {
        GameMeaning.teamID = 1;
        GameMeaning.hasPickedTeam = true;
    }

    public void HidingTeam()
    {
        GameMeaning.teamID = 2;
        GameMeaning.hasPickedTeam = true;
    }
}
