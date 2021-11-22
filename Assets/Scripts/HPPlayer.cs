using System.Threading.Tasks;
using UnityEngine;

public class HPPlayer : MonoBehaviour
{
    PlayerController playerController;

    private async void Start()
    {
        await Task.Delay(4000);
        playerController = FindObjectOfType<PlayerController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController.HpPlus();
        }
    }
}
