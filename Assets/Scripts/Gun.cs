using UnityEngine;

public abstract class Gun : Item
{
	public abstract override void Use();

	public GameObject bulletImpactPrefab;

	public GameObject mineJump;
}
