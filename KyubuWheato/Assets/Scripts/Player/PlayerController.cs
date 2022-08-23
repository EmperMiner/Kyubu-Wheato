using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerController : MonoBehaviour
{

    public float MoveSpeed;
    public int maxHealth;
    public int playerHealth;
    public float strength;
    public float defense;
    public float wheatDroprate;
    public int Wheat = 0;

    public Rigidbody2D playerRB ;
    Vector2 movement;
    public Animator animator;
    public Renderer spriteRenderer;

    [SerializeField] private HealthBar healthBar;

    public DiceThrow diceThrowScript;
    public Text DiceCounterNumber;
    public Text WheatCounterNumber;

    public GameObject crosshair;
    public GameObject diceThrower;

    private void Awake()
    {       
        LoadData();
        playerHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if ( movement.x!=0 ) { animator.SetFloat("Horizontal", movement.x); }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
            playerRB.MovePosition(playerRB.position + movement * MoveSpeed * Time.fixedDeltaTime);
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

    private void IncreaseDiceNumber()
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
        Time.timeScale = 0f;
        crosshair.SetActive(false);
        diceThrower.SetActive(false);
    }

    private class PlayerData
    {
        public float MoveSpeed;
        public int maxHealth;
        public float strength;
        public float defense;
        public float wheatDroprate;
        public int Wheat;
    }

    IEnumerator FlashingHealthBar()
    {
        healthBar.HealthBarFlash(true);
        yield return new WaitForSeconds(0.25f);
        healthBar.HealthBarFlash(false);
        yield return null;
    }
}
