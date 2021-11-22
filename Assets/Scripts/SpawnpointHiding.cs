using UnityEngine;

public class SpawnpointHiding : MonoBehaviour
{
	[SerializeField] 
	GameObject graphics;

	private void Awake()
	{
		graphics.SetActive(false);
	}
}
