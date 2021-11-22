using UnityEngine;

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class GunInfo : ItemInfo
{
	public float damage;

	public int id;

	private void Awake()
	{
		id = GameMeaning.teamID;
	}
}