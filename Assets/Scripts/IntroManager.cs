using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private async void Awake()
    {
        await Task.Delay(GameMeaning.INTROTIMELOAD);
        MenuScene();
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
