using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using MFPS.Audio;
using MFPS.Runtime.UI;
using MFPS.Runtime.FriendList;
using System.Threading.Tasks;
using TMPro;

public class bl_LobbyUI : MonoBehaviour
{
    public string MainWindowName = "room list";
    public List<WindowUI> windows = new List<WindowUI>();
    public List<PopUpWindows> popUpWindows = new List<PopUpWindows>();

    [Header("References")]
    public CanvasGroup FadeAlpha;
    public Text PlayerNameText = null;
    public Text PlayerCoinsText = null;
    public GameObject RoomInfoPrefab;
    public GameObject PhotonStaticticsUI;
    public GameObject PhotonGamePrefab;
    public GameObject EnterPasswordUI;
    public GameObject SeekingMatchUI;
    public GameObject DisconnectCauseUI;
    public Transform RoomListPanel;
    [SerializeField] private Text PasswordLogText = null;
    public Text LoadingScreenText;
    [SerializeField] private Text NoRoomText = null;
    public Image LevelIcon;
    public InputField PlayerNameField = null;
    [SerializeField] private Dropdown ServersDropdown = null;
    public CanvasGroup LoadingScreen;
    public bl_FriendListUI FriendUI;
    public GameObject[] OptionsWindows = null;
    public GameObject[] AddonsButtons;
    public LevelUI m_LevelUI;
    public bl_ConfirmationWindow confirmationWindow;
    public bl_CanvasGroupFader blackScreenFader;

    #region Private members
    private List<GameObject> CacheRoomList = new List<GameObject>();
    private Dictionary<string, RoomInfo> cachedRoomList;
    private string currentWindow = "";
    #endregion

    /// <summary>
    /// 
    /// </summary>
    public void InitialSetup()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        bl_EventHandler.onGameSettingsChange += ApplyRuntimeSettings;
#if LOCALIZATION
        bl_Localization.Instance.OnLanguageChange += OnLanguageChange;
#endif
        windows.ForEach(x => { if (x.UIRoot != null) { x.UIRoot.SetActive(false); } });//disable all windows
        if (PhotonStaticticsUI != null) { PhotonStaticticsUI.SetActive(bl_Lobby.Instance.ShowPhotonStatistics); }
        RoomInfoPrefab.SetActive(false);
        blackScreenFader.SetAlpha(1);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
#if ULSP
        if (bl_DataBase.IsUserLogged) bl_DataBase.OnUpdateData += OnUpdateDataBaseInfo;
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDisable()
    {
        bl_EventHandler.onGameSettingsChange -= ApplyRuntimeSettings;
#if LOCALIZATION
        bl_Localization.Instance.OnLanguageChange -= OnLanguageChange;
#endif
#if ULSP
        if (bl_DataBase.IsUserLogged) bl_DataBase.OnUpdateData -= OnUpdateDataBaseInfo;
#endif
    }

    /// <summary>
    /// display all available rooms
    /// </summary>
    public void SetRoomList(List<RoomInfo> rooms)
    {
        //Removed old list
        if (CacheRoomList.Count > 0)
        {
            CacheRoomList.ForEach(x => Destroy(x));
            CacheRoomList.Clear();
        }
        UpdateCachedRoomList(rooms);
        InstanceRoomList();
    }

