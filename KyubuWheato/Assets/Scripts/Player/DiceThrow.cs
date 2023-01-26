using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using EZCameraShake;

public class DiceThrow : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Transform playerTransform;
    private Camera mainCam;
    private Vector3 mousePos;
    private PlayerController player;
    private AudioManager AudioPlayer;
    private Text DiceCounterNumber;
    [SerializeField] private GameObject[] fakeDiceTypes;
    [SerializeField] private GameObject[] dicetypes;
    [SerializeField] private GameObject[] MultishotDiceTypes;
    [SerializeField] private GameObject[] FakeMultishotLeftDiceTypes;
    [SerializeField] private GameObject[] FakeMultishotRightDiceTypes;
    [SerializeField] private GameObject[] chargedDicetypes;
    [SerializeField] private GameObject[] chargedMultishotDiceTypes;
    [SerializeField] private Transform diceTransform;
    private CooldownBar cooldownBar;
    private UltimateBarCharge ultimateBar;
    private Image[] DicePreviewerImage = new Image[5];
    [SerializeField] private Sprite[] DiceSprites;
    [SerializeField] private GameObject[] KyubuTiles;
    [SerializeField] private GameObject[] diceSpinPrefabs;
    
    private bool inCooldown = false;
    private float cooldownTimer;
    private bool chargedAttack;

    public int diceNumber;
    private float playerCooldownTime;
    private int dicePreviewerLevel;
    private bool havePizza;
    private bool haveCarrotCake;
    private bool haveFlan;
    private bool haveCremeBrulee;
    private bool haveBanhmi;
    private bool havePastelDeChoclo;

    public int KyubuStack = 0;
    private int KyubuStackMax = 20;
    private float KyubuStackTimer = 10f;
    private float KyubuStackTimerLimit = 10f;
    private bool triggeredKyubuStack = false;
    public bool inKyubuKombo100 = false;
    private int RandomDiceValue;
    private int RandomFakeDiceValue;

    public bool isOnDiceTile1;
    public bool isOnDiceTile2;
    public bool isOnDiceTile3;
    public bool isOnDiceTile4;
    public bool isOnDiceTile5;
    public bool isOnDiceTile6;
    
    private int[] DiceValues = new int[6];
    private int[] PreviousDiceValues = new int[6];
    
    private int FakeDiceValue;
    private int[] FakePreviousDiceValues = new int[6];

    [SerializeField] private bool isLevel12;
    [SerializeField] private GameObject TimeCrescent;
    private SpriteRenderer TimeCrescentSprite;
    [SerializeField] private Sprite[] CrescentCracks;
    [SerializeField] private GameObject starPrefab;

    void Start()
    {
        TimeCrescentSprite = TimeCrescent.GetComponent<SpriteRenderer>();
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        AudioPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        ultimateBar = GameObject.FindGameObjectWithTag("Ultimate Bar").GetComponent<UltimateBarCharge>();
        cooldownBar = GameObject.FindGameObjectWithTag("CooldownBar").GetComponent<CooldownBar>();
        DiceCounterNumber = GameObject.FindGameObjectWithTag("DiceCounter").GetComponent<Text>();
        DicePreviewerImage[0] = GameObject.FindGameObjectWithTag("DicePreviewer1").GetComponent<Image>();
        DicePreviewerImage[1] = GameObject.FindGameObjectWithTag("DicePreviewer2").GetComponent<Image>();
        DicePreviewerImage[2] = GameObject.FindGameObjectWithTag("DicePreviewer3").GetComponent<Image>();
        DicePreviewerImage[3] = GameObject.FindGameObjectWithTag("DicePreviewer4").GetComponent<Image>();
        DicePreviewerImage[4] = GameObject.FindGameObjectWithTag("DicePreviewer5").GetComponent<Image>();
        DiceThrowLoadData();
        GenerateRandomDiceArray();
        GenerateRandomFakeArray();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
        DiceCounterNumber.text = diceNumber.ToString();
    }

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rotation = mousePos - transform.position;

            float rotZ = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (inCooldown)
            {   
                cooldownBar.SetCooldown(cooldownTimer*10);
                if (chargedAttack) { cooldownTimer += Time.deltaTime/4; }
                else { cooldownTimer += Time.deltaTime; }
                if (cooldownTimer > playerCooldownTime/10)
                {
                    if (chargedAttack)
                    {
                        PlayerPrefs.SetInt("ChargedAttacks", PlayerPrefs.GetInt("ChargedAttacks") + 1);
                        AudioPlayer.PlaySound("ThrowDice");
                        if (haveFlan == true && haveCremeBrulee == false) { ShootTwoChargedDice(); }
                        else if (haveCremeBrulee) { ShootThreeChargedDice(); }
                        else { ShootOneChargedDice(); }
                        DiceCounterNumber.text = diceNumber.ToString();
                        if (PlayerPrefs.GetInt("IngameCheese") == 1) { ShootStar(UnityEngine.Random.Range(10, 15)); }
                    }
                    chargedAttack = false;
                    inCooldown = false;
                    cooldownTimer = 0;
                    cooldownBar.CooldownBarInvisible();
                }
            }

            if (triggeredKyubuStack)
            {
                KyubuStackTimer += Time.deltaTime; 
                if (KyubuStackTimer > KyubuStackTimerLimit)
                {
                    triggeredKyubuStack = false;
                    KyubuStack = 0;
                }
            }

            if (Mathf.FloorToInt(PlayerPrefs.GetFloat("DiceSpinLevel")/10f) == Mathf.FloorToInt(PlayerPrefs.GetFloat("DiceSpinLevelUp")) && PlayerPrefs.GetFloat("DiceSpinLevelUp") < 11)
            {
                float i = PlayerPrefs.GetFloat("DiceSpinLevelUp");
                player.StartCoroutine(player.NotifTextChargedATKUpgrade(Mathf.RoundToInt(i)));
                PlayerPrefs.SetFloat("DiceSpinLevelUp", i + 1f);
            }

            if (Input.GetMouseButton(0) && diceNumber > 0 && inCooldown == false && ultimateBar.ultimateInProgress == false)
            {
                AudioPlayer.PlaySound("ThrowDice");
                inCooldown = true;
                if (haveFlan == true && haveCremeBrulee == false) { ShootTwoDice(); }
                else if (haveCremeBrulee) { ShootThreeDice(); }
                else { ShootOneDice(); }
                if (PlayerPrefs.GetInt("IngameCheese") == 1) { ShootStar(UnityEngine.Random.Range(0, 9)); }
                DiceCounterNumber.text = diceNumber.ToString();
            }
            else if (Input.GetMouseButtonDown(0)) { AudioPlayer.PlaySound("ThrowDiceDisabled"); }

            if (Input.GetMouseButton(1) && diceNumber > 0 && inCooldown == false && ultimateBar.ultimateInProgress == false)
            {
                AudioPlayer.PlaySound("DiceShaking");    
                inCooldown = true;
                chargedAttack = true;
            }
            else if (Input.GetMouseButtonDown(1)) { AudioPlayer.PlaySound("ThrowDiceDisabled"); }

            if (Input.GetKeyDown(KeyCode.Q) && ultimateBar.currentUltimateCharge == ultimateBar.maxUltimateCharge && ultimateBar.havePizza == true)
            {
                AudioPlayer.PlaySound("UltimateActive");
                ultimateBar.IncreaseUltimateCharge(-ultimateBar.maxUltimateCharge);
                StartCoroutine(ActivateUltimate());
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && dicePreviewerLevel >= 2 && haveBanhmi == true) { DiceHotkey(1); }
            if (Input.GetKeyDown(KeyCode.Alpha2) && dicePreviewerLevel >= 3 && haveBanhmi == true) { DiceHotkey(2); }
            if (Input.GetKeyDown(KeyCode.Alpha3) && dicePreviewerLevel >= 4 && haveBanhmi == true) { DiceHotkey(3); }
            if (Input.GetKeyDown(KeyCode.Alpha4) && dicePreviewerLevel >= 5 && haveBanhmi == true) { DiceHotkey(4); }

            if (PlayerPrefs.GetInt("IngameSteak") == 1 && TimeCrescent.activeSelf == false) { StartCrescent(); }
        }  
    }
    
    private void StartCrescent()
    {
        TimeCrescent.SetActive(true);
        AudioPlayer.PlaySound("TimeCrescent");
        TimeCrescentSprite.sprite = CrescentCracks[Mathf.FloorToInt(PlayerPrefs.GetInt("Crack")/800)];
    }

    public void CrescentCrack(int crack)
    {
        PlayerPrefs.SetInt("PreviousCrack", PlayerPrefs.GetInt("Crack"));
        PlayerPrefs.SetInt("Crack", PlayerPrefs.GetInt("Crack") + crack);
        if (PlayerPrefs.GetInt("Crack") < 8000) { TimeCrescentSprite.sprite = CrescentCracks[Mathf.FloorToInt(PlayerPrefs.GetInt("Crack")/800)]; }

        if (PlayerPrefs.GetInt("Crack")/800 >= Mathf.FloorToInt(PlayerPrefs.GetInt("PreviousCrack")/800) + 1) { AudioPlayer.PlaySound("CrescentBreak" + UnityEngine.Random.Range(1,7)); }
        
        if (PlayerPrefs.GetInt("Crack") >= 8000)
        {
            PlayerPrefs.SetInt("Crack", 0);
            PlayerPrefs.SetInt("PreviousCrack", 0);
            PlayerPrefs.SetInt("IngameSteak", 0);
            TimeCrescentSprite.sprite = CrescentCracks[0];
            TimeCrescent.SetActive(false);
        }
    }

    private void GetDiceValue()
    {
        if (isOnDiceTile6) { RandomDiceValue = 5; }
        else if (isOnDiceTile5) { RandomDiceValue = 4; }
        else if (isOnDiceTile4) { RandomDiceValue = 3; }
        else if (isOnDiceTile3) { RandomDiceValue = 2; }
        else if (isOnDiceTile2) { RandomDiceValue = 1; }
        else if (isOnDiceTile1) { RandomDiceValue = 0; }
        else { RandomDiceValue = UnityEngine.Random.Range(0,dicetypes.Length); }
    }

    private void GenerateRandomDiceArray()
    {
        DiceValues[0] = UnityEngine.Random.Range(0,dicetypes.Length);
        DiceValues[1] = UnityEngine.Random.Range(0,dicetypes.Length);
        DiceValues[2] = UnityEngine.Random.Range(0,dicetypes.Length);
        DiceValues[3] = UnityEngine.Random.Range(0,dicetypes.Length);
        DiceValues[4] = UnityEngine.Random.Range(0,dicetypes.Length);
        DiceValues[5] = UnityEngine.Random.Range(0,dicetypes.Length);
        PreviousDiceValues[0] = 10;
        PreviousDiceValues[1] = 10;
        PreviousDiceValues[2] = 10;
        PreviousDiceValues[3] = 10;
        PreviousDiceValues[4] = 10;
        PreviousDiceValues[5] = 10;
        UpdateDicePreviewerUI();
    }

    private void CycleThroughDiceValueArray()
    {
        PreviousDiceValues[0] = PreviousDiceValues[1];
        PreviousDiceValues[1] = PreviousDiceValues[2];
        PreviousDiceValues[2] = PreviousDiceValues[3];
        PreviousDiceValues[3] = PreviousDiceValues[4];
        PreviousDiceValues[4] = PreviousDiceValues[5];
        PreviousDiceValues[5] =  DiceValues[0];
        DiceValues[0] = DiceValues[1];
        DiceValues[1] = DiceValues[2];
        DiceValues[2] = DiceValues[3];
        DiceValues[3] = DiceValues[4];
        GetDiceValue();
        DiceValues[4] = RandomDiceValue;
        UpdateDicePreviewerUI();
        KyubuKomboCheck();
    }

    private void GetFakeDiceValue()
    {
        if (isOnDiceTile6) { RandomFakeDiceValue = 5; }
        else if (isOnDiceTile5) { RandomFakeDiceValue = 4; }
        else if (isOnDiceTile4) { RandomFakeDiceValue = 3; }
        else if (isOnDiceTile3) { RandomFakeDiceValue = 2; }
        else if (isOnDiceTile2) { RandomFakeDiceValue = 1; }
        else if (isOnDiceTile1) { RandomFakeDiceValue = 0; }
        else { RandomFakeDiceValue = UnityEngine.Random.Range(0,dicetypes.Length); }
    }

    private void GenerateRandomFakeArray()
    {
        FakeDiceValue = UnityEngine.Random.Range(0,dicetypes.Length);
        FakePreviousDiceValues[0] = 10;
        FakePreviousDiceValues[1] = 10;
        FakePreviousDiceValues[2] = 10;
        FakePreviousDiceValues[3] = 10;
        FakePreviousDiceValues[4] = 10;
        FakePreviousDiceValues[5] = 10;
    }

    private void CycleThroughFakeArray()
    {
        FakePreviousDiceValues[0] = FakePreviousDiceValues[1];
        FakePreviousDiceValues[1] = FakePreviousDiceValues[2];
        FakePreviousDiceValues[2] = FakePreviousDiceValues[3];
        FakePreviousDiceValues[3] = FakePreviousDiceValues[4];
        FakePreviousDiceValues[4] = FakePreviousDiceValues[5];
        FakePreviousDiceValues[5] =  FakeDiceValue;
        GetFakeDiceValue();
        FakeDiceValue = RandomFakeDiceValue;
        FakeKyubuKomboCheck();
    }

    public void UpdateDicePreviewerUI()
    {
        DicePreviewerImage[0].sprite = DiceSprites[DiceValues[0]];
        DicePreviewerImage[1].sprite = DiceSprites[DiceValues[1]];
        DicePreviewerImage[2].sprite = DiceSprites[DiceValues[2]];
        DicePreviewerImage[3].sprite = DiceSprites[DiceValues[3]];
        DicePreviewerImage[4].sprite = DiceSprites[DiceValues[4]];
        DicePreviewerLevelCheck();
    }

    private void DicePreviewerLevelCheck()
    {
        if (dicePreviewerLevel >= 1) { DicePreviewerImage[0].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[0].color = new Color32(255, 255, 255, 0); }

        if (dicePreviewerLevel >= 2) { DicePreviewerImage[1].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[1].color = new Color32(255, 255, 255, 0); }

        if (dicePreviewerLevel >= 3) { DicePreviewerImage[2].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[2].color = new Color32(255, 255, 255, 0); }

        if (dicePreviewerLevel >= 4) { DicePreviewerImage[3].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[3].color = new Color32(255, 255, 255, 0); }

        if (dicePreviewerLevel >= 5) { DicePreviewerImage[4].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[4].color = new Color32(255, 255, 255, 0); }
    }

    private void DiceHotkey(int ChangingWithDice)
    {
        AudioPlayer.PlaySound("DiceHotkey");
        int mock = DiceValues[0];
        DiceValues[0] = DiceValues[ChangingWithDice];
        DiceValues[ChangingWithDice] = mock;
        UpdateDicePreviewerUI();
    }

    private void KyubuKomboCheck()
    {
        if (haveCarrotCake)
        {
            if (PreviousDiceValues[5] == 0 && PreviousDiceValues[4] == 0) { StartCoroutine(KyubuKombo(1)); }
            if (PreviousDiceValues[5] == 1 && PreviousDiceValues[4] == 1) { StartCoroutine(KyubuKombo(2)); }
            if (PreviousDiceValues[5] == 2 && PreviousDiceValues[4] == 2) { StartCoroutine(KyubuKombo(3)); }
            if (PreviousDiceValues[5] == 3 && PreviousDiceValues[4] == 3) { StartCoroutine(KyubuKombo(4)); }
            if (PreviousDiceValues[5] == 4 && PreviousDiceValues[4] == 4) { StartCoroutine(KyubuKombo(5)); }
            if (PreviousDiceValues[5] == 5 && PreviousDiceValues[4] == 5) { StartCoroutine(KyubuKombo(6)); }
            if (PreviousDiceValues[5] == 5 && PreviousDiceValues[4] == 4 && PreviousDiceValues[3] == 3 && PreviousDiceValues[2] == 2 && PreviousDiceValues[1] == 1 && PreviousDiceValues[0] == 0) 
            { 
                StartCoroutine(KyubuKombo(7)); 
            }
            if (PreviousDiceValues[5] == 0 && PreviousDiceValues[4] == 1 && PreviousDiceValues[3] == 2 && PreviousDiceValues[2] == 3 && PreviousDiceValues[1] == 4 && PreviousDiceValues[0] == 5) 
            { 
                StartCoroutine(KyubuKombo(7)); 
            }
        }
    }

    private void FakeKyubuKomboCheck()
    {
        if (haveCarrotCake && havePizza)
        {
            if (FakePreviousDiceValues[5] == 0 && FakePreviousDiceValues[4] == 0) { StartCoroutine(KyubuKombo(1)); }
                if (FakePreviousDiceValues[5] == 1 && FakePreviousDiceValues[4] == 1) { StartCoroutine(KyubuKombo(2)); }
                if (FakePreviousDiceValues[5] == 2 && FakePreviousDiceValues[4] == 2) { StartCoroutine(KyubuKombo(3)); }
                if (FakePreviousDiceValues[5] == 3 && FakePreviousDiceValues[4] == 3) { StartCoroutine(KyubuKombo(4)); }
                if (FakePreviousDiceValues[5] == 4 && FakePreviousDiceValues[4] == 4) { StartCoroutine(KyubuKombo(5)); }
                if (FakePreviousDiceValues[5] == 5 && FakePreviousDiceValues[4] == 5) { StartCoroutine(KyubuKombo(6)); }
                if (FakePreviousDiceValues[5] == 5 && FakePreviousDiceValues[4] == 4 && FakePreviousDiceValues[3] == 3 && FakePreviousDiceValues[2] == 2 && FakePreviousDiceValues[1] == 1 && FakePreviousDiceValues[0] == 0) 
                { 
                    StartCoroutine(KyubuKombo(100)); 
                }
                if (FakePreviousDiceValues[5] == 0 && FakePreviousDiceValues[4] == 1 && FakePreviousDiceValues[3] == 2 && FakePreviousDiceValues[2] == 3 && FakePreviousDiceValues[1] == 4 && FakePreviousDiceValues[0] == 5) 
                { 
                    StartCoroutine(KyubuKombo(100)); 
                }
        }
    }

    private void ShootStar(int starAmount)
    {
        for (int i = 0; i < starAmount; i++)
        {
            Instantiate(starPrefab, diceTransform.position, UnityEngine.Random.rotation); 
        }
    }

    IEnumerator KyubuKombo(int KyubuTileValue)
    {
        if (havePastelDeChoclo) 
        { 
            if (KyubuStack < KyubuStackMax) { KyubuStack++; }
            else { KyubuStack = KyubuStackMax;}
            KyubuStackTimer = 0f;
            triggeredKyubuStack = true;
        }

        if (KyubuTileValue == 1) 
        { 
            AudioPlayer.PlaySound("KK1");
            if (isLevel12 == false) { Instantiate(KyubuTiles[0], new Vector3(transform.position.x + UnityEngine.Random.Range(-1f, 1f), transform.position.y + 15.5f + UnityEngine.Random.Range(-1f, 1f), transform.position.z), Quaternion.identity); }
            else {
                try
                { 
                    Transform enemyPosition = GameObject.FindWithTag("enemyMouse").GetComponent<Transform>(); 
                    Instantiate(KyubuTiles[0], new Vector3(enemyPosition.position.x + UnityEngine.Random.Range(-1f, 1f), enemyPosition.position.y + 15.5f + UnityEngine.Random.Range(-1f, 1f), enemyPosition.position.z), Quaternion.identity); 
                }
                catch (NullReferenceException) 
                { 
                    Instantiate(KyubuTiles[0], new Vector3(transform.position.x + UnityEngine.Random.Range(-1f, 1f), transform.position.y + 15.5f + UnityEngine.Random.Range(-1f, 1f), transform.position.z), Quaternion.identity); 
                }
            }
        }
        if (KyubuTileValue == 2) 
        { 
            AudioPlayer.PlaySound("KK2");
            if (isLevel12 == false) { KK2NoTarget(); }
            else {
                try {
                    Transform enemyPosition = GameObject.FindWithTag("enemyMouse").GetComponent<Transform>();
                    int dafuq = UnityEngine.Random.Range(0,2);
                    if (dafuq < 1) 
                    { 
                        float randomOffset = UnityEngine.Random.Range(-1.5f, 1.5f);
                        Instantiate(KyubuTiles[1], new Vector3(enemyPosition.position.x - 22.63f, enemyPosition.position.y + randomOffset, enemyPosition.position.z), Quaternion.identity); 
                        Instantiate(KyubuTiles[2], new Vector3(enemyPosition.position.x + 22.63f, enemyPosition.position.y + randomOffset, enemyPosition.position.z), Quaternion.identity);  
                    }
                    else 
                    { 
                        float randomOffset2 = UnityEngine.Random.Range(-1.5f, 1.5f);
                        Instantiate(KyubuTiles[3], new Vector3(enemyPosition.position.x + randomOffset2, enemyPosition.position.y + 13.63f, enemyPosition.position.z), Quaternion.identity); 
                        Instantiate(KyubuTiles[4], new Vector3(enemyPosition.position.x + randomOffset2, enemyPosition.position.y - 13.63f, enemyPosition.position.z), Quaternion.identity);  
                    }
                }
                catch (NullReferenceException) { KK2NoTarget(); }
            }
        }
        if (KyubuTileValue == 3) 
        { 
            AudioPlayer.PlaySound("KK3Throw");
            Instantiate(KyubuTiles[UnityEngine.Random.Range(5,7)], transform.position, Quaternion.identity); 
        }
        if (KyubuTileValue == 4) 
        { 
            for (int i = 11; i < 15; i++) 
            { 
                Instantiate(KyubuTiles[i], transform.position, Quaternion.identity); 
            } 
            AudioPlayer.PlaySound("KK4");
        }
        if (KyubuTileValue == 5) 
        { 
            int rand = UnityEngine.Random.Range(7,9);
            if (rand == 7) { AudioPlayer.PlaySound("KK5Var"); }
            else { AudioPlayer.PlaySound("KK5"); }
            if (isLevel12 == false) { Instantiate(KyubuTiles[rand], new Vector3(transform.position.x + UnityEngine.Random.Range(-2f, 2f), transform.position.y + 12.5f + UnityEngine.Random.Range(-2f, 2f), transform.position.z), Quaternion.identity); }
            else {
                try  
                { 
                    Transform enemyPosition = GameObject.FindWithTag("enemyMouse").GetComponent<Transform>(); 
                    Instantiate(KyubuTiles[rand], new Vector3(enemyPosition.position.x + UnityEngine.Random.Range(-3f, 3f), enemyPosition.position.y + 12.5f + UnityEngine.Random.Range(-3f, 3f), enemyPosition.position.z), Quaternion.identity); 
                }
                catch (NullReferenceException) 
                { Instantiate(KyubuTiles[rand], new Vector3(transform.position.x + UnityEngine.Random.Range(-2f, 2f), transform.position.y + 12.5f + UnityEngine.Random.Range(-2f, 2f), transform.position.z), Quaternion.identity); }
            }
        }
        if (KyubuTileValue == 6) 
        { 
            AudioPlayer.PlaySound("KK6Hang");
            if (isLevel12 == false)
            {
                int bruh = UnityEngine.Random.Range(0,2);
                if (bruh < 1) { Instantiate(KyubuTiles[9], new Vector3(transform.position.x + 9f, transform.position.y + 6f, transform.position.z), Quaternion.identity); }
                else { Instantiate(KyubuTiles[10], new Vector3(transform.position.x - 22f, transform.position.y + 12f, transform.position.z), Quaternion.identity); }
            }
            else {
                try 
                {
                    Transform enemyPosition = GameObject.FindWithTag("enemyMouse").GetComponent<Transform>(); 
                    int bruh = UnityEngine.Random.Range(0,2);
                    if (bruh < 1) { Instantiate(KyubuTiles[9], new Vector3(enemyPosition.position.x + 9f + UnityEngine.Random.Range(-1f, 1f), enemyPosition.position.y + 6f + UnityEngine.Random.Range(-1f, 1f), enemyPosition.position.z), Quaternion.identity); }
                    else { Instantiate(KyubuTiles[10], new Vector3(enemyPosition.position.x - 22f + UnityEngine.Random.Range(-1f, 1f), enemyPosition.position.y + 12f + UnityEngine.Random.Range(-1f, 1f), enemyPosition.position.z), Quaternion.identity); }
                }
                catch (NullReferenceException) 
                { 
                    int bruh = UnityEngine.Random.Range(0,2);
                    if (bruh < 1) { Instantiate(KyubuTiles[9], new Vector3(transform.position.x + 9f, transform.position.y + 6f, transform.position.z), Quaternion.identity); }
                    else { Instantiate(KyubuTiles[10], new Vector3(transform.position.x - 22f, transform.position.y + 12f, transform.position.z), Quaternion.identity); }
                }
            }
        }
        if (KyubuTileValue == 100)
        {
            inKyubuKombo100 = true;
            for (int i = 0; i < 50; i++) 
            { 
                StartCoroutine(KyubuKombo(UnityEngine.Random.Range(0, 7)));
                yield return new WaitForSeconds(0.1f);
            }
            inKyubuKombo100 = false;
        }
        yield return null;
    }

    private void KK2NoTarget()
    {
        int dafuq = UnityEngine.Random.Range(0,2);
                float randXOffset = UnityEngine.Random.Range(-1f, 1f);
                float randYOffset = UnityEngine.Random.Range(-1f, 1f);
                if (dafuq < 1) 
                { 
                    Instantiate(KyubuTiles[1], new Vector3(transform.position.x - 22.63f + randXOffset, transform.position.y + randYOffset, transform.position.z), Quaternion.identity); 
                    Instantiate(KyubuTiles[2], new Vector3(transform.position.x + 22.63f + randXOffset, transform.position.y + randYOffset, transform.position.z), Quaternion.identity);  
                }
                else 
                { 
                    Instantiate(KyubuTiles[3], new Vector3(transform.position.x + randXOffset, transform.position.y + 13.63f + randYOffset, transform.position.z), Quaternion.identity); 
                    Instantiate(KyubuTiles[4], new Vector3(transform.position.x + randXOffset, transform.position.y - 13.63f + randYOffset, transform.position.z), Quaternion.identity);  
                }
    }

    private void ShootOneDice()
    {
        diceNumber--;
        SaveDiceNumber();
        Instantiate(dicetypes[DiceValues[0]], diceTransform.position, UnityEngine.Random.rotation);
        CycleThroughDiceValueArray();
    }
    private void ShootTwoDice()
    {
        if (diceNumber >= 2)
        {
            diceNumber -= 2;
            SaveDiceNumber();
            for (int i = 0; i < 2; i++) 
            { 
                Instantiate(MultishotDiceTypes[DiceValues[0]], diceTransform.position, UnityEngine.Random.rotation); 
                CycleThroughDiceValueArray();
            }
        }
        else { ShootOneDice(); }
    }
    private void ShootThreeDice()
    {
        if (diceNumber >= 3)
        {
            diceNumber -= 3;
            SaveDiceNumber();
            for (int i = 0; i < 3; i++) 
            { 
                Instantiate(MultishotDiceTypes[DiceValues[0]], diceTransform.position, UnityEngine.Random.rotation); 
                CycleThroughDiceValueArray();
            }
        }
        else if (diceNumber == 2) { ShootTwoDice(); }
        else { ShootOneDice(); }
    }

    private void ShootOneChargedDice()
    {
        diceNumber--;
        SaveDiceNumber();
        Instantiate(chargedDicetypes[DiceValues[0]], diceTransform.position, UnityEngine.Random.rotation);
        CycleThroughDiceValueArray();
        player.UseChargeAttack();
        if (PlayerPrefs.GetFloat("DiceSpinLevelUp") < 11f) { Instantiate(diceSpinPrefabs[Mathf.RoundToInt(Mathf.Floor(PlayerPrefs.GetFloat("DiceSpinLevel")/10))], transform.position, Quaternion.identity); }
        else { Instantiate(diceSpinPrefabs[11], transform.position, Quaternion.identity); }
    }
    private void ShootTwoChargedDice()
    {
        if (diceNumber >= 2)
        {
            diceNumber -= 2;
            SaveDiceNumber();
            player.UseChargeAttack();
            if (PlayerPrefs.GetFloat("DiceSpinLevelUp") < 11f) { Instantiate(diceSpinPrefabs[Mathf.RoundToInt(Mathf.Floor(PlayerPrefs.GetFloat("DiceSpinLevel")/10))], transform.position, Quaternion.identity); }
            else { Instantiate(diceSpinPrefabs[11], transform.position, Quaternion.identity); }
            for (int i = 0; i < 2; i++) 
            { 
                Instantiate(chargedMultishotDiceTypes[DiceValues[0]], diceTransform.position, UnityEngine.Random.rotation); 
                CycleThroughDiceValueArray();
            }
        }
        else { ShootOneChargedDice(); }
    }
    private void ShootThreeChargedDice()
    {
        if (diceNumber >= 3)
        {
            diceNumber -= 3;
            SaveDiceNumber();
            player.UseChargeAttack();
            if (PlayerPrefs.GetFloat("DiceSpinLevelUp") < 11f) { Instantiate(diceSpinPrefabs[Mathf.RoundToInt(Mathf.Floor(PlayerPrefs.GetFloat("DiceSpinLevel")/10))], transform.position, Quaternion.identity); }
            else { Instantiate(diceSpinPrefabs[11], transform.position, Quaternion.identity); }
            for (int i = 0; i < 3; i++) 
            { 
                Instantiate(chargedMultishotDiceTypes[DiceValues[0]], diceTransform.position, UnityEngine.Random.rotation); 
                CycleThroughDiceValueArray();
            }
        }
        else if (diceNumber == 2) { ShootTwoChargedDice(); }
        else { ShootOneChargedDice(); }
    }

    private IEnumerator ActivateUltimate()
    {
        ultimateBar.ultimateInProgress = true;
        yield return new WaitForSeconds(1f);
        CameraShakeInstance bruh = CameraShaker.Instance.StartShake(1f, 4f, .1f);
        StartCoroutine(AlongUltimate());
        for (float i = 0; i < 5f; i += 0.2f)
        {
            AudioPlayer.PlaySound("ThrowDice");
            if (haveFlan == true && haveCremeBrulee == false) 
            { 
                Instantiate(FakeMultishotLeftDiceTypes[FakeDiceValue], diceTransform.position, UnityEngine.Random.rotation);
                CycleThroughFakeArray();
                Instantiate(FakeMultishotRightDiceTypes[FakeDiceValue], diceTransform.position, UnityEngine.Random.rotation);
                CycleThroughFakeArray();
            }
            else if (haveCremeBrulee) 
            { 
                Instantiate(fakeDiceTypes[FakeDiceValue], diceTransform.position, UnityEngine.Random.rotation);
                CycleThroughFakeArray();
                Instantiate(FakeMultishotLeftDiceTypes[FakeDiceValue], diceTransform.position, UnityEngine.Random.rotation);
                CycleThroughFakeArray();
                Instantiate(FakeMultishotRightDiceTypes[FakeDiceValue], diceTransform.position, UnityEngine.Random.rotation);
                CycleThroughFakeArray();
            }
            else { Instantiate(fakeDiceTypes[FakeDiceValue], diceTransform.position, UnityEngine.Random.rotation); CycleThroughFakeArray(); }
            yield return new WaitForSeconds(0.2f);
        }
        bruh.StartFadeOut(1f);
        ultimateBar.ultimateInProgress = false;
        yield return null;
    }

    private IEnumerator AlongUltimate()
    {
        for (int i = 0; i < 5; i ++)
        {
            if (PlayerPrefs.GetFloat("DiceSpinLevelUp") < 11f) { Instantiate(diceSpinPrefabs[Mathf.RoundToInt(Mathf.Floor(PlayerPrefs.GetFloat("DiceSpinLevel")/10))], transform.position, Quaternion.identity); }
            else { Instantiate(diceSpinPrefabs[11], transform.position, Quaternion.identity); }
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    private void SaveDiceNumber()
    {
        player.diceNumber = diceNumber;
        player.LatterIngameSaveData();
    }

    public void DiceThrowLoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/ingameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        diceNumber = loadedPlayerData.diceNumber;
        playerCooldownTime = loadedPlayerData.playerCooldownTime;
        dicePreviewerLevel = loadedPlayerData.dicePreviewerLevel;
        haveFlan = loadedPlayerData.haveFlan;
        haveCremeBrulee = loadedPlayerData.haveCremeBrulee;
        haveBanhmi = loadedPlayerData.haveBanhmi;
        havePizza = loadedPlayerData.havePizza;
        haveCarrotCake = loadedPlayerData.haveCarrotCake;
        havePastelDeChoclo = loadedPlayerData.havePastelDeChoclo;
    }   
    
    private class PlayerData
    {
        public int diceNumber;
        public float playerCooldownTime;
        public int dicePreviewerLevel;
        public bool haveFlan;
        public bool haveCremeBrulee;
        public bool haveBanhmi;
        public bool havePizza;
        public bool haveCarrotCake;
        public bool havePastelDeChoclo;
    }
}
