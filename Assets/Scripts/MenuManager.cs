using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public static MenuManager inst;

	[SerializeField]
	private Menu[] menus;

	[SerializeField]
	private GameObject settingPanel;

	private void Awake()
    {
		inst = this;
    }

    public void OpenMenu(string menuName)
	{
		SoundManager.inst.PlayButton();

		for (int i = 0; i < menus.Length; i++)
		{
			if(menus[i].menuName == menuName)
			{
				menus[i].Open();
				SoundManager.inst.PlayButton();
			}
			else if(menus[i].open)
				CloseMenu(menus[i]);
		}
	}

	public void OpenMenu(Menu menu)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			if(menus[i].open)
			{
				SoundManager.inst.PlayButton();
				CloseMenu(menus[i]);
			}
		}
		menu.Open();
	}

	public void MenuScene()
    {
		SceneManager.LoadScene("Menu");
	}

	public void CloseMenu(Menu menu)
	{
		menu.Close();
	}

	public void OnMenu(Menu menu)
	{
		menu.Open();
	}

	public void SettingOpen()
    {
		settingPanel.SetActive(true);
    }

	public void ClousedSettings()
    {
		settingPanel.SetActive(false);
	}

	public void SinglePlayer()
	{
		SoundManager.inst.PlayButton();
		SceneManager.LoadScene("GameScene2");
	}

	public async void Quit()
    {
		SoundManager.inst.PlayButton();
		await Task.Delay(GameMeaning.QuitGame);
		Application.Quit();
    }
}