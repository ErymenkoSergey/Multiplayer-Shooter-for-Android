using UnityEngine;

public class SpawnpointManiac : MonoBehaviour
{
	[SerializeField]
	GameObject graphics;

	private void Awake()
	{
		graphics.SetActive(false);
	}
}
