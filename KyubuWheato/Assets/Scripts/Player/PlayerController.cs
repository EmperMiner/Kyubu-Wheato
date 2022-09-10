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
    public int playerHealth;
    public float strength;
    public float defense;
    public float wheatDroprate;
    public int Wheat;

    public bool playerAlive;
    
    private Transform mainCam;
    [SerializeField] private float LeftCamLimit;
    [SerializeField] private float RightCamLimit;
    [SerializeField] private float UpperCamLimit;
    [SerializeField] private float LowerCamLimit;
    [SerializeField] private Rigidbody2D playerRB ;
    private Vector2 movement;
    [SerializeField] private Animator animator;
    public Renderer spriteRenderer;

    [SerializeField] private HealthBar healthBar;

    [SerializeField] private DiceThrow diceThrowScript;
    [SerializeField] private Text DiceCounterNumber;
    [SerializeField] private Text WheatCounterNumber;
    [SerializeField] private GameOverScreen gameOverScript;

    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject diceThrower;
    
    [SerializeField] private GameObject[] DiceRayTest;
    private void Awake()
    {       
        LoadData();
        Time.timeScale = 1f;
        playerAlive = true;
        playerHealth = maxHealth;
        crosshair.SetActive(true);
        diceThrower.SetActive(true);
        Wheat = 0;
        healthBar.SetMaxHealth(maxHealth);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform; 
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

            if (Input.GetKeyDown(KeyCode.E)) { CraftBread(); }
            
            if (diceThrowScript.inKyubuKombo100) { UpdateHealth(maxHealth); }
            if (playerHealth == 0) { GameOver(); }
        }
    }

    private void FixedUpdate()
    {
            playerRB.MovePosition(playerRB.position + movement * MoveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "6sidedDice1") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice2") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice3") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice4") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice5") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice6") { IncreaseDiceNumber(); } 
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
        if (other.gameObject.tag == "DiceTile1") { diceThrowScript.isOnDiceTile1 = false; }
        if (other.gameObject.tag == "DiceTile2") { diceThrowScript.isOnDiceTile2 = false; }
        if (other.gameObject.tag == "DiceTile3") { diceThrowScript.isOnDiceTile3 = false; }
        if (other.gameObject.tag == "DiceTile4") { diceThrowScript.isOnDiceTile4 = false; }
        if (other.gameObject.tag == "DiceTile5") { diceThrowScript.isOnDiceTile5 = false; }
        if (other.gameObject.tag == "DiceTile6") { diceThrowScript.isOnDiceTile6 = false; }
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
        if (Wheat >= 3 && playerHealth < maxHealth) { UpdateWheat(-3); UpdateHealth(10); }
        else if (playerHealth == maxHealth) { Debug.Log("You're At Full Health"); }
        else { Debug.Log("You Don't Have Enough Wheat To Craft Bread"); }
    }

    public void IncreaseDiceNumber()
    {
        diceThrowScript.diceNumber++;
        DiceCounterNumber.text = diceThrowScript.diceNumber.ToString();
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

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        MoveSpeed = loadedPlayerData.MoveSpeed;
        maxHealth = loadedPlayerData.maxHealth;
        strength = loadedPlayerData.strength;
        defense = loadedPlayerData.defense;
        wheatDroprate = loadedPlayerData.wheatDroprate;

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
