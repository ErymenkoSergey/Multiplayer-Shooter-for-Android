using System.Threading.Tasks;
using UnityEngine;

public class SpawnPlayerNewScenes : MonoBehaviour
{
    PlayerController playerController;

    private async void Awake()
    {
        await Task.Delay(2000);
        playerController = FindObjectOfType<PlayerController>();
    }

    private async void OnTriggerEnter(Collider other)
    {
        await Task.Delay(2200);
        if (other.gameObject.tag == "Player")
        {
            playerController.LoadingNewScenesOff();
            Destroy(gameObject);
        }
    }
}
