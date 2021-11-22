using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

#region RequireComponentBlock
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
#endregion
public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    #region Variables
    public static PlayerController inst;

    [SerializeField]
    Image healthbarImage;
    [SerializeField]
    TextMeshProUGUI timeGame;
    [SerializeField]
    GameObject ui;
    [SerializeField]
    GameObject cameraHolder;
    [SerializeField]
    GameObject cameraPricel;
    [SerializeField]
    GameObject loadingPanel;

    private bool gameNow;

    private bool pricelNow, isMine;

    private int weaponThisNow;

    private float mouseSensitivity, sprintSpeed, walkSpeed, quietSpeed, jumpForce, smoothTime;

    [SerializeField]
    Item[] items;

    Animator _anim;

    private int itemIndex;
    private int previousItemIndex = -1;
    private float verticalLookRotation;
    private bool grounded, damageCheckShoot, damageCheckFromHeight;

    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    PhotonView PV;

    public int ViewID;

    private float currentHealth;

    PlayerManager playerManager;

    public int TeamID;

    private float StepTimerDown, nowSpeed;

    private float quietStep;
    private float stepTimer;
    private float runSpeed;

    public float gromkost;
    private float value1;
    private float value2;

    [SerializeField]
    private GameObject raicast;

    private RaycastHit hit;
    public AudioSource source;

    [SerializeField]
    private AudioClip[] Beton;
    [SerializeField]
    private AudioClip[] Wood;
    [SerializeField]
    private AudioClip[] Metal;
    [SerializeField]
    private AudioClip[] Ground;
    #endregion

    #region Start and Update
    private void Awake()
    {
        inst = this;
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        _anim = GetComponent<Animator>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ViewID = PV.ViewID;

        mouseSensitivity = GameMeaning.MOUSESENSETIVITY;
        sprintSpeed = GameMeaning.SPINTSPEED;
        walkSpeed = GameMeaning.WALKSPEED;
        quietSpeed = GameMeaning.QUIETSPEED;
        jumpForce = GameMeaning.JUMPFORCE;
        smoothTime = GameMeaning.SMOOTHTIME;
        TeamID = GameMeaning.teamID;
        quietStep = GameMeaning.QUIETSTEP;
        stepTimer = GameMeaning.STEPTIMER;
        runSpeed = GameMeaning.RUNSPEED;
        gromkost = GameMeaning.VALUENORMAL;
        value1 = GameMeaning.VALUEGAME;
        value2 = GameMeaning.VALUESHIFT;

        if (TeamID == 2)
            currentHealth = GameMeaning.MAXHEALTHPLAYER;

        if (TeamID == 1)
            currentHealth = GameMeaning.MAXHEALTHENEMY;

        if (PV.IsMine)
            EquipItem(0);

        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }

        gameNow = true;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
        AnimationsController();
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

        if (!pricelNow)
            Look();
        else
            LookInPricel();

        if (gameNow)
        {
            CheckMines();
            Move();
            Jump();
            HeightCheck();
            LifeControls();
            SoundsControll();
            WeaponController();
        }
    }
    #endregion

    #region Interactions Player
    private void SoundsControll()
    {
        if (isMove && grounded)
        {
            if (StepTimerDown > 0)
                StepTimerDown -= Time.deltaTime;

            if (StepTimerDown < 0)
                StepTimerDown = 0;

            if (StepTimerDown == 0)
            {
                if (Physics.Raycast(raicast.transform.position, -Vector3.up, out hit, 6))
                {
                    if (hit.transform.tag == "Ground")
                        source.PlayOneShot(Ground[Random.Range(0, Ground.Length)], gromkost);
                    if (hit.transform.tag == "Beton")
                        source.PlayOneShot(Beton[Random.Range(0, Beton.Length)], gromkost);
                    if (hit.transform.tag == "Wood")
                        source.PlayOneShot(Wood[Random.Range(0, Wood.Length)], gromkost);
                    if (hit.transform.tag == "Metal" || hit.transform.name == "luk_podsobki")
                        source.PlayOneShot(Metal[Random.Range(0, Metal.Length)], gromkost);
                }
                StepTimerDown = stepTimer;
            }
        }
    }

    internal void HpPlus()
    {
        currentHealth += 50f;

        healthbarImage.fillAmount = currentHealth / GameMeaning.MAXHEALTHPLAYER;
        Debug.Log(" HP Plus " + currentHealth.ToString());
    }

    public void InteractionsOn(int weaponThis)
    {
        weaponThisNow = weaponThis;
    }

    private int damageCurrent, damageNow;
    private bool checkDamageOne, checkDamageTwo, checkDamageThree, checkDamageFour, checkDamageFive;

    private void HeightCheck()
    {
        if (Physics.Raycast(raicast.transform.position, -Vector3.up, out hit, 300))
        {
            if (hit.distance >= 6 && hit.distance <= 15 && !checkDamageOne)
            {
                damageCurrent += 20;
                checkDamageOne = true;
            }
            if (hit.distance >= 16 && hit.distance <= 25 && !checkDamageTwo)
            {
                damageCurrent += 30;
                checkDamageTwo = true;
            }
            if (hit.distance >= 26 && hit.distance <= 45 && !checkDamageThree)
            {
                damageCurrent += 50;
                checkDamageThree = true;
            }
            if (hit.distance >= 46 && hit.distance <= 65 && !checkDamageFour)
            {
                damageCurrent += 70;
                checkDamageFour = true;
            }
            if (hit.distance >= 66 && hit.distance <= 300 && !checkDamageFive)
            {
                damageCurrent += 79;
                checkDamageFive = true;
            }
            damageNow = damageCurrent;
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;

        if (!grounded)
        {
            checkDamageOne = checkDamageTwo = checkDamageThree = checkDamageFour = checkDamageFive = false;
            damageCheckFromHeight = false;
        }
        else
        {
            damageCheckFromHeight = true;
            CheckNow();
        }
    }

    private void CheckNow()
    {
        if (damageCheckFromHeight)
        {
            currentHealth -= damageNow;

            healthbarImage.fillAmount = currentHealth / GameMeaning.MAXHEALTHPLAYER;

            if (currentHealth <= 0)
                Die();
            
            damageCurrent = 0;
            damageNow = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "LootGun")
        {
            collision.gameObject.GetComponent<InteractionsObjects>().OpenObject();
            WeaponsControll(weaponThisNow);
        }
    }

    private void Look()
    {
        cameraPricel.SetActive(false);
        cameraHolder.SetActive(true);

        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -100f, 100f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void LookInPricel()
    {
        cameraHolder.SetActive(false);
        cameraPricel.SetActive(true);

        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -100f, 100f);

        cameraPricel.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void LifeControls()
    {
        if (transform.position.y < -100f)
            Die();
        
        if (transform.position.y > 300f)
            Die();
    }

    private async void Die()
    {
        SoundManager.inst.Die();
        AnimStop();
        UpdateBoleanDie(true);

        await Task.Delay(GameMeaning.DIETIME);
        playerManager.Die();
    }

    internal void LoadingNewScenesOn()
    {
        gameNow = false;
        loadingPanel.SetActive(true);
    }

    internal void LoadingNewScenesOff()
    {
        gameNow = true;
        loadingPanel.SetActive(false);
        AnimStop();
    }

    internal void DisablePlayer()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Moved Controlls
    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftControl) ? sprintSpeed : nowSpeed), ref smoothMoveVelocity, smoothTime);

        if (Input.GetKeyDown(KeyCode.W))
        {
            isIdel = false;
            isMove = true;
            stepTimer = 0.4f;
            gromkost = value1;
            nowSpeed = walkSpeed;
        }

        if (isMove && Input.GetKeyDown(KeyCode.LeftControl))
        {
            isRun = true;
            stepTimer = runSpeed;
            gromkost = value1;
            nowSpeed = walkSpeed;
        }

        if (isMove && Input.GetKeyUp(KeyCode.LeftControl))
        {
            isRun = false;
            stepTimer = 0.4f;
            gromkost = value1;
            nowSpeed = walkSpeed;
        }

        if (isMove && Input.GetKeyDown(KeyCode.LeftShift))
        {
            isQuiet = true;
            isRun = false;
            stepTimer = quietStep;
            gromkost = value2;
            nowSpeed = quietSpeed;
        }

        if (isMove && Input.GetKeyUp(KeyCode.LeftShift))
        {
            isMove = true;
            isQuiet = false;
            isRun = false;
            stepTimer = 0.4f;
            gromkost = value1;
            nowSpeed = walkSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
            isQuiet = false;

        if (Input.GetKeyUp(KeyCode.W))
        {
            isMove = false;
            isRun = false;
            isQuiet = false;
            gromkost = value1;
            nowSpeed = walkSpeed;
            isIdel = true;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
            isJump = true;
        }
    }

    public void JumpStop()
    {
        SoundManager.inst.JumpSound();
        isJump = false;
    }
    #endregion

    #region Weapon Item
    private void CheckMines()
    {
        if (itemIndex == 4)
            isMine = true;
        else
            isMine = false;
    }
    private void WeaponController()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
                EquipItem(0);
            else
                EquipItem(itemIndex + 1);
        }

        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
                EquipItem(items.Length - 1);
            else
                EquipItem(itemIndex - 1);
        }

        if (Input.GetMouseButtonDown(1))
            if (items[itemIndex].checkOnGun && !isMine)
                pricelNow = true;

        if (Input.GetMouseButtonUp(1))
            pricelNow = false;

        if (Input.GetMouseButtonDown(0) && pricelNow)
        {
            if (items[itemIndex].checkOnGun)
                items[itemIndex].UsePricel();
        }

        if (Input.GetMouseButtonDown(0) && !pricelNow)
        {
            if (items[itemIndex].checkOnGun && !isMine)
                items[itemIndex].Use();

            if (items[itemIndex].checkOnGun && isMine)
                items[itemIndex].InstMines();
        }
    }

    public void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;
       
        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
            items[previousItemIndex].itemGameObject.SetActive(false);

        previousItemIndex = itemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void WeaponsControll(int indexGun)
    {
        switch (indexGun)
        {
            case 0:
                items[0].OpenWeapon();
                EquipItem(0);
                break;
            case 1:
                items[1].OpenWeapon();
                EquipItem(1);
                break;
            case 2:
                items[2].OpenWeapon();
                EquipItem(2);
                break;
            case 3:
                items[3].OpenWeapon();
                EquipItem(3);
                break;
            case 4:
                items[4].OpenWeapon();
                EquipItem(4);
                break;
            case 5:
                items[5].OpenWeapon();
                EquipItem(5);
                break;
            case 6:
                items[6].OpenWeapon();
                EquipItem(6);
                break;
            case 7:
                items[7].OpenWeapon();
                EquipItem(7);
                break;
            case 8:
                items[8].OpenWeapon();
                EquipItem(8);
                break;
            default:
                items[0].OpenWeapon();
                EquipItem(0);
                break;
        }
    }
    #endregion

    #region RPC Commands
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
            EquipItem((int)changedProps["itemIndex"]);
    }

    public void MineActives()
    {
        PV.RPC("RPC_MineGo", RpcTarget.All);
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    private void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine)
            return;

        if (damageCheckShoot == true)
            currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / GameMeaning.MAXHEALTHPLAYER;

        if (currentHealth <= 0)
        {
            Debug.Log("DIE&&as 3");
            Die();
        }
    }

    public void Identifications(int teamId)
    {
        PV.RPC("RPC_TeamID", RpcTarget.All, teamId);
    }

    [PunRPC]
    private void RPC_TeamID(int teamId)
    {
        if (!PV.IsMine)
            return;

        if (teamId == TeamID)
            damageCheckShoot = false;
        else
            damageCheckShoot = true;
    }

    [PunRPC]
    private void RPC_MineGo()
    {
        if (!PV.IsMine)
            return;
    }
    #endregion

    #region Anim
    private bool isIdel, isMove, isRun, isQuiet;

    internal bool isJump;

    private void AnimationsController()
    {
        if (isIdel)
            UpsdateIdelBolean(true);
        else
            UpsdateIdelBolean(false);

        if (isMove)
            UpsdateMovingBolean(true);
        else
            UpsdateMovingBolean(false);

        if (isJump)
            UpdateBoleanJump(true);
        else
            UpdateBoleanJump(false);

        if (isRun)
            UpdateRunBolean(true);
        else
            UpdateRunBolean(false);

        if (isQuiet)
            UpdateQuietMoveBolean(true);
        else
            UpdateQuietMoveBolean(false);
    }

    private void UpsdateIdelBolean(bool idel)
    {
        _anim.SetBool("Idel", idel);
    }

    private void UpsdateMovingBolean(bool moving)
    {
        _anim.SetBool("Move", moving);
    }

    private void UpdateRunBolean(bool run)
    {
        _anim.SetBool("Run", run);
    }

    private void UpdateQuietMoveBolean(bool quiet)
    {
        _anim.SetBool("Quiet", quiet);
    }

    private void UpdateBoleanShoot(bool shoot)
    {
        _anim.SetBool("Shoot", shoot);
    }

    private void UpdateBoleanJump(bool jump)
    {
        _anim.SetBool("Jump", jump);
    }

    private void UpdateBoleanDie(bool die)
    {
        _anim.SetBool("Die", die);
    }

    private void AnimStop()
    {
        isMove = isRun = isQuiet = isJump = false;
        isIdel = true;
    }
    #endregion
}