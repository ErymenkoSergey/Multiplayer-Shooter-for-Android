using System.Threading.Tasks;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController inst;

    [SerializeField]
    private GameObject pausePanel;

    private PlayerController player;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        FindPlayersScene();
    }

    private async void FindPlayersScene()
    {
        await Task.Delay(GameMeaning.FINDPLAYERFORSCENE);
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        ESCButton();
    }

    private void ESCButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            pausePanel.SetActive(true);
            SoundManager.inst.PlayButton();
            if (player != null)
            {
                player.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
                player.gameObject.GetComponent<PlayerController>().enabled = false;
            }
        }
    }

    public void ContinueGame()
    {
        Cursor.visible = false;
        if (player != null)
        {
            player.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            player.gameObject.GetComponent<PlayerController>().enabled = true;
        }
    }
}