    /// <summary>
    /// 
    /// </summary>
    void InstanceRoomList()
    {
        if (cachedRoomList.Count > 0)
        {
            NoRoomText.text = string.Empty;
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                if (info.Name == bl_Lobby.Instance.justCreatedRoomName) continue;

                GameObject entry = Instantiate(RoomInfoPrefab);
                entry.SetActive(true);
                entry.transform.SetParent(RoomListPanel, false);
                entry.GetComponent<bl_RoomInfo>().GetInfo(info);
                CacheRoomList.Add(entry);
            }

        }
        else
        {
#if LOCALIZATION
            NoRoomText.text = bl_Localization.Instance.GetText("norooms");
#else

            NoRoomText.text = bl_GameTexts.NoRoomsCreated;
#endif
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }
                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangeWindow(string window)
    {
        if (window == currentWindow) return;//return if we are trying to open the opened window
        WindowUI w = windows.Find(x => x.Name == window);
        if (w == null) return;//the window with that windowName doesn't exist

        StopCoroutine("DoChangeWindow");
        StartCoroutine("DoChangeWindow", w);
        currentWindow = window;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetEnableWindow(string windowName, bool active)
    {
        WindowUI w = windows.Find(x => x.Name == windowName);
        if (w == null || w.UIRoot == null) return;//the window with that windowName doesn't exist

        w.UIRoot.SetActive(active);
        if (w.MenuButton != null) w.MenuButton.interactable = !active;
    }

    public void Home() { ChangeWindow(MainWindowName); bl_Lobby.Instance.onShowMenu?.Invoke(); }
    public void HideAll() 
    { 
        currentWindow = ""; windows.ForEach(x => { if (x.UIRoot != null) { x.UIRoot.SetActive(false); } }); 
    }//disable all windows 

    /// <summary>
    /// 
    /// </summary>
    IEnumerator DoChangeWindow(WindowUI window)
    {
        //now change the windows
        for (int i = 0; i < windows.Count; i++)
        {
            WindowUI w = windows[i];
            if (w.UIRoot == null) continue;

            if (w.isPersistent)
            {
                w.UIRoot.SetActive(!window.hidePersistents);
            }
            else
            {
                if (w.MenuButton != null) w.MenuButton.interactable = true;
                w.UIRoot.SetActive(false);
            }
            window.UIRoot.SetActive(true);
            if (window.MenuButton != null) window.MenuButton.interactable = false;
            if (window.showTopMenu)
            {
                SetEnableWindow("top menu", true);
            }
        }

        if (window.playFadeInOut)
        {
            float d = 0;
            while (d < 1)
            {
                d += Time.deltaTime * 2;
                FadeAlpha.alpha = d;
                yield return null;
            }
        }
    }

    public void ChangeOptionsWindow(int id)
    {
        foreach (GameObject g in OptionsWindows) { g.SetActive(false); }
        OptionsWindows[id].SetActive(true);
    }

    public void ShowPopUpWindow(string popUpName)
    {
        PopUpWindows w = popUpWindows.Find(x => x.Name == popUpName);
        if (w == null || w.Window == null) return;

        w.Window.SetActive(true);
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected && bl_GameData.isDataCached)
        {
            PlayerNameText.text = PhotonNetwork.NickName;
        }
    }

    public void SignOut()
    {
        bl_Lobby.Instance.SignOut();
    }

    #region Settings
    public void LoadSettings()
    {
        ApplyRuntimeSettings();

        bool km = PlayerPrefs.GetInt(PropertiesKeys.KickKey, 0) == 1;
        if (km) { ShowPopUpWindow("kicked"); }
        if (bl_PhotonNetwork.Instance.hasPingKick) { ShowPopUpWindow("ping kick"); bl_PhotonNetwork.Instance.hasPingKick = false; }
        PlayerPrefs.SetInt(PropertiesKeys.KickKey, 0);
        bl_Lobby.Instance.rememberMe = !string.IsNullOrEmpty(PlayerPrefs.GetString(PropertiesKeys.RememberMe, string.Empty));
        if (bl_PhotonNetwork.Instance.hasAFKKick) { ShowPopUpWindow("afk kick"); bl_PhotonNetwork.Instance.hasAFKKick = false; }
#if INPUT_MANAGER
        bl_Input.Initialize();
        bl_Input.CheckGamePadRequired();
#endif
#if LM
        bl_LevelManager.Instance.Initialize();
        if (bl_LevelManager.Instance.isNewLevel)
        {
            var info = bl_LevelManager.Instance.GetLevel();
            m_LevelUI.Icon.sprite = info.Icon;
            m_LevelUI.LevelNameText.text = info.Name;
            m_LevelUI.Root.SetActive(true);
            bl_LevelManager.Instance.Refresh();
        }
        bl_LevelManager.Instance.GetInfo();
#endif
#if ULSP
        if (bl_DataBase.Instance != null && bl_DataBase.Instance.isLogged)
        {
            bl_GameData.Instance.VirtualCoins.UserCoins = bl_DataBase.Instance.LocalUser.Coins;
        }
#endif
#if CUSTOMIZER
        AddonsButtons[2].SetActive(true);
#endif
    }

