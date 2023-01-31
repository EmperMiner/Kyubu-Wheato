using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreasureChestScript : MonoBehaviour
{
    private bool CloseEnoughToChest;
    private PlayerController player; 
    private DiceThrow diceThrowScript;
    private TreasureChestSpawner chestSpawnerScript; 

    private bool CheckForValidSpawn = true;
    
    [SerializeField] private GameObject[] ChestTypes;

    private TextMeshProUGUI SmallText;
    [SerializeField] private int ChestRarity;
    [SerializeField] private int StatChance;
    [SerializeField] private Sprite[] UpgradeSpritePopup;
    [SerializeField] private SpriteRenderer ImagePopup;
    [SerializeField] private GameObject ImagePopupObject;
    [SerializeField] private bool prePlaced;
    [SerializeField] private bool dontUpgradeChargedAttack;
    private int WheatCost;

    private void Start()
    {  
        SmallText = GameObject.FindGameObjectWithTag("IngameNotifText").GetComponent<TextMeshProUGUI>();
        SmallText.text = "";
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        diceThrowScript = GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceThrow>();
        chestSpawnerScript = GameObject.FindGameObjectWithTag("ChestManager").GetComponent<TreasureChestSpawner>();
        CloseEnoughToChest = false;
        StartCoroutine(ValidSpawn());
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CloseEnoughToChest) 
        { 
            if (ChestRarity == 0) 
            { 
                if (player.Wheat*0.07f < 10) { WheatCost = 10; }
                else { WheatCost = Mathf.RoundToInt(player.Wheat*0.1f); }
            }
            else if (ChestRarity == 1)
            {
                if (player.Wheat*0.14f < 20) { WheatCost = 20; }
                else { WheatCost = Mathf.RoundToInt(player.Wheat*0.15f); }
            }
            else 
            {
                if (player.Wheat*0.25f < 35) { WheatCost = 35; }
                else { WheatCost = Mathf.RoundToInt(player.Wheat*0.25f); }
            }
            
            if (player.Wheat >= WheatCost) { OpenTreasureChest(); }
            else { StartCoroutine(NotifTextWarning()); }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        bool TouchIllegalObject = other.gameObject.tag == "MapCollider" || other.gameObject.tag == "enemyMouse" || other.gameObject.tag == "chest";
        if (TouchIllegalObject && CheckForValidSpawn) 
        {   
            Instantiate(ChestTypes[Random.Range(0,ChestTypes.Length)], new Vector3(Random.Range(player.LeftMapLimit,player.RightMapLimit), Random.Range(player.LowerMapLimit, player.UpperMapLimit), 0), Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player") { CloseEnoughToChest = true; }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") { CloseEnoughToChest = false; }
    }

    public void OpenTreasureChest()
    {
        if (prePlaced == false) { chestSpawnerScript.ChestSpawned--; }
        if (dontUpgradeChargedAttack == false) { 
            if (StatChance == 95) 
            { 
                float bro = PlayerPrefs.GetFloat("DiceSpinLevel");
                PlayerPrefs.SetFloat("DiceSpinLevel", bro + 1f); 
            }
            if (StatChance == 88) 
            { 
                float bro = PlayerPrefs.GetFloat("DiceSpinLevel");
                PlayerPrefs.SetFloat("DiceSpinLevel", bro + 2f); 
            }
            if (StatChance == 75) 
            { 
                float bro = PlayerPrefs.GetFloat("DiceSpinLevel");
                PlayerPrefs.SetFloat("DiceSpinLevel", bro + 3f); 
            }
        }
        FindObjectOfType<AudioManager>().PlaySound("ChestOpening");
        player.UpdateWheat(-WheatCost);
        if (player.AllEntrees) 
        {
            ChooseStat();
        }
        else 
        {
            int FirstGachaRoll = Random.Range(1, 101);
            if (FirstGachaRoll > StatChance) { ChooseEntree(); }
            else { ChooseStat(); }
        }
        player.UpdateValues();
        Destroy(gameObject);
    }

    private void ChooseEntree()
    {
        int EntreeGachaRoll = Random.Range(0, 13);
        int NewEntreeChance = Random.Range(0, 7);
        if (EntreeGachaRoll == 0 && player.havePizza == false) { player.havePizza = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 1 && player.haveCarrotCake == false) { player.haveCarrotCake = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 2 && player.haveFlan == false) { player.haveFlan = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 3 && player.haveCremeBrulee == false && player.haveFlan == true) { player.haveCremeBrulee = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 4 && player.haveBanhmi == false && player.dicePreviewerLevel > 0) { player.haveBanhmi = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 5 && player.haveCupcake == false) { player.haveCupcake = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 6 && player.haveChickenNuggets == false) { player.haveChickenNuggets = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 7 && player.havePastelDeChoclo == false) { player.havePastelDeChoclo = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 8 && player.haveGarlicBread == false) { player.haveGarlicBread = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 9 && PlayerPrefs.GetInt("IngameRamen") == 0 && NewEntreeChance == 0) { PlayerPrefs.SetInt("IngameRamen", 1); CreateImagePopup(EntreeGachaRoll + 9); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 10 && PlayerPrefs.GetInt("IngameSalmon") == 0 && NewEntreeChance == 0) { PlayerPrefs.SetInt("IngameSalmon", 1); CreateImagePopup(EntreeGachaRoll + 9); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); } 
        else if (EntreeGachaRoll == 11 && PlayerPrefs.GetInt("IngameSteak") == 0 && NewEntreeChance == 0) { PlayerPrefs.SetInt("IngameSteak", 1); CreateImagePopup(EntreeGachaRoll + 9); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 12 && PlayerPrefs.GetInt("IngameCheese") == 0 && NewEntreeChance == 0) { PlayerPrefs.SetInt("IngameCheese", 1); CreateImagePopup(EntreeGachaRoll + 9); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll <= 8) { ChooseEntree(); }
        else { ChooseStat(); }
    }

    private void ChooseStat()
    {
        int StatGachaRoll = Random.Range(0, 20);
        if (StatGachaRoll <= 2 && player.MoveSpeed <= 3.4f) { player.MoveSpeed += Random.Range(0.1f, 0.3f + 0.2f*ChestRarity); CreateImagePopup(9); player.StartCoroutine(player.ChestNotification(13)); }
        else if (StatGachaRoll <= 5) { int i = Random.Range(player.maxHealth*1/10, player.maxHealth*1/2 + (player.maxHealth/5)*ChestRarity); player.maxHealth += i; player.healthBar.SetMaxHealth(player.maxHealth); player.UpdateHealth(i); CreateImagePopup(10); player.StartCoroutine(player.ChestNotification(14)); }
        else if (StatGachaRoll <= 8) { player.strength += Random.Range(0.25f, 0.75f + 0.5f*ChestRarity); CreateImagePopup(11); player.StartCoroutine(player.ChestNotification(15)); }
        else if (StatGachaRoll <= 11) { player.defense += Random.Range(0.25f, 0.75f + 0.5f*ChestRarity); CreateImagePopup(12); player.StartCoroutine(player.ChestNotification(16)); }
        else if (StatGachaRoll <= 13) { for (int i = 0; i < Random.Range(2, 6 + 3*ChestRarity); i++) { player.IncreaseDiceNumber(); }; CreateImagePopup(13); player.StartCoroutine(player.ChestNotification(17)); }
        else if (StatGachaRoll <= 15 && player.playerCooldownTime > 5.0f) { player.playerCooldownTime -= 0.5f; CreateImagePopup(14); player.StartCoroutine(player.ChestNotification(18)); }
        else if (StatGachaRoll <= 17 && player.wheatDroprate < 100f) { player.wheatDroprate += Random.Range(0.1f, 2.0f); CreateImagePopup(15); player.StartCoroutine(player.ChestNotification(19)); }
        else if (StatGachaRoll <= 18 && player.diceDroprate > 1) { player.diceDroprate -= Random.Range(10, 31); CreateImagePopup(16); player.StartCoroutine(player.ChestNotification(20)); }
        else if (StatGachaRoll <= 19 && player.dicePreviewerLevel < 5) { player.dicePreviewerLevel += 1; diceThrowScript.UpdateDicePreviewerUI(); CreateImagePopup(17); player.StartCoroutine(player.ChestNotification(21)); }
        else { ChooseStat(); }
    }
    
    private void CreateImagePopup(int i)
    {
        ImagePopup.sprite = UpgradeSpritePopup[i];
        Instantiate(ImagePopupObject, player.transform.position, Quaternion.identity, player.transform);
    }

    private IEnumerator NotifTextWarning()
    {
        FindObjectOfType<AudioManager>().PlaySound("UIButtonError");
        SmallText.text = "Needs " + WheatCost + " Wheat";
        yield return new WaitForSeconds(1f);
        SmallText.text = "";
        yield return null;
    }

    private IEnumerator ValidSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        CheckForValidSpawn = false;
        yield return null;
    }
}
