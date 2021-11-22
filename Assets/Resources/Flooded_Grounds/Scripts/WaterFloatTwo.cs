using UnityEngine;

public class WaterFloatTwo : MonoBehaviour 
{
	public float WaterHeight = 15.5f;

	private void Update () 
	{
		if (transform.position.y < WaterHeight)
			transform.position = new Vector3 (transform.position.x, WaterHeight, transform.position.z);
	}
}