    void ApplyRuntimeSettings()
    {
        if (bl_MFPS.Settings != null)
        {
            bl_MFPS.Settings.ApplySettings(true, false);
            Application.targetFrameRate = bl_MFPS.Settings.RefreshRates[(int)bl_MFPS.Settings.GetSettingOf("Frame Rate")];
            bl_MFPS.MusicVolume = (float)bl_MFPS.Settings.GetSettingOf("Music Volume");
            bl_AudioController.Instance.ForceStopAllFades();
            bl_AudioController.Instance.BackgroundVolume = bl_MFPS.MusicVolume;
            bl_AudioController.Instance.MaxBackgroundVolume = bl_MFPS.MusicVolume;
        }
    }

    public void FullSetUp()
    {
        List<Dropdown.OptionData> od = new List<Dropdown.OptionData>();
        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            data.text = QualitySettings.names[i].ToUpper();
            od.Add(data);
        }

#if LM
        LevelIcon.gameObject.SetActive(true);
        var pli = bl_LevelManager.Instance.GetLevel();
        LevelIcon.sprite = pli.Icon;
        Text plt = LevelIcon.GetComponentInChildren<Text>();
        if (plt != null) plt.text = pli.LevelID.ToString();
#else
        LevelIcon.gameObject.SetActive(false);
#endif
#if ULSP && CLANS
        AddonsButtons[7].SetActive(true);
#endif
#if SHOP
        AddonsButtons[9].SetActive(true);
#endif
#if PSELECTOR
        AddonsButtons[10].SetActive(!bl_PlayerSelector.InMatch);
#endif
        AddonsButtons[0].SetActive(false);
#if CLASS_CUSTOMIZER
        AddonsButtons[0].SetActive(true);
#endif
        SetRegionDropdown();
    }

    void OnLanguageChange(Dictionary<string, string> lang)
    {
#if LOCALIZATION
        NoRoomText.text = bl_Localization.Instance.GetText("norooms");
        bl_LobbyRoomCreatorUI.Instance.SetupSelectors();
#endif
    }

    public void ResetSettings()
    {
        LoadSettings();
        FullSetUp();
    }

    void SetRegionDropdown()
    {
        //when Photon Server is used
        if (!PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer)
        {
            //disable the dropdown server selection since it doesn't work with Photon Server.
            ServersDropdown.gameObject.SetActive(false);
            return;
        }
        string key = PlayerPrefs.GetString(PropertiesKeys.PreferredRegion, bl_Lobby.Instance.DefaultServer.ToString());
        string[] Regions = Enum.GetNames(typeof(SeverRegionCode));
        for (int i = 0; i < Regions.Length; i++)
        {
            if (key == Regions[i])
            {
                int id = i;
                if (id > 4) { id--; }
                ServersDropdown.value = id;
                break;
            }
        }
        ServersDropdown.RefreshShownValue();
    }
    #endregion

    #region UI Callbacks
    public void EnterName(InputField field = null)
    {
        if (field == null || string.IsNullOrEmpty(field.text))
            return;

        int check = bl_GameData.Instance.CheckPlayerName(field.text);
        if (check == 1)
        {
            bl_Lobby.Instance.CachePlayerName = field.text;
            SetEnableWindow("user password", true);
            return;
        }
        else if (check == 2)
        {
            field.text = string.Empty;
            return;
        }
        bl_Lobby.Instance.SetPlayerName(field.text);
#if !ULSP
        PlayerCoinsText.text = bl_GameData.Instance.VirtualCoins.UserCoins.ToString();
#endif
    }

    public void EnterPassword(InputField field = null)
    {
        if (field == null || string.IsNullOrEmpty(field.text))
            return;

        string pass = field.text;
        if (!bl_Lobby.Instance.EnterPassword(pass))
        {
            field.text = string.Empty;
        }
    }

    public void CheckRoomPassword(RoomInfo room)
    {
        EnterPasswordUI.SetActive(true);
        bl_Lobby.Instance.CheckRoomPassword(room);
    }

    public void OnEnterPassworld(InputField pass)
    {
        if (!bl_Lobby.Instance.SetRoomPassworld(pass.text))
        {
            PasswordLogText.text = "Wrong room password";
        }
    }

    public void ChangeServerCloud(int id)
    {
        bl_Lobby.Instance.ChangeServerCloud(id);
    }

    public void LoadLocalLevel(string level)
    {
        bl_Lobby.Instance.LoadLocalLevel(level);
    }

    public void UpdateCoinsText()
    {
#if ULSP
        if (bl_DataBase.Instance != null && !bl_DataBase.Instance.isGuest)
        {
            PlayerCoinsText.text = bl_DataBase.Instance.LocalUser.Coins.ToString();
        }
#else
            bl_GameData.Instance.VirtualCoins.LoadCoins(PhotonNetwork.NickName);
            PlayerCoinsText.text = bl_GameData.Instance.VirtualCoins.UserCoins.ToString();
#endif
    }

    public void ShowBuyCoins()
    {
#if SHOP
        ChangeWindow("shop");
        bl_ShopManager.Instance.BuyCoinsWindow.SetActive(true);
#else
        Debug.Log("Require shop addon.");
#endif
    }

