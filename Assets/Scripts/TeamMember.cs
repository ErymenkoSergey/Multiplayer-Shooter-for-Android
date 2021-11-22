using UnityEngine;

public class TeamMember : MonoBehaviour
{
    public int teamID;

    private void Start()
    {
        teamID = GameMeaning.teamID;
    }
}
