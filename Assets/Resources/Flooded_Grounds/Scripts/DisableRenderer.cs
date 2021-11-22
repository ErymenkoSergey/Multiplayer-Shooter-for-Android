using UnityEngine;

public class DisableRenderer : MonoBehaviour 
{
	private void Start () 
	{
		GetComponent<Renderer>().enabled = false;
	}
}
