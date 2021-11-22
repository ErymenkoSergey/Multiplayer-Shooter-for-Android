using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager inst;

    PhotonView PV;

    private bool hasPickedTeam = false;

    private int teamID = 0;

    GameObject controller;

    private void Awake()
    {
        inst = this;
        PV = GetComponent<PhotonView>();
        DontDestroyOnLoad(gameObject); //!!!!!!!!!!!!!!!
    }

    private void Start()
    {
        hasPickedTeam = GameMeaning.hasPickedTeam;
        teamID = GameMeaning.teamID;
        Collectors();
    }

    private void Collectors()
    {
        if (PV.IsMine)
            switch (teamID)
            {
                case 1:
                    CreateController(1);
                    break;
                case 2:
                    CreateController(2);
                    break;
            }
    }

    private void CreateController(int teamID)
    {
        if (hasPickedTeam)
        {
            if (teamID == 1)
            {
                Transform spawnpoint = SpawnManager.inst.GetSpawnpointManiac();

                controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs",
                "PlayerController"), spawnpoint.position, spawnpoint.rotation,
                0, new object[] { PV.ViewID });

                controller.GetComponent<TeamMember>().teamID = teamID;

                SkinnedMeshRenderer skinnedMesh = controller.GetComponentInChildren<SkinnedMeshRenderer>();
                skinnedMesh.material.color = Color.red;
            }

            if (teamID == 2)
            {
                Transform spawnpoint = SpawnManager.inst.GetSpawnpointHiding();

                controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs",
                "PlayerController"), spawnpoint.position, spawnpoint.rotation,
                0, new object[] { PV.ViewID });

                controller.GetComponent<TeamMember>().teamID = teamID;

                SkinnedMeshRenderer skinnedMesh = controller.GetComponentInChildren<SkinnedMeshRenderer>();
                skinnedMesh.material.color = Color.white;
            }
        }
    }

    internal void Die()
    {
        PhotonNetwork.Destroy(controller);
        Collectors();
    }

    public void DisconnectController()
    {
        PhotonNetwork.Destroy(controller);
    }
}