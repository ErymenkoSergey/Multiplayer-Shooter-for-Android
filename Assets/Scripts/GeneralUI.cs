using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    public static GeneralUI inst;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPannel;

    [Header("UI")]
    [SerializeField] GameObject soundsOn;
    [SerializeField] GameObject soundsOff;
    [SerializeField] GameObject vibrateOn;
    [SerializeField] GameObject vibrateOff;
    [SerializeField] GameObject musicOn;
    [SerializeField] GameObject musicOff;

    [SerializeField] Toggle toggleFps; 

    private void Awake()
    {
        if (inst)
            Destroy(gameObject);
        else
            inst = this;
    }

    private void Start()
    {
        toggleFps.onValueChanged.AddListener((value) => FpsOn(value));
    }

    private void Update()
    {
        UpdateSoundsUI();
        UpdateMusiconUI();
    }

    private void FpsOn(bool on)
    {
        if (on)
            GameMeaning.FPSUI = true;
        else
            GameMeaning.FPSUI = false;
    }

    public void ButtonSoundOnOff()
    {
        SoundManager.inst.PlayButton();

        if (GameManager.inst.SoundSettings)
            GameManager.inst.SoundSettings = false;
        else
            GameManager.inst.SoundSettings = true;
    }

    public void MusicSoundOnOff()
    {
        SoundManager.inst.PlayButton();

        if (GameManager.inst.MusicSettings)
        {
            GameManager.inst.MusicSettings = false;
            VolumeValue.inst.PlayMusic();
        }
        else
        {
            GameManager.inst.MusicSettings = true;
            VolumeValue.inst.StopMusic();
        }
    }

    public void OpenSettings()
    {
        settingsPannel.SetActive(true);
        SoundManager.inst.PlayButton();
    }

    public void CloseSettings()
    {
        settingsPannel.SetActive(false);
        SoundManager.inst.PlayButton();
    }

    private void UpdateSoundsUI()
    {
        if (GameManager.inst.SoundSettings)
        {
            soundsOn.SetActive(true);
            soundsOff.SetActive(false);
        }
        else
        {
            soundsOn.SetActive(false);
            soundsOff.SetActive(true);
        }
    }

    private void UpdateMusiconUI()
    {
        if (GameManager.inst.MusicSettings)
        {
            musicOn.SetActive(true);
            musicOff.SetActive(false);
        }
        else
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
        }
    }
}
