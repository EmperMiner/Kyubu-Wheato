using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public int maxHealth;
    public int playerHealth;
    public float strength;
    public float defense;
    public float wheatDroprate;
    public int Wheat;
    public int diceDroprate;
    public bool haveGarlicBread;

    public int diceNumber;
    public float playerCooldownTime;
    public int dicePreviewerLevel;
    public bool havePizza;
    public bool haveCarrotCake;
    public bool haveFlan;
    public bool haveCremeBrulee;
    public bool haveBanhmi;
    public bool haveCupcake;
    public bool haveChickenNuggets;
    public bool havePastelDeChoclo;
    public bool haveHornScallop;

    public bool playerAlive;

    public int CurrentMode;
    private bool triggeredBroom = false;
    private bool BroomInMode = false;
    public bool InHealMode = false;
    private bool RegenStop = true;
    
    private AudioManager AudioPlayer;
    private Transform mainCam;
    [SerializeField] private bool FirstLevelSave;
    [SerializeField] private float[] XSpawnpoints;
    [SerializeField] private float[] YSpawnpoints;
    [SerializeField] private float LeftCamLimit;
    [SerializeField] private float RightCamLimit;
    [SerializeField] private float UpperCamLimit;
    [SerializeField] private float LowerCamLimit;

    public float LeftMapLimit;
    public float RightMapLimit;
    public float UpperMapLimit;
    public float LowerMapLimit;

    [SerializeField] private Rigidbody2D playerRB ;
    private Vector2 movement;
    [SerializeField] private Animator animator;
    public Renderer spriteRenderer;
    public HealthBar healthBar;
    private DiceThrow diceThrowScript;
    [SerializeField] private DicePadSpawner dicePadSpawnerScript;
    private UltimateBarCharge ultimateScript;
    private CooldownBar cooldownBarScript;
    private Text DiceCounterNumber;
    private Text WheatCounterNumber;
    private GameOverScreen gameOverScript;
    private GameOverScreen victoryScreenScript;
    private ExitHoeContainer KeyWheatScript;

    private TextMeshProUGUI SmallText;
    private GameObject crosshair;
    private GameObject diceThrower;
    
    [SerializeField] private GameObject[] BroomPrefabs;
    private bool HasStrengthTemporaryBuff;
    private bool HasDefenseTemporaryBuff;
    private bool HasSpeedTemporaryBuff;
    public bool AllEntrees = false;
    private Image BroomBuffImage;
    [SerializeField] private Sprite[] BroomBuffIcons;
    
    Collider2D other;
    string requiredWheat;

    private GameObject MapDisplay;
    private GameObject RedWheatIndicator;
    private GameObject BlueWheatIndicator;
    private GameObject GreenWheatIndicator;
    private TextMeshProUGUI LevelText;
    private TextMeshProUGUI LevelTextMap;

    private bool inCooldown = false;
    private float cooldownTimer;
    private float healingCooldownTime;
    private int healWheatCost;
    private bool SalmonStarted;
    public bool Invincible;
    [SerializeField] private GameObject SalmonRiser;

    private bool FSCInvincible;
    [SerializeField] private GameObject MyriadCookies;
    [SerializeField] private GameObject Whale;
    [SerializeField] private GameObject ChanceWhale;
    [SerializeField] private float[] WhaleLevelInterval;
    private bool lowHealth;
    [SerializeField] private GameObject[] flippedEnemyPrefabs;
    [SerializeField] private Image whaleJumpscare;
    [SerializeField] private GameObject WaterContainer;
    [SerializeField] private GameObject ghost;
    [SerializeField] private TextMeshProUGUI SanesssIntroText;
    [SerializeField] private GameObject SanesssPrefab;

    private GameObject transitionOut;
    private SpeedrunTimer timerScript;

    public bool playerAttacked = false;
    private float playerAttackedTimer = 0f;
    private float playerAttackedGracePeriod = 1f;

    public float distancePerSecond = 0;
    private Vector3 previousLoc;

    private void Awake()
    {       
        SalmonStarted = false;
        Invincible = false;
        SalmonRiser.SetActive(false);
        
        int level = SceneManager.GetActiveScene().buildIndex - 4;
        SmallText = GameObject.FindGameObjectWithTag("IngameNotifText").GetComponent<TextMeshProUGUI>();
        SmallText.text = "";
        KeyWheatScript = GameObject.FindGameObjectWithTag("ExitHoeContainer").GetComponent<ExitHoeContainer>();

        MapDisplay = GameObject.Find("Map");
        RedWheatIndicator = GameObject.Find("RedWheatIndicator");
        BlueWheatIndicator = GameObject.Find("BlueWheatIndicator");
        GreenWheatIndicator = GameObject.Find("GreenWheatIndicator");
        LevelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
        if (SceneManager.GetActiveScene().buildIndex != 15) { LevelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex - 3); }
        else { LevelText.text = "Level AK_-B(N*#23K+CS!@d"; }

        LevelTextMap = GameObject.Find("LevelTextMap").GetComponent<TextMeshProUGUI>();
        if (SceneManager.GetActiveScene().buildIndex != 15) { LevelTextMap.text = "Level " + (SceneManager.GetActiveScene().buildIndex - 3); }
        else { LevelTextMap.text = "Level (#93MS8*-D%LD_-LQ"; }

        timerScript = GameObject.FindGameObjectWithTag("Timer").GetComponent<SpeedrunTimer>();
        AudioPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        diceThrower = GameObject.FindGameObjectWithTag("DiceManager");
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        diceThrowScript = GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceThrow>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        ultimateScript = GameObject.FindGameObjectWithTag("Ultimate Bar").GetComponent<UltimateBarCharge>();
        cooldownBarScript = GameObject.FindGameObjectWithTag("CooldownBar").GetComponent<CooldownBar>();
        DiceCounterNumber = GameObject.FindGameObjectWithTag("DiceCounter").GetComponent<Text>();
        WheatCounterNumber = GameObject.FindGameObjectWithTag("WheatCounter").GetComponent<Text>();
        gameOverScript = GameObject.FindGameObjectWithTag("GameOverScreen").GetComponent<GameOverScreen>();
        try { victoryScreenScript = GameObject.FindGameObjectWithTag("GameOverScreen").GetComponent<GameOverScreen>(); }
        catch (NullReferenceException) { Debug.Log("Not Level 11"); }

        BroomBuffImage = GameObject.FindGameObjectWithTag("BroomBuffImage").GetComponent<Image>();

        BroomBuffImage.color = new Color32(255, 255, 255, 0);

        if (FirstLevelSave) 
        { 
            PlayerPrefs.SetInt("Crack", 0);
            ultimateScript.currentUltimateCharge = 0;
            PlayerPrefs.SetInt("PreviousCrack", 0);
            PlayerPrefs.SetInt("UltCharge", 0);
            PlayerPrefs.SetInt("ChargedAttacks", 0);
            PlayerPrefs.SetInt("IngameRamen", 0);
            PlayerPrefs.SetInt("IngameSalmon", 0);
            PlayerPrefs.SetInt("IngameSteak", 0);
            PlayerPrefs.SetInt("IngameCheese", 0);
            PlayerPrefs.SetInt("IngameFSC", 0);
            PlayerPrefs.SetInt("IngameInfinitruths", 0);
            if (PlayerPrefs.GetInt("Ramen") == 1) { PlayerPrefs.SetInt("IngameRamen", 1); }
            if (PlayerPrefs.GetInt("Salmon") == 1) { PlayerPrefs.SetInt("IngameSalmon", 1); }
            if (PlayerPrefs.GetInt("Steak") == 1) { PlayerPrefs.SetInt("IngameSteak", 1); }
            if (PlayerPrefs.GetInt("Cheese") == 1) { PlayerPrefs.SetInt("IngameCheese", 1); }
            if (PlayerPrefs.GetInt("FSC") == 1) { PlayerPrefs.SetInt("IngameFSC", 3); }
            if (PlayerPrefs.GetInt("Infinitruths") == 2) { PlayerPrefs.SetInt("IngameInfinitruths", 1); }
            PlayerPrefs.SetFloat("DiceSpinLevel", 0f);
            PlayerPrefs.SetFloat("DiceSpinLevelUp", 1f);
            Wheat = 0;
            LoadData();
            FirstIngameSaveData(); 
            IngameLoadData();
        }
        else
        {
            SubtractTemporaryBuff();
            LoadLastSaved();
            UpdateValues();
        }
        
        WheatCounterNumber.text = Wheat.ToString();

        if (XSpawnpoints.Length > 0)
        {
            int ChooseSpawn = UnityEngine.Random.Range(0, XSpawnpoints.Length);
            transform.position = new Vector3(XSpawnpoints[ChooseSpawn], YSpawnpoints[ChooseSpawn] , -2);
        }
        
        Time.timeScale = 1f;
        playerAlive = true;
        playerHealth = maxHealth;
        crosshair.SetActive(true);
        diceThrower.SetActive(true);
        healthBar.SetMaxHealth(maxHealth);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform; 
        LeftMapLimit = LeftCamLimit - 7.5f;
        RightMapLimit = RightCamLimit + 7.5f;
        UpperMapLimit = UpperCamLimit + 3f;
        LowerMapLimit = LowerCamLimit - 3f;

        healingCooldownTime = 30f;
        lowHealth = false;

        transitionOut = GameObject.FindGameObjectWithTag("TransitionOut");
        transitionOut.SetActive(false);

        if (level == 11) { StartCoroutine(DevilishWhaleChance()); }
        else if (level == 0 || level == 1 ) { }
        else
        {
            StartCoroutine(WhaleChance());
            StartCoroutine(WhaleForced());
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 15) { PlayerPrefs.SetInt("SanesssStatus", 0); StartCoroutine(SanesssIntro()); }
        PlayerPrefs.SetInt("PlayerStopped", 0);
        Invincible = false;
        RedWheatIndicator.SetActive(false);
        BlueWheatIndicator.SetActive(false);
        GreenWheatIndicator.SetActive(false);
        MapDisplay.SetActive(false);
        StartCoroutine(CampChecking());
    }

    private void Update()
    {
        if (playerAlive)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Time.timeScale == 1f)
            {
                if ( movement.x!=0 ) { animator.SetFloat("Horizontal", movement.x); }
                animator.SetFloat("Speed", movement.sqrMagnitude);
            }
                

            if (Input.GetKeyDown(KeyCode.R) && inCooldown == false) { CraftBread(); }
            else if (Input.GetKeyDown(KeyCode.R)) { StartCoroutine(CantCraftBread(Mathf.RoundToInt(healingCooldownTime - cooldownTimer))); }

            if (Input.GetKeyDown(KeyCode.Tab)) 
            { 
                if (MapDisplay.activeSelf == false) { MapDisplay.SetActive(true); }
                else { MapDisplay.SetActive(false); }
            }

            if (haveGarlicBread && triggeredBroom == false) { StartCoroutine(RollMode());}
            if (CurrentMode == 1 && BroomInMode == false) { StartCoroutine(AttackMode());}
            if (CurrentMode == 2 && BroomInMode == false) { StartCoroutine(HealMode());}
            if (CurrentMode == 3 && BroomInMode == false) { StartCoroutine(StatBuffMode());}
            
            if (diceThrowScript.inKyubuKombo100) { UpdateHealth(maxHealth); }
            if (havePizza && haveCarrotCake && haveFlan && haveCremeBrulee && haveBanhmi && haveCupcake && haveChickenNuggets && havePastelDeChoclo && haveGarlicBread) { AllEntrees = true; }
            else { AllEntrees = false; }

            if (diceThrowScript.isOnDiceTile1 && !other) { diceThrowScript.isOnDiceTile1 = false; }
            if (diceThrowScript.isOnDiceTile2 && !other) { diceThrowScript.isOnDiceTile2 = false; }
            if (diceThrowScript.isOnDiceTile3 && !other) { diceThrowScript.isOnDiceTile3 = false; }
            if (diceThrowScript.isOnDiceTile4 && !other) { diceThrowScript.isOnDiceTile4 = false; }
            if (diceThrowScript.isOnDiceTile5 && !other) { diceThrowScript.isOnDiceTile5 = false; }
            if (diceThrowScript.isOnDiceTile6 && !other) { diceThrowScript.isOnDiceTile6 = false; }

            if (PlayerPrefs.GetInt("IngameSalmon") == 1 && SalmonStarted == false) { StartCoroutine(InvincibleWaves()); }
            if (Invincible) { healthBar.HealthBarFlash(2); }

            if (inCooldown)
            {    
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer > healingCooldownTime)
                {
                    inCooldown = false;
                    cooldownTimer = 0;
                }
            }

            if (playerAttacked)
            {    
                playerAttackedTimer += Time.deltaTime;
                if (playerAttackedTimer > playerAttackedGracePeriod)
                {
                    playerAttacked = false;
                    playerAttackedTimer = 0f;
                }
            }

            if ((float)playerHealth <= maxHealth*0.3f && lowHealth == false) { Gasp(); }

            if(GameObject.FindGameObjectsWithTag("Star").Length > 25) 
            {
                Destroy(GameObject.FindGameObjectWithTag("Star"));
            }
            if ( FSCInvincible == true) { Invincible = true; }

            if (PlayerPrefs.GetInt("BossDefeated") == 0)
            {
                distancePerSecond += Vector3.Distance(transform.position, previousLoc);
                previousLoc = transform.position;
            }

            if (Input.GetKeyDown(KeyCode.Z) && PlayerPrefs.GetInt("IngameInfinitruths") == 1)
            {
                Instantiate(MyriadCookies, transform.position, Quaternion.identity);
                AudioPlayer.PlaySound("FSC");
            }

            if (playerHealth == 0 && PlayerPrefs.GetInt("IngameFSC") > 0) { StartCoroutine(FSC()); }
            else if (playerHealth == 0) { GameOver(); }
        }
    }

    private IEnumerator SanesssIntro()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(370f,430f));
        AudioPlayer.StopSound("Discord");
        SanesssIntroText.text = "";
        SanesssIntroText.text = "Y";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "Yo";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You'";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You'r";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're n";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're no";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not s";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not su";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not sup";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supp";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not suppo";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not suppos";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not suppose";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed t";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed to";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed to b";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed to be";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed to be h";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed to be he";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed to be her";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed to be here";
        yield return new WaitForSeconds(0.08f);
        SanesssIntroText.text = "You're not supposed to be here.";
        yield return new WaitForSeconds(4f);
        AudioPlayer.StopSound("Discord");
        SanesssIntroText.text = "Leave. Return to your world immediately.";
        yield return new WaitForSeconds(6f);
        AudioPlayer.StopSound("Discord");
        SanesssIntroText.text = "This is our playground to destroy.";
        yield return new WaitForSeconds(5f);
        AudioPlayer.StopSound("Discord");
        SanesssIntroText.text = "Huh? Not listening?";
        yield return new WaitForSeconds(3f);
        AudioPlayer.StopSound("Discord");
        SanesssIntroText.text = "Then we'll send him in. Good luck, you'll need it.";
        yield return new WaitForSeconds(6f);
        SanesssIntroText.text = "";
        AudioPlayer.StopSound("Discord");
        transform.position = new Vector3(0f, 0f, -2f);
        Instantiate(SanesssPrefab, new Vector3(-7f, 0f, -2f), Quaternion.identity);
        yield return null;
    }

    private IEnumerator CampChecking()
    {
        bool standingOnDiceTile = diceThrowScript.isOnDiceTile1 || diceThrowScript.isOnDiceTile2 || diceThrowScript.isOnDiceTile3 ||
                                  diceThrowScript.isOnDiceTile4 || diceThrowScript.isOnDiceTile5 || diceThrowScript.isOnDiceTile6; 
        if (PlayerPrefs.GetInt("BossDefeated") == 0)
        {
            yield return new WaitForSeconds(1f);
            if (distancePerSecond < 0.3f && UnityEngine.Random.Range(0,50) == 0 && standingOnDiceTile) { Instantiate(ghost, new Vector3(transform.position.x + UnityEngine.Random.Range(-10f, 10f), transform.position.y + UnityEngine.Random.Range(-10f, 10f), 0), Quaternion.identity); }
            distancePerSecond = 0;
            StartCoroutine(CampChecking());
        }
        
        yield return null;
    }

    private IEnumerator WhaleForced()
    {
        for (int numberOfSummons = 0; numberOfSummons < WhaleLevelInterval.Length; numberOfSummons++)
        {
            yield return new WaitForSeconds(WhaleLevelInterval[numberOfSummons]*1.5f);
            Instantiate(Whale, transform.position, Quaternion.identity);
        }
        yield return null;
    }

    private IEnumerator WhaleChance()
    {
        int level = SceneManager.GetActiveScene().buildIndex - 4;
        yield return new WaitForSeconds(UnityEngine.Random.Range(25f, 35f) - level/2);
        int whaleChance = UnityEngine.Random.Range(0,10);
        if (KeyWheatScript.EnemiesKilled < KeyWheatScript.EnemyLimit && whaleChance == 0)
        {
            Instantiate(Whale, transform.position, Quaternion.identity);
            StartCoroutine(WhaleChance());
        }
        yield return null;
    }

    private IEnumerator DevilishWhaleChance()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(60f, 100f));
        int whaleChance = UnityEngine.Random.Range(0,2);
        if  (whaleChance == 0)
        {
            StartCoroutine(SummonDevilishWhale());
            StartCoroutine(DevilishWhaleChance());
        }
        yield return null;
    }

    public IEnumerator SummonDevilishWhale()
    {
        AudioPlayer.StopSound("Discord");
        yield return new WaitForSeconds(3f);
        AudioPlayer.PlaySound("WindingMusicBox");
        yield return new WaitForSeconds(11.2f);
        AudioPlayer.PlaySound("Meow");
        whaleJumpscare.color = new Color32(255,255,255,255);
        yield return new WaitForSeconds(0.65f);
        whaleJumpscare.color = new Color32(255,255,255,0);
        summonFlippedEnemies();
        Instantiate(WaterContainer, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f,8f));
        if (PlayerPrefs.GetInt("SanesssStatus") != 1) { AudioPlayer.PlayJingle("Discord"); }
        yield return null;
    }

    private void summonFlippedEnemies()
    {
        for (int a = 0; a < UnityEngine.Random.Range(0, 3); a++)
        {
            Instantiate(flippedEnemyPrefabs[UnityEngine.Random.Range(0, flippedEnemyPrefabs.Length)], new Vector3(transform.position.x + UnityEngine.Random.Range(-11f, 11f), transform.position.y + UnityEngine.Random.Range(-11f, 11f), 0), Quaternion.identity);
        }
        for (int i = 0; i < 6; i++)
        {
            Instantiate(flippedEnemyPrefabs[i], new Vector3(transform.position.x + UnityEngine.Random.Range(-11f, 11f), transform.position.y + UnityEngine.Random.Range(-11f, 11f), 0), Quaternion.identity);
        }
    }

    public void SummonChanceWhale()
    {
        Instantiate(ChanceWhale, transform.position, Quaternion.identity);
    }

    private IEnumerator FSC()
    {
        PlayerPrefs.SetInt("IngameFSC", PlayerPrefs.GetInt("IngameFSC") - 1);
        UpdateHealth(maxHealth);
        FSCInvincible = true;
        AudioPlayer.PlaySound("LilD");
        yield return new WaitForSeconds(3.5f);
        Instantiate(MyriadCookies, transform.position, Quaternion.identity);
        AudioPlayer.PlaySound("FSC");
        yield return new WaitForSeconds(16.5f);
        FSCInvincible = false;
        Invincible = false;
        yield return null;
    }

    public void UpdateValues()
    {
        LatterIngameSaveData();
        diceThrowScript.DiceThrowLoadData();
        dicePadSpawnerScript.DicePadLoadData();
        ultimateScript.UltimateLoadData();
        cooldownBarScript.CooldownBarLoadData();
        DiceCounterNumber.text = diceNumber.ToString();
    }

    private void FixedUpdate()
    {
        playerRB.MovePosition(playerRB.position + movement * MoveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wheat" || other.gameObject.tag == "PGW") { AudioPlayer.PlaySound("DicePickup"); } 

        if (other.gameObject.tag == "DiceTile1") { AudioPlayer.PlaySound("PadOn"); diceThrowScript.isOnDiceTile1 = true; this.other = other; }
        if (other.gameObject.tag == "DiceTile2") { AudioPlayer.PlaySound("PadOn"); diceThrowScript.isOnDiceTile2 = true; this.other = other; }
        if (other.gameObject.tag == "DiceTile3") { AudioPlayer.PlaySound("PadOn"); diceThrowScript.isOnDiceTile3 = true; this.other = other; }
        if (other.gameObject.tag == "DiceTile4") { AudioPlayer.PlaySound("PadOn"); diceThrowScript.isOnDiceTile4 = true; this.other = other; }
        if (other.gameObject.tag == "DiceTile5") { AudioPlayer.PlaySound("PadOn"); diceThrowScript.isOnDiceTile5 = true; this.other = other; }
        if (other.gameObject.tag == "DiceTile6") { AudioPlayer.PlaySound("PadOn"); diceThrowScript.isOnDiceTile6 = true; this.other = other; }

        if (other.gameObject.tag == "FSCDrop") 
        { 
            if (PlayerPrefs.GetInt("FSC") == 0) 
            { 
                PlayerPrefs.SetInt("FSC", 1); 
                StartCoroutine(MyriadCookiesText());
            } 
            PlayerPrefs.SetInt("IngameFSC", 3);
            AudioPlayer.PlaySound("FSCGang");
            
            GameObject FSCDropContainer = GameObject.Find("FSCDropContainer(Clone)");
            Destroy(FSCDropContainer);
        }

        if (other.gameObject.tag == "GoldenWheat") { Win(); }

        if (other.gameObject.tag == "Level12") { StartCoroutine(LoadToLevel(12)); }
        if (other.gameObject.tag == "Level2") { StartCoroutine(LoadToLevel(2)); }
        if (other.gameObject.tag == "Level5") { StartCoroutine(LoadToLevel(5)); }
        if (other.gameObject.tag == "Level8") { StartCoroutine(LoadToLevel(8)); }
        if (other.gameObject.tag == "Level10") { StartCoroutine(LoadToLevel(10)); }

        if (other.gameObject.tag == "Tumbleweed") { AudioPlayer.PlaySound("TumbleweedHit"); }
        if (other.gameObject.tag == "4thWall") { AudioPlayer.PlaySound("4thWallHit"); }

        if (other.gameObject.tag == "RedWheat") { UnlockKey("Red"); Destroy(other.gameObject); }
        if (other.gameObject.tag == "BlueWheat") { UnlockKey("Blue"); Destroy(other.gameObject); }
        if (other.gameObject.tag == "GreenWheat") { UnlockKey("Green"); Destroy(other.gameObject); }
        if (other.gameObject.tag == "ExitHoe") 
        { 
            
            if (KeyWheatScript.Level == 1) { StartCoroutine(GoToNextLevel()); }
            else if (KeyWheatScript.Level == 2) 
            { 
                if (!KeyWheatScript.haveRedWheat)
                {
                    requiredWheat = "";
                    requiredWheat += "Red "; 
                    StartCoroutine(NotifTextWarning());
                }
                else { StartCoroutine(GoToNextLevel()); }
            }
            else if (KeyWheatScript.Level == 3) 
            { 
                if (!KeyWheatScript.haveBlueWheat || !KeyWheatScript.haveGreenWheat)
                {
                    requiredWheat = "";
                    if (!KeyWheatScript.haveBlueWheat) { requiredWheat += "Blue "; }
                    if (!KeyWheatScript.haveGreenWheat) { requiredWheat += "Green "; }
                    StartCoroutine(NotifTextWarning());
                }
                else { StartCoroutine(GoToNextLevel()); }
            }        
            else if (!KeyWheatScript.haveRedWheat || !KeyWheatScript.haveBlueWheat || !KeyWheatScript.haveGreenWheat)
            {
                requiredWheat = "";
                if (!KeyWheatScript.haveRedWheat) { requiredWheat += "Red "; }
                if (!KeyWheatScript.haveBlueWheat) { requiredWheat += "Blue "; }
                if (!KeyWheatScript.haveGreenWheat) { requiredWheat += "Green "; }
                StartCoroutine(NotifTextWarning());
            }
            else { StartCoroutine(GoToNextLevel()); }
        }
    }

    private IEnumerator TransitionInSound()
    {
        yield return new WaitForSeconds(0.9f);
        AudioPlayer.PlaySound("ButtonSelect");
        yield return null;
    }

    private IEnumerator LoadToLevel(int wantedScene)
    {
        transitionOut.SetActive(true);
        yield return new WaitForSeconds(1.9f);
        AudioPlayer.PlaySound("ButtonSelect");
        yield return new WaitForSeconds(0.3f);

        EnterLevelIngameSaveData();
        PlayerPrefs.SetInt("SavedLevel", wantedScene);

        SceneManager.LoadScene(wantedScene + 3);

        yield return null;
    }

    private IEnumerator GoToNextLevel()
    {
        Invincible = true;
        transitionOut.SetActive(true);
        yield return new WaitForSeconds(1.9f);
        AudioPlayer.PlaySound("ButtonSelect");
        yield return new WaitForSeconds(0.3f);

        int current = PlayerPrefs.GetInt("SavedLevel");
        PlayerPrefs.SetInt("SavedLevel", current + 1);

        EnterLevelIngameSaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        yield return null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "DiceTile1") { diceThrowScript.isOnDiceTile1 = false; AudioPlayer.PlaySound("PadOff"); }
        if (other.gameObject.tag == "DiceTile2") { diceThrowScript.isOnDiceTile2 = false; AudioPlayer.PlaySound("PadOff"); }
        if (other.gameObject.tag == "DiceTile3") { diceThrowScript.isOnDiceTile3 = false; AudioPlayer.PlaySound("PadOff"); }
        if (other.gameObject.tag == "DiceTile4") { diceThrowScript.isOnDiceTile4 = false; AudioPlayer.PlaySound("PadOff"); }
        if (other.gameObject.tag == "DiceTile5") { diceThrowScript.isOnDiceTile5 = false; AudioPlayer.PlaySound("PadOff"); }
        if (other.gameObject.tag == "DiceTile6") { diceThrowScript.isOnDiceTile6 = false; AudioPlayer.PlaySound("PadOff"); }
    }

    private IEnumerator CantCraftBread(int cooldown)
    {
        AudioPlayer.PlaySound("UIButtonError");
        SmallText.text = "Wait " + cooldown + " before healing again";
        yield return new WaitForSeconds(1.5f);
        SmallText.text = "";
        yield return null;
    }

    private IEnumerator MyriadCookiesText()
    {
        SmallText.text = "You got Myriad Cookies!";
        yield return new WaitForSeconds(4f);
        SmallText.text = "";
        yield return null;
    }

    private IEnumerator FullHealth()
    {
        AudioPlayer.PlaySound("UIButtonError");
        SmallText.text = "You're at full health";
        yield return new WaitForSeconds(1f);
        SmallText.text = "";
        yield return null;
    }

    private IEnumerator NotEnoughWheat()
    {
        AudioPlayer.PlaySound("UIButtonError");
        SmallText.text = "Needs " + requiredWheat + " Wheat";
        yield return new WaitForSeconds(1f);
        SmallText.text = "";
        yield return null;
    }

    public IEnumerator UltNotifText()
    {
        SmallText.text = "Ultimate Ready!";
        yield return new WaitForSeconds(2f);
        SmallText.text = "";
        yield return null;
    }

    private IEnumerator NotifTextWarning()
    {
        AudioPlayer.PlaySound("UIButtonError");
        SmallText.text = "Needs " + requiredWheat + "Wheat";
        yield return new WaitForSeconds(1f);
        SmallText.text = "";
        yield return null;
    }

    public IEnumerator NotifTextMapUnlock()
    {
        AudioPlayer.PlaySound("MapUnlock");
        SmallText.text = "Map Revealed";
        yield return new WaitForSeconds(3f);
        SmallText.text = "";
        yield return null;
    }

    public IEnumerator NotifTextKeyUnlock()
    {
        AudioPlayer.PlaySound("MapUnlock");
        SmallText.text = "Wheat Cards Position Shown";
        yield return new WaitForSeconds(3f);
        SmallText.text = "";
        yield return null;
    }

    public IEnumerator NotifTextChargedATKUpgrade(int ok)
    {
        AudioPlayer.PlaySound("MapUnlock");
        SmallText.text = "Charged Attack Level " + ok;
        yield return new WaitForSeconds(2f);
        SmallText.text = "";
        yield return null;
    }

    private void UnlockKey(string i)
    {
        if (i == "Red") { KeyWheatScript.haveRedWheat = true; RedWheatIndicator.SetActive(true); }
        if (i == "Blue") { KeyWheatScript.haveBlueWheat = true; BlueWheatIndicator.SetActive(true); }
        if (i == "Green") { KeyWheatScript.haveGreenWheat = true; GreenWheatIndicator.SetActive(true); }
        AudioPlayer.PlaySound("DicePickup");
    }

    private void CraftBread()
    {
        int level = SceneManager.GetActiveScene().buildIndex - 4;
        int lowPrice = Mathf.RoundToInt(Mathf.Pow((float)level, 2f) + 15f); 
        if (Mathf.RoundToInt(Wheat*0.1f) < lowPrice) { healWheatCost = lowPrice; }
        else { healWheatCost = Mathf.RoundToInt(Wheat*0.1f); }

        if (Wheat < healWheatCost) 
        { 
            requiredWheat = healWheatCost.ToString();
            StartCoroutine(NotEnoughWheat());
        }
        else if (Wheat >= healWheatCost && playerHealth < maxHealth) 
        { 
            UpdateWheat(-healWheatCost); 
            UpdateHealth(Mathf.RoundToInt(maxHealth*0.35f));
            AudioPlayer.PlaySound("Burp"); 
            inCooldown = true;
            LatterIngameSaveData();
        }
        else { StartCoroutine(FullHealth()); }
    }

    public void IncreaseDiceNumber()
    {
        diceNumber++;
        DiceCounterNumber.text = diceNumber.ToString();
        UpdateValues();
    }

    public void HitByScythe()
    {
        if (Invincible == false) { diceNumber--; }
        if (diceNumber < 0) { diceNumber = 0; }
        if (Invincible == false) { MoveSpeed -= 0.5f; }
        if (MoveSpeed < 2.5f) { MoveSpeed = 2.5f; }
        DiceCounterNumber.text = diceNumber.ToString();
        UpdateValues();
    }

    public void UseChargeAttack()
    {
        diceNumber -= 3;
        UpdateValues();
    }

    public void HitByHand()
    {
        if (Invincible == false) { MoveSpeed -= 0.15f; }
        if (MoveSpeed < 2.5f) { MoveSpeed = 2.5f; }
        UpdateValues();
    }

    public void HitByC()
    {
        if (Invincible == false) { defense -= 1f; }
        if (defense < 1f) { defense = 1f; }
        UpdateValues();
    }

    public void UpdateHealth(int healthMod)
    {
        if (Invincible == true && healthMod < 0) { healthMod = 0; }
        playerHealth += healthMod;

        if ((float)playerHealth > maxHealth*0.3f) { lowHealth = false; }

        healthBar.SetHealth(playerHealth);
        StartCoroutine(FlashingHealthBar());

        if (playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
        } else if (playerHealth <= 0)
        {
            playerHealth = 0;
        }
    }

    private void Gasp()
    {
        lowHealth = true;
        AudioPlayer.PlaySound("Gasp");
    }

    public void UpdateWheat(int wheatMod) 
    {
        Wheat += wheatMod;
        if (Wheat < 0) { Wheat = 0; }
        WheatCounterNumber.text = Wheat.ToString();
        LatterIngameSaveData();
    }

    private void GameOver()
    {
        PlayerPrefs.SetInt("SavedLevel", 1);
        AudioPlayer.PlayJingle("GameOver");
        playerAlive = false;
        SaveData();
        Time.timeScale = 0f;
        crosshair.SetActive(false);
        diceThrower.SetActive(false);
        gameOverScript.GameOverTrigger(Wheat);
    }

    private void Win()
    {
        PlayerPrefs.SetInt("SavedLevel", 1);
        gameOverScript.WinningTrigger(Wheat);
        Wheat += 1000;
        AudioPlayer.PlayJingle("YouWon");
        playerAlive = false;
        SpeedrunTimer TimerScript = GameObject.FindGameObjectWithTag("Timer").GetComponent<SpeedrunTimer>();
        TimerScript.SetEndingTime();
        TimerScript.UpdateTimer(1);
        TimerScript.timerText.color = new Color32(255, 255, 255, 0);
        if (PlayerPrefs.GetFloat("Timer") < PlayerPrefs.GetFloat("BestTime")) { PlayerPrefs.SetFloat("BestTime", PlayerPrefs.GetFloat("Timer")); }
        SaveData();
        Time.timeScale = 0f;
        crosshair.SetActive(false);
        diceThrower.SetActive(false);
    }

    private IEnumerator FlashingHealthBar()
    {
        healthBar.HealthBarFlash(1);
        yield return new WaitForSeconds(0.25f);
        healthBar.HealthBarFlash(0);
        yield return null;
    }

    IEnumerator RollMode()
    {
        triggeredBroom = true;
        if (playerAlive) 
        {
            CurrentMode = 0;
            yield return new WaitForSeconds(0.05f);
            CurrentMode = UnityEngine.Random.Range(1,4);
            BroomInMode = false;
            if (CurrentMode > 1) { Instantiate(BroomPrefabs[CurrentMode], new Vector3(transform.position.x - 1.5f, transform.position.y + 1f, 0), Quaternion.identity, this.gameObject.transform); }
        }
        yield return null;
    }
    IEnumerator AttackMode()
    {
        if (playerAlive) 
        {
            BroomInMode = true;
            for (int a = 0; a < 20; a++)
            {
                int hi = UnityEngine.Random.Range(0,4);
                if (hi == 0) { AudioPlayer.PlaySound("BroomZoom1"); }
                if (hi == 1) { AudioPlayer.PlaySound("BroomZoom2"); }
                if (hi == 2) { AudioPlayer.PlaySound("BroomZoom3"); }
                if (hi == 3) { AudioPlayer.PlaySound("BroomZoom4"); }

                Instantiate(BroomPrefabs[UnityEngine.Random.Range(0,2)], new Vector3(transform.position.x - 10f, transform.position.y + UnityEngine.Random.Range(-5f,5f), transform.position.z), Quaternion.identity);
                yield return new WaitForSeconds(UnityEngine.Random.Range(1f,3f));
            }
            StartCoroutine(RollMode()); 
        }
        yield return null;
    }
    IEnumerator HealMode()
    {
        if (playerAlive) {
            BroomBuffImage.color = new Color32(255, 255, 255, 255);
            BroomInMode = true;
            BroomBuffImage.sprite = BroomBuffIcons[5]; 
            InHealMode = true;
            yield return new WaitForSeconds(UnityEngine.Random.Range(3f,12f));
            BroomBuffImage.color = new Color32(255, 255, 255, 0);
            InHealMode = false;
            StartCoroutine(RollMode());
        }
        yield return null;
    }
    IEnumerator StatBuffMode()
    {
        if (playerAlive) 
        {
            BroomBuffImage.color = new Color32(255, 255, 255, 255);
            BroomInMode = true;
            for (int b = 0; b < 5; b++)
            {
                AudioPlayer.PlaySound("BroomBuff");
                int PickRandomStat = UnityEngine.Random.Range(0,5);
                if (PickRandomStat == 0) { strength += 2f; HasStrengthTemporaryBuff = true; LatterIngameSaveData(); BroomBuffImage.sprite = BroomBuffIcons[0]; }
                if (PickRandomStat == 1) { defense += 2f; HasDefenseTemporaryBuff = true; LatterIngameSaveData(); BroomBuffImage.sprite = BroomBuffIcons[1]; }
                if (PickRandomStat == 2) { MoveSpeed += 1f; HasSpeedTemporaryBuff = true; LatterIngameSaveData(); BroomBuffImage.sprite = BroomBuffIcons[2]; }
                if (PickRandomStat == 3) { IncreaseDiceNumber(); BroomBuffImage.sprite = BroomBuffIcons[3]; }
                if (PickRandomStat == 4) { RegenStop = false; StartCoroutine(Regen()); BroomBuffImage.sprite = BroomBuffIcons[4]; }
                yield return new WaitForSeconds(UnityEngine.Random.Range(4f,12f));
                SubtractTemporaryBuff();
                if (PickRandomStat == 4) { RegenStop = true; }
            }
            BroomBuffImage.color = new Color32(255, 255, 255, 0);
            StartCoroutine(RollMode());
        }
        yield return null;
    }

    private void SubtractTemporaryBuff()
    {
        if (HasStrengthTemporaryBuff) { strength -= 2f; HasStrengthTemporaryBuff = false; LatterIngameSaveData(); }
        if (HasDefenseTemporaryBuff) { defense -= 2f; HasDefenseTemporaryBuff = false; LatterIngameSaveData(); }
        if (HasSpeedTemporaryBuff) { MoveSpeed -= 1f; HasSpeedTemporaryBuff = false; LatterIngameSaveData(); }
    }

    IEnumerator Regen()
    {
        UpdateHealth(Mathf.RoundToInt(maxHealth*0.05f));
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f,4f));
        if (RegenStop == false) { StartCoroutine(Regen()); }
        yield return null;
    }

    private IEnumerator InvincibleWaves()
    {
        SalmonStarted = true;
        yield return new WaitForSeconds(100f);
        Invincible = true;
        AudioPlayer.PlaySound("HeatRiser");
        SalmonRiser.SetActive(true);
        yield return new WaitForSeconds(4f + 0.16f*diceNumber);
        Invincible = false;
        SalmonRiser.SetActive(false);
        healthBar.HealthBarFlash(0);
        StartCoroutine(InvincibleWaves());
        yield return null;
    }

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);
        
        
        PlayerPrefs.SetFloat("MoveSpeed", loadedPlayerData.MoveSpeed);
        PlayerPrefs.SetInt("maxHealth", loadedPlayerData.maxHealth);
        PlayerPrefs.SetInt("playerHealth", loadedPlayerData.playerHealth);
        PlayerPrefs.SetFloat("strength", loadedPlayerData.strength);
        PlayerPrefs.SetInt("Wheat", loadedPlayerData.Wheat);
        PlayerPrefs.SetInt("diceNumber", loadedPlayerData.diceNumber);
        PlayerPrefs.SetFloat("playerCooldownTime", loadedPlayerData.playerCooldownTime);
        PlayerPrefs.SetFloat("defense", loadedPlayerData.defense);
        PlayerPrefs.SetFloat("wheatDroprate", loadedPlayerData.wheatDroprate);
        PlayerPrefs.SetInt("dicePreviewerLevel", loadedPlayerData.dicePreviewerLevel);
        PlayerPrefs.SetInt("diceDroprate", loadedPlayerData.diceDroprate);
        PlayerPrefs.SetInt("havePizza", Convert.ToInt32(loadedPlayerData.havePizza));
        PlayerPrefs.SetInt("haveCarrotCake", Convert.ToInt32(loadedPlayerData.haveCarrotCake));
        PlayerPrefs.SetInt("haveFlan", Convert.ToInt32(loadedPlayerData.haveFlan));
        PlayerPrefs.SetInt("haveCremeBrulee", Convert.ToInt32(loadedPlayerData.haveCremeBrulee));
        PlayerPrefs.SetInt("haveBanhmi", Convert.ToInt32(loadedPlayerData.haveBanhmi));
        PlayerPrefs.SetInt("haveCupcake", Convert.ToInt32(loadedPlayerData.haveCupcake));
        PlayerPrefs.SetInt("haveChickenNuggets", Convert.ToInt32(loadedPlayerData.haveChickenNuggets));
        PlayerPrefs.SetInt("havePastelDeChoclo", Convert.ToInt32(loadedPlayerData.havePastelDeChoclo));
        PlayerPrefs.SetInt("haveGarlicBread", Convert.ToInt32(loadedPlayerData.haveGarlicBread));
        PlayerPrefs.SetInt("haveHornScallop", Convert.ToInt32(loadedPlayerData.haveHornScallop));
    }

    private void SaveData()
    {
        PlayerData savingPlayerData = new PlayerData();

        savingPlayerData.firstTime = false;
        savingPlayerData.MoveSpeed = PlayerPrefs.GetFloat("MoveSpeed");
        savingPlayerData.maxHealth = PlayerPrefs.GetInt("maxHealth");
        savingPlayerData.playerHealth = PlayerPrefs.GetInt("playerHealth");
        savingPlayerData.strength = PlayerPrefs.GetFloat("strength");
        savingPlayerData.Wheat = PlayerPrefs.GetInt("Wheat") + Wheat;
        savingPlayerData.diceNumber = PlayerPrefs.GetInt("diceNumber");
        savingPlayerData.playerCooldownTime = PlayerPrefs.GetFloat("playerCooldownTime");
        savingPlayerData.defense = PlayerPrefs.GetFloat("defense");
        savingPlayerData.wheatDroprate = PlayerPrefs.GetFloat("wheatDroprate");
        savingPlayerData.dicePreviewerLevel = PlayerPrefs.GetInt("dicePreviewerLevel");
        savingPlayerData.diceDroprate = PlayerPrefs.GetInt("diceDroprate");
        savingPlayerData.havePizza = Convert.ToBoolean(PlayerPrefs.GetInt("havePizza"));
        savingPlayerData.haveCarrotCake = Convert.ToBoolean(PlayerPrefs.GetInt("haveCarrotCake"));
        savingPlayerData.haveFlan = Convert.ToBoolean(PlayerPrefs.GetInt("haveFlan"));
        savingPlayerData.haveCremeBrulee = Convert.ToBoolean(PlayerPrefs.GetInt("haveCremeBrulee"));
        savingPlayerData.haveBanhmi = Convert.ToBoolean(PlayerPrefs.GetInt("haveBanhmi"));
        savingPlayerData.haveCupcake = Convert.ToBoolean(PlayerPrefs.GetInt("haveCupcake"));
        savingPlayerData.haveChickenNuggets = Convert.ToBoolean(PlayerPrefs.GetInt("haveChickenNuggets"));
        savingPlayerData.havePastelDeChoclo = Convert.ToBoolean(PlayerPrefs.GetInt("havePastelDeChoclo"));
        savingPlayerData.haveGarlicBread = Convert.ToBoolean(PlayerPrefs.GetInt("haveGarlicBread"));
        savingPlayerData.haveHornScallop = Convert.ToBoolean(PlayerPrefs.GetInt("haveHornScallop"));

        string json = JsonUtility.ToJson(savingPlayerData);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/gameSaveData.json", json);    
    }

    private void IngameLoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/ingameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        MoveSpeed = loadedPlayerData.MoveSpeed;
        maxHealth = loadedPlayerData.maxHealth;
        strength = loadedPlayerData.strength;
        Wheat = loadedPlayerData.Wheat;
        diceNumber = loadedPlayerData.diceNumber;
        playerCooldownTime = loadedPlayerData.playerCooldownTime;
        defense = loadedPlayerData.defense;
        wheatDroprate = loadedPlayerData.wheatDroprate;
        dicePreviewerLevel = loadedPlayerData.dicePreviewerLevel;
        diceDroprate = loadedPlayerData.diceDroprate;
        havePizza = loadedPlayerData.havePizza;
        haveCarrotCake = loadedPlayerData.haveCarrotCake;
        haveFlan = loadedPlayerData.haveFlan;
        haveCremeBrulee = loadedPlayerData.haveCremeBrulee;
        haveBanhmi = loadedPlayerData.haveBanhmi;
        haveCupcake = loadedPlayerData.haveCupcake;
        haveChickenNuggets = loadedPlayerData.haveChickenNuggets;
        havePastelDeChoclo = loadedPlayerData.havePastelDeChoclo;
        haveGarlicBread = loadedPlayerData.haveGarlicBread;
        haveHornScallop = loadedPlayerData.haveHornScallop;
    }
    private void FirstIngameSaveData()
    {
        PlayerData savingPlayerData = new PlayerData();

        savingPlayerData.MoveSpeed = PlayerPrefs.GetFloat("MoveSpeed");
        savingPlayerData.maxHealth = PlayerPrefs.GetInt("maxHealth");
        savingPlayerData.playerHealth = PlayerPrefs.GetInt("playerHealth");
        savingPlayerData.strength = PlayerPrefs.GetFloat("strength");
        savingPlayerData.Wheat = 0;
        savingPlayerData.diceNumber = PlayerPrefs.GetInt("diceNumber");
        savingPlayerData.playerCooldownTime = PlayerPrefs.GetFloat("playerCooldownTime");
        savingPlayerData.defense = PlayerPrefs.GetFloat("defense");
        savingPlayerData.wheatDroprate = PlayerPrefs.GetFloat("wheatDroprate");
        savingPlayerData.dicePreviewerLevel = PlayerPrefs.GetInt("dicePreviewerLevel");
        savingPlayerData.diceDroprate = PlayerPrefs.GetInt("diceDroprate");
        savingPlayerData.havePizza = Convert.ToBoolean(PlayerPrefs.GetInt("havePizza"));
        savingPlayerData.haveCarrotCake = Convert.ToBoolean(PlayerPrefs.GetInt("haveCarrotCake"));
        savingPlayerData.haveFlan = Convert.ToBoolean(PlayerPrefs.GetInt("haveFlan"));
        savingPlayerData.haveCremeBrulee = Convert.ToBoolean(PlayerPrefs.GetInt("haveCremeBrulee"));
        savingPlayerData.haveBanhmi = Convert.ToBoolean(PlayerPrefs.GetInt("haveBanhmi"));
        savingPlayerData.haveCupcake = Convert.ToBoolean(PlayerPrefs.GetInt("haveCupcake"));
        savingPlayerData.haveChickenNuggets = Convert.ToBoolean(PlayerPrefs.GetInt("haveChickenNuggets"));
        savingPlayerData.havePastelDeChoclo = Convert.ToBoolean(PlayerPrefs.GetInt("havePastelDeChoclo"));
        savingPlayerData.haveGarlicBread = Convert.ToBoolean(PlayerPrefs.GetInt("haveGarlicBread"));
        savingPlayerData.haveHornScallop = Convert.ToBoolean(PlayerPrefs.GetInt("haveHornScallop"));

        string json = JsonUtility.ToJson(savingPlayerData);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/ingameSaveData.json", json);    
    }
    public void LatterIngameSaveData()
    {
        PlayerData savingPlayerData = new PlayerData();

        savingPlayerData.MoveSpeed = MoveSpeed;
        savingPlayerData.maxHealth = maxHealth;
        savingPlayerData.playerHealth = playerHealth;
        savingPlayerData.strength = strength;
        savingPlayerData.Wheat = Wheat;
        savingPlayerData.diceNumber = diceNumber;
        savingPlayerData.playerCooldownTime = playerCooldownTime;
        savingPlayerData.defense = defense;
        savingPlayerData.wheatDroprate = wheatDroprate;
        savingPlayerData.dicePreviewerLevel = dicePreviewerLevel;
        savingPlayerData.diceDroprate = diceDroprate;
        savingPlayerData.havePizza = havePizza;
        savingPlayerData.haveCarrotCake = haveCarrotCake;
        savingPlayerData.haveFlan = haveFlan;
        savingPlayerData.haveCremeBrulee = haveCremeBrulee;
        savingPlayerData.haveBanhmi = haveBanhmi;
        savingPlayerData.haveCupcake = haveCupcake;
        savingPlayerData.haveChickenNuggets = haveChickenNuggets;
        savingPlayerData.havePastelDeChoclo = havePastelDeChoclo;
        savingPlayerData.haveGarlicBread = haveGarlicBread;
        savingPlayerData.haveHornScallop = haveHornScallop;

        string json = JsonUtility.ToJson(savingPlayerData);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/ingameSaveData.json", json);    
    }

    public void EnterLevelIngameSaveData()
    {
        PlayerData savingPlayerData = new PlayerData();

        savingPlayerData.MoveSpeed = MoveSpeed;
        savingPlayerData.maxHealth = maxHealth;
        savingPlayerData.playerHealth = playerHealth;
        savingPlayerData.strength = strength;
        savingPlayerData.Wheat = Wheat;
        savingPlayerData.diceNumber = diceNumber;
        savingPlayerData.playerCooldownTime = playerCooldownTime;
        savingPlayerData.defense = defense;
        savingPlayerData.wheatDroprate = wheatDroprate;
        savingPlayerData.dicePreviewerLevel = dicePreviewerLevel;
        savingPlayerData.diceDroprate = diceDroprate;
        savingPlayerData.havePizza = havePizza;
        savingPlayerData.haveCarrotCake = haveCarrotCake;
        savingPlayerData.haveFlan = haveFlan;
        savingPlayerData.haveCremeBrulee = haveCremeBrulee;
        savingPlayerData.haveBanhmi = haveBanhmi;
        savingPlayerData.haveCupcake = haveCupcake;
        savingPlayerData.haveChickenNuggets = haveChickenNuggets;
        savingPlayerData.havePastelDeChoclo = havePastelDeChoclo;
        savingPlayerData.haveGarlicBread = haveGarlicBread;
        savingPlayerData.haveHornScallop = haveHornScallop;
        PlayerPrefs.SetInt("UltCharge", ultimateScript.currentUltimateCharge);
        PlayerPrefs.SetInt("IngameRamenSave", PlayerPrefs.GetInt("IngameRamen"));
        PlayerPrefs.SetInt("IngameSalmonSave", PlayerPrefs.GetInt("IngameSalmon"));
        PlayerPrefs.SetInt("IngameSteakSave", PlayerPrefs.GetInt("IngameSteak"));
        PlayerPrefs.SetInt("IngameCheeseSave", PlayerPrefs.GetInt("IngameCheese"));
        PlayerPrefs.SetInt("IngameFSCSave", PlayerPrefs.GetInt("IngameFSC"));
        PlayerPrefs.SetFloat("DiceSpinLevelUpSave", PlayerPrefs.GetFloat("DiceSpinLevelUp"));
        PlayerPrefs.SetFloat("DiceSpinLevelSave", PlayerPrefs.GetFloat("DiceSpinLevel"));
        PlayerPrefs.SetInt("ChargedAttacksSave", PlayerPrefs.GetInt("ChargedAttacks"));
        PlayerPrefs.SetFloat("Timer", timerScript.time);

        string json = JsonUtility.ToJson(savingPlayerData);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/enterLevelSaveData.json", json);    
    }

    public void LoadLastSaved()
    {
        string json = File.ReadAllText(Application.dataPath + "/enterLevelSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        MoveSpeed = loadedPlayerData.MoveSpeed;
        maxHealth = loadedPlayerData.maxHealth;
        strength = loadedPlayerData.strength;
        Wheat = loadedPlayerData.Wheat;
        diceNumber = loadedPlayerData.diceNumber;
        playerCooldownTime = loadedPlayerData.playerCooldownTime;
        defense = loadedPlayerData.defense;
        wheatDroprate = loadedPlayerData.wheatDroprate;
        dicePreviewerLevel = loadedPlayerData.dicePreviewerLevel;
        diceDroprate = loadedPlayerData.diceDroprate;
        havePizza = loadedPlayerData.havePizza;
        haveCarrotCake = loadedPlayerData.haveCarrotCake;
        haveFlan = loadedPlayerData.haveFlan;
        haveCremeBrulee = loadedPlayerData.haveCremeBrulee;
        haveBanhmi = loadedPlayerData.haveBanhmi;
        haveCupcake = loadedPlayerData.haveCupcake;
        haveChickenNuggets = loadedPlayerData.haveChickenNuggets;
        havePastelDeChoclo = loadedPlayerData.havePastelDeChoclo;
        haveGarlicBread = loadedPlayerData.haveGarlicBread;
        haveHornScallop = loadedPlayerData.haveHornScallop;
        ultimateScript.currentUltimateCharge = PlayerPrefs.GetInt("UltCharge");
        PlayerPrefs.SetInt("IngameRamen", PlayerPrefs.GetInt("IngameRamenSave"));
        PlayerPrefs.SetInt("IngameSalmon", PlayerPrefs.GetInt("IngameSalmonSave"));
        PlayerPrefs.SetInt("IngameSteak", PlayerPrefs.GetInt("IngameSteakSave"));
        PlayerPrefs.SetInt("IngameCheese", PlayerPrefs.GetInt("IngameCheeseSave"));
        PlayerPrefs.SetInt("IngameFSC", PlayerPrefs.GetInt("IngameFSCSave"));
        PlayerPrefs.SetFloat("DiceSpinLevelUp", PlayerPrefs.GetFloat("DiceSpinLevelUpSave"));
        PlayerPrefs.SetFloat("DiceSpinLevel", PlayerPrefs.GetFloat("DiceSpinLevelSave"));
        PlayerPrefs.SetInt("ChargedAttacks", PlayerPrefs.GetInt("ChargedAttacksSave"));
    }
    

    private class PlayerData
    {
        public bool firstTime;
        public float MoveSpeed;
        public int maxHealth;
        public int playerHealth;
        public float strength;
        public int Wheat;
        public int diceNumber;
        public float playerCooldownTime;
        public float defense;
        public float wheatDroprate;
        public int dicePreviewerLevel;
        public int diceDroprate;
        public bool havePizza;
        public bool haveCarrotCake;
        public bool haveFlan;
        public bool haveCremeBrulee;
        public bool haveBanhmi;
        public bool haveCupcake;
        public bool haveChickenNuggets;
        public bool havePastelDeChoclo;
        public bool haveGarlicBread;
        public bool haveHornScallop;
    }
}
