using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionNewScenesTwo : MonoBehaviour
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
            playerController.transform.position = new Vector3 (-22.7f, 15.53f, 2.06f);
            SceneManager.LoadScene(4);
        }
    }
}
