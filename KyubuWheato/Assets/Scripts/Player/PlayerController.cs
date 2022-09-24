using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerController : MonoBehaviour
{
    private float shopMoveSpeed;
    private int shopMaxHealth;
    private int shopPlayerHealth;
    private float shopStrength;
    private int shopWheat;
    private int shopDiceNumber;
    private float shopPlayerCooldownTime;
    private float shopDefense;
    private float shopWheatDroprate;
    private int shopDicePreviewerLevel;
    private int shopDiceDroprate;
    private bool shopHavePizza;
    private bool shopHaveCarrotCake;
    private bool shopHaveFlan;
    private bool shopHaveCremeBrulee;
    private bool shopHaveBanhmi;
    private bool shopHaveCupcake;
    private bool shopHaveChickenNuggets;
    private bool shopHavePastelDeChoclo;
    private bool shopHaveGarlicBread;

    public float MoveSpeed;
    public int maxHealth;
    private int playerHealth;
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

    [SerializeField] private DiceThrow diceThrowScript;
    [SerializeField] private DicePadSpawner dicePadSpawnerScript;
    [SerializeField] private UltimateBarCharge ultimateScript;
    [SerializeField] private CooldownBar cooldownBarScript;
    [SerializeField] private Text DiceCounterNumber;
    [SerializeField] private Text WheatCounterNumber;
    [SerializeField] private GameOverScreen gameOverScript;

    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject diceThrower;
    
    [SerializeField] private GameObject[] BroomPrefabs;
    private bool HasStrengthTemporaryBuff;
    private bool HasDefenseTemporaryBuff;
    private bool HasSpeedTemporaryBuff;
    public bool AllEntrees = false;

    

    private void Awake()
    {       
        AudioPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        if (FirstLevelSave) 
        { 
            LoadData();
            FirstIngameSaveData(); 
        }
        else
        {
            SubtractTemporaryBuff();
        }
        IngameLoadData();

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
        Wheat = 0;
        healthBar.SetMaxHealth(maxHealth);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform; 

        LeftMapLimit = LeftCamLimit - 7.5f;
        RightMapLimit = RightCamLimit + 7.5f;
        UpperMapLimit = UpperCamLimit + 3f;
        LowerMapLimit = LowerCamLimit - 3f;
    }

    private void Update()
    {
        if (playerAlive)
        {
            LimitCamera();
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if ( movement.x!=0 ) { animator.SetFloat("Horizontal", movement.x); }
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (Input.GetKeyDown(KeyCode.R)) { CraftBread(); }

            if (haveGarlicBread && triggeredBroom == false) { StartCoroutine(RollMode());}
            if (CurrentMode == 1 && BroomInMode == false) { StartCoroutine(AttackMode());}
            if (CurrentMode == 2 && BroomInMode == false) { StartCoroutine(HealMode());}
            if (CurrentMode == 3 && BroomInMode == false) { StartCoroutine(StatBuffMode());}
            
            if (diceThrowScript.inKyubuKombo100) { UpdateHealth(maxHealth); }
            if (havePizza && haveCarrotCake && haveFlan && haveCremeBrulee && haveBanhmi && haveCupcake && haveChickenNuggets && havePastelDeChoclo && haveGarlicBread) { AllEntrees = true; }
            else { AllEntrees = false; }

            if (playerHealth == 0) { GameOver(); }
        }
    }

    public void UpdateValues()
    {
        LatterIngameSaveData();
        diceThrowScript.DiceThrowLoadData();
        dicePadSpawnerScript.DicePadLoadData();
        ultimateScript.UltimateLoadData();
        cooldownBarScript.CooldownBarLoadData();
    }

    private void FixedUpdate()
    {
        playerRB.MovePosition(playerRB.position + movement * MoveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "6sidedDice1") { IncreaseDiceNumber(); AudioPlayer.PlaySound("DicePickup"); } 
        if (collider.gameObject.tag == "6sidedDice2") { IncreaseDiceNumber(); AudioPlayer.PlaySound("DicePickup"); } 
        if (collider.gameObject.tag == "6sidedDice3") { IncreaseDiceNumber(); AudioPlayer.PlaySound("DicePickup"); } 
        if (collider.gameObject.tag == "6sidedDice4") { IncreaseDiceNumber(); AudioPlayer.PlaySound("DicePickup"); } 
        if (collider.gameObject.tag == "6sidedDice5") { IncreaseDiceNumber(); AudioPlayer.PlaySound("DicePickup"); } 
        if (collider.gameObject.tag == "6sidedDice6") { IncreaseDiceNumber(); AudioPlayer.PlaySound("DicePickup"); } 

        if (collider.gameObject.tag == "DiceTile1") { AudioPlayer.PlaySound("DicePad"); }
        if (collider.gameObject.tag == "DiceTile2") { AudioPlayer.PlaySound("DicePad"); }
        if (collider.gameObject.tag == "DiceTile3") { AudioPlayer.PlaySound("DicePad"); }
        if (collider.gameObject.tag == "DiceTile4") { AudioPlayer.PlaySound("DicePad"); }
        if (collider.gameObject.tag == "DiceTile5") { AudioPlayer.PlaySound("DicePad"); }
        if (collider.gameObject.tag == "DiceTile6") { AudioPlayer.PlaySound("DicePad"); }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "DiceTile1") { diceThrowScript.isOnDiceTile1 = true; }
        if (other.gameObject.tag == "DiceTile2") { diceThrowScript.isOnDiceTile2 = true; }
        if (other.gameObject.tag == "DiceTile3") { diceThrowScript.isOnDiceTile3 = true; }
        if (other.gameObject.tag == "DiceTile4") { diceThrowScript.isOnDiceTile4 = true; }
        if (other.gameObject.tag == "DiceTile5") { diceThrowScript.isOnDiceTile5 = true; }
        if (other.gameObject.tag == "DiceTile6") { diceThrowScript.isOnDiceTile6 = true; }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "DiceTile1") { diceThrowScript.isOnDiceTile1 = false; AudioPlayer.PlaySound("DicePad"); }
        if (other.gameObject.tag == "DiceTile2") { diceThrowScript.isOnDiceTile2 = false; AudioPlayer.PlaySound("DicePad"); }
        if (other.gameObject.tag == "DiceTile3") { diceThrowScript.isOnDiceTile3 = false; AudioPlayer.PlaySound("DicePad"); }
        if (other.gameObject.tag == "DiceTile4") { diceThrowScript.isOnDiceTile4 = false; AudioPlayer.PlaySound("DicePad"); }
        if (other.gameObject.tag == "DiceTile5") { diceThrowScript.isOnDiceTile5 = false; AudioPlayer.PlaySound("DicePad"); }
        if (other.gameObject.tag == "DiceTile6") { diceThrowScript.isOnDiceTile6 = false; AudioPlayer.PlaySound("DicePad"); }
    }

    private void LimitCamera()
    {
        if (transform.position.x > RightCamLimit && transform.position.y > UpperCamLimit) { mainCam.position = new Vector3(RightCamLimit, UpperCamLimit, -10); }
        else if (transform.position.x < LeftCamLimit && transform.position.y > UpperCamLimit) { mainCam.position = new Vector3(LeftCamLimit, UpperCamLimit, -10); }
        else if (transform.position.x > RightCamLimit && transform.position.y < LowerCamLimit) { mainCam.position = new Vector3(RightCamLimit, LowerCamLimit, -10); }
        else if (transform.position.x < LeftCamLimit && transform.position.y < LowerCamLimit) { mainCam.position = new Vector3(LeftCamLimit, LowerCamLimit, -10); }
        else if (transform.position.x > RightCamLimit) { mainCam.position = new Vector3(RightCamLimit, transform.position.y, -10); }
        else if (transform.position.x < LeftCamLimit) { mainCam.position = new Vector3(LeftCamLimit, transform.position.y, -10); }
        else if (transform.position.y > UpperCamLimit) { mainCam.position = new Vector3(transform.position.x, UpperCamLimit, -10); }
        else if (transform.position.y < LowerCamLimit) { mainCam.position = new Vector3(transform.position.x, LowerCamLimit, -10); }
        else {}
    }

    private void CraftBread()
    {
        if (Wheat >= 3 && playerHealth < maxHealth) { UpdateWheat(-3); UpdateHealth(10); LatterIngameSaveData();}
        else if (playerHealth == maxHealth) { Debug.Log("You're At Full Health"); }
        else { Debug.Log("You Don't Have Enough Wheat To Craft Bread"); }
    }

    public void IncreaseDiceNumber()
    {
        diceNumber++;
        DiceCounterNumber.text = diceNumber.ToString();
        UpdateValues();
    }

    public void UpdateHealth(int healthMod)
    {
        playerHealth += healthMod;
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

    public void UpdateWheat(int wheatMod) 
    {
        Wheat += wheatMod;
        WheatCounterNumber.text = Wheat.ToString();
        LatterIngameSaveData();
    }

    public void GameOver()
    {
        playerAlive = false;
        SaveData();
        Time.timeScale = 0f;
        crosshair.SetActive(false);
        diceThrower.SetActive(false);
        gameOverScript.GameOverTrigger(Wheat);
    }

    IEnumerator FlashingHealthBar()
    {
        healthBar.HealthBarFlash(true);
        yield return new WaitForSeconds(0.25f);
        healthBar.HealthBarFlash(false);
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
            BroomInMode = true;
            InHealMode = true;
            yield return new WaitForSeconds(UnityEngine.Random.Range(20f,60f));
            InHealMode = false;
            StartCoroutine(RollMode());
        }
        yield return null;
    }
    IEnumerator StatBuffMode()
    {
        if (playerAlive) 
        {
            BroomInMode = true;
            for (int b = 0; b < 5; b++)
            {
                AudioPlayer.PlaySound("BroomBuff");
                int PickRandomStat = UnityEngine.Random.Range(0,5);
                if (PickRandomStat == 0) { strength += 2f; HasStrengthTemporaryBuff = true; LatterIngameSaveData(); }
                if (PickRandomStat == 1) { defense += 2f; HasDefenseTemporaryBuff = true; LatterIngameSaveData(); }
                if (PickRandomStat == 2) { MoveSpeed += 1f; HasSpeedTemporaryBuff = true; LatterIngameSaveData(); }
                if (PickRandomStat == 3) { IncreaseDiceNumber(); }
                if (PickRandomStat == 4) { RegenStop = false; StartCoroutine(Regen()); }
                yield return new WaitForSeconds(UnityEngine.Random.Range(4f,12f));
                SubtractTemporaryBuff();
                if (PickRandomStat == 4) { RegenStop = true; }
            }
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
        UpdateHealth(2);
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f,4f));
        if (RegenStop == false) { StartCoroutine(Regen()); }
        yield return null;
    }

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        shopMoveSpeed = loadedPlayerData.MoveSpeed;
        shopMaxHealth = loadedPlayerData.maxHealth;
        shopPlayerHealth = loadedPlayerData.playerHealth;
        shopStrength = loadedPlayerData.strength;
        shopWheat = loadedPlayerData.Wheat;
        shopDiceNumber = loadedPlayerData.diceNumber;
        shopPlayerCooldownTime = loadedPlayerData.playerCooldownTime;
        shopDefense = loadedPlayerData.defense;
        shopWheatDroprate = loadedPlayerData.wheatDroprate;
        shopDicePreviewerLevel = loadedPlayerData.dicePreviewerLevel;
        shopDiceDroprate = loadedPlayerData.diceDroprate;
        shopHavePizza = loadedPlayerData.havePizza;
        shopHaveCarrotCake = loadedPlayerData.haveCarrotCake;
        shopHaveFlan = loadedPlayerData.haveFlan;
        shopHaveCremeBrulee = loadedPlayerData.haveCremeBrulee;
        shopHaveBanhmi = loadedPlayerData.haveBanhmi;
        shopHaveCupcake = loadedPlayerData.haveCupcake;
        shopHaveChickenNuggets = loadedPlayerData.haveChickenNuggets;
        shopHavePastelDeChoclo = loadedPlayerData.havePastelDeChoclo;
        shopHaveGarlicBread = loadedPlayerData.haveGarlicBread;
    }

    private void SaveData()
    {
        PlayerData savingPlayerData = new PlayerData();

        savingPlayerData.MoveSpeed = shopMoveSpeed;
        savingPlayerData.maxHealth = shopMaxHealth;
        savingPlayerData.playerHealth = shopPlayerHealth;
        savingPlayerData.strength = shopStrength;
        savingPlayerData.Wheat = shopWheat + Wheat;
        savingPlayerData.diceNumber = shopDiceNumber;
        savingPlayerData.playerCooldownTime = shopPlayerCooldownTime;
        savingPlayerData.defense = shopDefense;
        savingPlayerData.wheatDroprate = shopWheatDroprate;
        savingPlayerData.dicePreviewerLevel = shopDicePreviewerLevel;
        savingPlayerData.diceDroprate = shopDiceDroprate;
        savingPlayerData.havePizza = shopHavePizza;
        savingPlayerData.haveCarrotCake = shopHaveCarrotCake;
        savingPlayerData.haveFlan = shopHaveFlan;
        savingPlayerData.haveCremeBrulee = shopHaveCremeBrulee;
        savingPlayerData.haveBanhmi = shopHaveBanhmi;
        savingPlayerData.haveCupcake = shopHaveCupcake;
        savingPlayerData.haveChickenNuggets = shopHaveChickenNuggets;
        savingPlayerData.havePastelDeChoclo = shopHavePastelDeChoclo;
        savingPlayerData.haveGarlicBread = shopHaveGarlicBread;


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
    }
    private void FirstIngameSaveData()
    {
        PlayerData savingPlayerData = new PlayerData();

        savingPlayerData.MoveSpeed = shopMoveSpeed;
        savingPlayerData.maxHealth = shopMaxHealth;
        savingPlayerData.playerHealth = shopPlayerHealth;
        savingPlayerData.strength = shopStrength;
        savingPlayerData.Wheat = 0;
        savingPlayerData.diceNumber = shopDiceNumber;
        savingPlayerData.playerCooldownTime = shopPlayerCooldownTime;
        savingPlayerData.defense = shopDefense;
        savingPlayerData.wheatDroprate = shopWheatDroprate;
        savingPlayerData.dicePreviewerLevel = shopDicePreviewerLevel;
        savingPlayerData.diceDroprate = shopDiceDroprate;
        savingPlayerData.havePizza = shopHavePizza;
        savingPlayerData.haveCarrotCake = shopHaveCarrotCake;
        savingPlayerData.haveFlan = shopHaveFlan;
        savingPlayerData.haveCremeBrulee = shopHaveCremeBrulee;
        savingPlayerData.haveBanhmi = shopHaveBanhmi;
        savingPlayerData.haveCupcake = shopHaveCupcake;
        savingPlayerData.haveChickenNuggets = shopHaveChickenNuggets;
        savingPlayerData.havePastelDeChoclo = shopHavePastelDeChoclo;
        savingPlayerData.haveGarlicBread = shopHaveGarlicBread;

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

        string json = JsonUtility.ToJson(savingPlayerData);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/ingameSaveData.json", json);    
    }
    

    private class PlayerData
    {
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
    }
}
