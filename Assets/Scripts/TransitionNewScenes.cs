using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionNewScenes : MonoBehaviour
{
    PlayerController playerController;

    private async void Awake()
    {
        await Task.Delay(3000);
        playerController = FindObjectOfType<PlayerController>();
    }

    private async void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController.LoadingNewScenesOn();
            await Task.Delay(GameMeaning.timingNewScenes);
            SceneManager.LoadScene(3);
        }
    }
}
