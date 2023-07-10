using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlippedChestScript : MonoBehaviour
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
    [SerializeField] private bool dontUpgradeChargedAttack;

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
            if (player.Wheat >= WheatCost) { OpenFlippedChest(); }
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

    public void OpenFlippedChest()
    {
        if (prePlaced == false) { chestSpawnerScript.ChestSpawned--; }
        if (dontUpgradeChargedAttack == false) 
        {
            float bro = PlayerPrefs.GetFloat("DiceSpinLevel");
            PlayerPrefs.SetFloat("DiceSpinLevel", bro + 6f); 
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
        if (EntreeGachaRoll == 0 && player.havePizza == false) { player.havePizza = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 1 && player.haveCarrotCake == false) { player.haveCarrotCake = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 2 && player.haveFlan == false) { player.haveFlan = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 3 && player.haveCremeBrulee == false && player.haveFlan == true) { player.haveCremeBrulee = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 4 && player.haveBanhmi == false && player.dicePreviewerLevel > 0) { player.haveBanhmi = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 5 && player.haveCupcake == false) { player.haveCupcake = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 6 && player.haveChickenNuggets == false) { player.haveChickenNuggets = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 7 && player.havePastelDeChoclo == false) { player.havePastelDeChoclo = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 8 && player.haveGarlicBread == false) { player.haveGarlicBread = true; CreateImagePopup(EntreeGachaRoll); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 9 && PlayerPrefs.GetInt("IngameRamen") == 0) { PlayerPrefs.SetInt("IngameRamen", 1); CreateImagePopup(EntreeGachaRoll + 10); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 10 && PlayerPrefs.GetInt("IngameSalmon") == 0) { PlayerPrefs.SetInt("IngameSalmon", 1); CreateImagePopup(EntreeGachaRoll + 10); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); } 
        else if (EntreeGachaRoll == 11 && PlayerPrefs.GetInt("IngameSteak") == 0) { PlayerPrefs.SetInt("IngameSteak", 1); CreateImagePopup(EntreeGachaRoll + 10); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll == 12 && PlayerPrefs.GetInt("IngameCheese") == 0) { PlayerPrefs.SetInt("IngameCheese", 1); CreateImagePopup(EntreeGachaRoll + 10); player.StartCoroutine(player.ChestNotification(EntreeGachaRoll)); }
        else if (EntreeGachaRoll <= 8) { ChooseEntree(); }
        else { ChooseStat(); }
    }

    private void ChooseStat()
    {
        int StatGachaRoll = Random.Range(0, 35);
        if (StatGachaRoll <= 2 && player.MoveSpeed <= 3.5f) { player.MoveSpeed += Random.Range(0.6f, 1.2f); CreateImagePopup(9); player.StartCoroutine(player.ChestNotification(22)); }
        else if (StatGachaRoll <= 5) {  int i = Random.Range(player.maxHealth*1/5, player.maxHealth*1/2); player.maxHealth += i; player.healthBar.SetMaxHealth(player.maxHealth); player.UpdateHealth(i*3); CreateImagePopup(10); player.StartCoroutine(player.ChestNotification(23)); }
        else if (StatGachaRoll <= 8) { player.strength += Random.Range(0.75f, 2f); CreateImagePopup(11); player.StartCoroutine(player.ChestNotification(24)); }
        else if (StatGachaRoll <= 11) { player.defense += Random.Range(0.75f, 2f); CreateImagePopup(12); player.StartCoroutine(player.ChestNotification(25)); }
        else if (StatGachaRoll <= 13) { for (int i = 0; i < Random.Range(5, 10); i++) { player.IncreaseDiceNumber(); }; CreateImagePopup(13); player.StartCoroutine(player.ChestNotification(26)); }
        else if (StatGachaRoll <= 15 && player.playerCooldownTime > 5.0f) { player.playerCooldownTime -= 1f; CreateImagePopup(14); player.StartCoroutine(player.ChestNotification(27)); }
        else if (StatGachaRoll <= 17 && player.wheatDroprate < 100f) { player.wheatDroprate += Random.Range(2f, 5f); CreateImagePopup(15); player.StartCoroutine(player.ChestNotification(28)); }
        else if (StatGachaRoll <= 18 && player.diceDroprate > 100) { player.diceDroprate -= Mathf.RoundToInt(Random.Range(player.diceDroprate*0.1f, player.diceDroprate*0.15f)); if(player.diceDroprate < 100) { player.diceDroprate = 100; } CreateImagePopup(16); player.StartCoroutine(player.ChestNotification(29)); }
        else if (StatGachaRoll <= 19 && player.dicePreviewerLevel < 5) { player.dicePreviewerLevel += 1; diceThrowScript.UpdateDicePreviewerUI(); CreateImagePopup(17); player.StartCoroutine(player.ChestNotification(30)); }
        else if (StatGachaRoll <= 34) { player.UpdateWheat(Random.Range(300, 500)); CreateImagePopup(18); player.StartCoroutine(player.ChestNotification(31)); }
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
