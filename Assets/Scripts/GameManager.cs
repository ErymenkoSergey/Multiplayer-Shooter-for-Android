using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    private void Awake()
    {
        inst = this;
    }

    #region Sound Settings
    private const string SOUND_SETTINGS = "SoundSettings";
    private const string MUSIC_SETTINGS = "MusicSettings";

    public bool SoundSettings
    {
        get => PlayerPrefs.GetInt(SOUND_SETTINGS, 1) == 1;
        set => PlayerPrefs.SetInt(SOUND_SETTINGS, value ? 1 : 0);
    }

    public bool MusicSettings
    {
        get => PlayerPrefs.GetInt(MUSIC_SETTINGS, 1) == 1;
        set => PlayerPrefs.SetInt(MUSIC_SETTINGS, value ? 1 : 0);
    }
    #endregion
}

public static class GameMeaning
{
    #region Build Settings
    public const int INTRO = 0; // id сцены в билд сеттингс)
    public const int MENU = 2;
    public const int SCENEFIRST = 1; 
    public const int SCENESECOND = 2; // not
    public const int SCENETHIRD = 3; // not
    #endregion

    #region Intro
    public const int INTROTIMELOAD = 1100; //milliseq
    #endregion

    #region Menu
    public static bool hasPickedTeam; 
    public static int teamID;
    public static int timingNewScenes = 1000;
    public static int QuitGame = 120;

    #endregion

    #region GameSettings
    internal static bool FPSUI;
    #endregion

    #region Game Process (in Scene)
    public const float TIMEGAME = 480000f; //Время раунда 8 минут  

    public const int FINDPLAYERFORSCENE = 1000; //milliseq, задержка при поключении к игре

    public const int TIMINGLOADGAMESTART = 1100; // ОЖИДАНИЕ ЗАГРУЗКИ УРОВНЯ В milliseq
    public const int TIMINGLEAVEROOM = 800; // ОЖИДАНИЕ ЗАГРУЗКИ УРОВНЯ В milliseq
    public const int TIMINGLEAVEGAME = 700; // ОЖИДАНИЕ ЗАГРУЗКИ УРОВНЯ В milliseq. ждем анимацию и идем в меню

    public const float MAXHEALTHPLAYER = 250; // хп ИГРОКА
    public const float MAXHEALTHENEMY = 10000; // ХП МАНЬЯКА

    public static float destroyGameInteractions = 0.2f; // время уничтожения объекта на карте 
    #endregion

    #region Loot
    public static bool INTERACTIONSLOOT; //возможность открыть ящик в игре.
    public static bool CONNECTLOOT; 

    public static int GUNZERO = 0; // порядковый номер орудия для игрока
    public static int GUNFIRST = 1;  
    public static int GUNSECOND = 2;
    public static int GUNTHIRD = 3;
    public static int GUNFOURTH = 4;
    public static int GUNFIFTH = 5;
    public static int GUNSIXTH = 6;
    public static int GUNSEVENTH = 7;
    public static int GUNEIGHTH = 8;
    public static int GUNNINTH = 9;
    public static int GUNTENTH = 10;
    #endregion

    #region Controlls
    public const float MOUSESENSETIVITY = 3F; // Сенсор мыши - чувствительность

    public const float SPINTSPEED = 6F;
    public const float WALKSPEED = 3F;
    public const float QUIETSPEED = 1.0F;
    public const float JUMPFORCE = 400F;
    public const float SMOOTHTIME = 0.15F;

    public const float QUIETSTEP = 1.4F; //скорость звука на шифте 
    public const float STEPTIMER = 0.6F; //скорость звука шага
    public const float RUNSPEED = 0.3F; //скорость звука бега
    public const float VALUENORMAL = 1F; //громкость (максимальное значение = 1,0)
    public const float VALUEGAME = 0.1F; // на норме
    public const float VALUESHIFT = 0.001F; // на шифте
    #endregion

    #region Gun
    public const float BULLETDRAWINGTIME = 20f; //sec , время отрисовки пули на объекте
    #endregion

    #region  Animations TIMING sync
    public const int DIETIME = 1000; //milliseq
    #endregion
}
