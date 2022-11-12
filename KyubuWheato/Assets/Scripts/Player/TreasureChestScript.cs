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
    [SerializeField] private int WheatCost;
    [SerializeField] private int StatChance;
    [SerializeField] private Sprite[] UpgradeSpritePopup;
    [SerializeField] private SpriteRenderer ImagePopup;
    [SerializeField] private GameObject ImagePopupObject;
    [SerializeField] private bool prePlaced;

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

    private void OpenTreasureChest()
    {
        if (prePlaced == false) { chestSpawnerScript.ChestSpawned--; }
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
        int EntreeGachaRoll = Random.Range(0, 12);
        int NewEntreeChance = Random.Range(0, 3);
        if (EntreeGachaRoll == 0 && player.havePizza == false) { player.havePizza = true; CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 1 && player.haveCarrotCake == false) { player.haveCarrotCake = true; CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 2 && player.haveFlan == false) { player.haveFlan = true; CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 3 && player.haveCremeBrulee == false && player.haveFlan == true) { player.haveCremeBrulee = true; CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 4 && player.haveBanhmi == false && player.dicePreviewerLevel > 0) { player.haveBanhmi = true; CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 5 && player.haveCupcake == false) { player.haveCupcake = true; CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 6 && player.haveChickenNuggets == false) { player.haveChickenNuggets = true; CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 7 && player.havePastelDeChoclo == false) { player.havePastelDeChoclo = true; CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 8 && player.haveGarlicBread == false) { player.haveGarlicBread = true; CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 9 && PlayerPrefs.GetInt("IngameRamen") == 0 && NewEntreeChance == 0) { PlayerPrefs.SetInt("IngameRamen", 1); CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll == 10 && PlayerPrefs.GetInt("IngameSalmon") == 0 && NewEntreeChance == 0) { PlayerPrefs.SetInt("IngameSalmon", 1); CreateImagePopup(EntreeGachaRoll); } 
        else if (EntreeGachaRoll == 11 && PlayerPrefs.GetInt("IngameSteak") == 0 && NewEntreeChance == 0) { PlayerPrefs.SetInt("IngameSteak", 1); CreateImagePopup(EntreeGachaRoll); }
        else if (EntreeGachaRoll <= 8) { ChooseEntree(); }
        else { ChooseStat(); }
    }

    private void ChooseStat()
    {
        int StatGachaRoll = Random.Range(0, 20);
        if (StatGachaRoll <= 2 && player.MoveSpeed <= 3.4f) { player.MoveSpeed += Random.Range(0.1f, 0.3f); CreateImagePopup(9); }
        else if (StatGachaRoll <= 5) { int i = Random.Range(2, 6); player.maxHealth += i; player.healthBar.SetMaxHealth(player.maxHealth); player.UpdateHealth(i); CreateImagePopup(10); }
        else if (StatGachaRoll <= 8) { player.strength += Random.Range(0.25f, 0.75f); CreateImagePopup(11); }
        else if (StatGachaRoll <= 11) { player.defense += Random.Range(0.25f, 0.75f); CreateImagePopup(12); }
        else if (StatGachaRoll <= 13) { for (int i = 0; i < Random.Range(1, 4); i++) { player.IncreaseDiceNumber(); }; CreateImagePopup(13); }
        else if (StatGachaRoll <= 15 && player.playerCooldownTime > 5.0f) { player.playerCooldownTime -= 0.5f; CreateImagePopup(14); }
        else if (StatGachaRoll <= 17 && player.wheatDroprate < 100f) { player.wheatDroprate += Random.Range(0.1f, 2.0f); CreateImagePopup(15); }
        else if (StatGachaRoll <= 18 && player.diceDroprate > 1) { player.diceDroprate -= Random.Range(10, 31); CreateImagePopup(16); }
        else if (StatGachaRoll <= 19 && player.dicePreviewerLevel < 5) { player.dicePreviewerLevel += 1; diceThrowScript.UpdateDicePreviewerUI(); CreateImagePopup(17); }
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
