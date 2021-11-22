using UnityEngine;

public class DestroyObjectControl : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            Destroy(other.gameObject);
        }
        
        Debug.Log("Destroy Object " + other.name);
    }
}