#if ULSP
    void OnUpdateDataBaseInfo(MFPS.ULogin.LoginUserInfo info)
    {
        UpdateCoinsText();
    }
#endif

    public void Quit()
    {
        confirmationWindow.AskConfirmation(bl_GameTexts.QuitGameConfirmation, () =>
         {
             Application.Quit();
             Debug.Log("Game quit only work on the game build.");
         });
    }

    public void AutoMatch()
    {
        bl_Lobby.Instance.AutoMatch(); // сделать автоматч 

    }

    [SerializeField]
    TMP_InputField roomNameInputField; //add name Maps

    public void CreateRoom()
    { 
        bl_Lobby.Instance.CreateRoom();


        //if (string.IsNullOrEmpty(roomNameInputField.text))
        //    return;

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.inst.OpenMenu("loading");
        //SoundManager.inst.PlayButton();
    }

    public async void StartGame()
    {
        сделать выбор режимов
        //if (GameMeaning.teamID == 1 || GameMeaning.teamID == 2)
        //{
            //SoundManager.inst.PlayButton();
            await Task.Delay(GameMeaning.TIMINGLOADGAMESTART);
            PhotonNetwork.LoadLevel(GameMeaning.SCENEFIRST); // my game scene
            Cursor.visible = false;
        //}
        //else
        //    panelControlsTeams.SetActive(true);
    }

    public void SetRememberMe(bool value)
    { 
        bl_Lobby.Instance.SetRememberMe(value); 
    }

    public void RoomTwo()
    {
        Debug.Log("2 Room");
    }
    public void Room3()
    {
        Debug.Log("3 Room");
    }
    public void Room4()
    {
        Debug.Log("4 Room");
    }
    public void Room5()
    {
        Debug.Log("5 Room");
    }
    public void Room6()
    {
        Debug.Log("6 Room");
    }
    #endregion

    #region Classes
    [System.Serializable]
    public class WindowUI
    {
        public string Name;
        public GameObject UIRoot;
        public Button MenuButton;

        public bool isPersistent = false;//this window will stay show up when change window?
        public bool hidePersistents = false;//force hide persistent windows
        public bool playFadeInOut = false;
        public bool showTopMenu = true;
    }

    [System.Serializable]
    public class PopUpWindows
    {
        public string Name;
        public GameObject Window;
    }

    [System.Serializable]
    public class LevelUI
    {
        public GameObject Root;
        public Image Icon;
        public Text LevelNameText;
    }

    private static bl_LobbyUI _instance;
    public static bl_LobbyUI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<bl_LobbyUI>();
            }
            return _instance;
        }
    }
    #endregion
}