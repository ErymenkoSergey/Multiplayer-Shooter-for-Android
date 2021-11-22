using UnityEngine;

public class Menu : MonoBehaviour
{
	public string menuName;
	public bool open;

    public void Open()
    {
        SoundManager.inst.PlayButton();
        open = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        SoundManager.inst.PlayButton();
        open = false;
        gameObject.SetActive(false);
    }
}