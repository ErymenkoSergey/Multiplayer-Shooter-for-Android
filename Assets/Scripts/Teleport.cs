using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject teleport;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SoundManager.inst?.TeleportOneSound();
            other.transform.position = teleport.transform.position;
        }
    }
}
