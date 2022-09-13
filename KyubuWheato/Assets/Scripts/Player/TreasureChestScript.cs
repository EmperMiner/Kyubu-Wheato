using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreasureChestScript : MonoBehaviour
{
    private bool CloseEnoughToChest;
    private PlayerController player;
    private DiceThrow diceThrowScript;
    
    [SerializeField] private Transform SmallTextPopup;
    [SerializeField] private TextMeshPro SmallText;
    [SerializeField] private int WheatCost;
    [SerializeField] private int StatChance;
    [SerializeField] private Sprite[] UpgradeSpritePopup;
    [SerializeField] private SpriteRenderer ImagePopup;
    [SerializeField] private GameObject ImagePopupObject;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        diceThrowScript = GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceThrow>();
        CloseEnoughToChest = false;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CloseEnoughToChest) 
        { 
            if (player.Wheat >= WheatCost) { OpenTreasureChest(); }
            else 
            { 
                SmallText.text = "Not Enough Wheat";
                Instantiate(SmallTextPopup, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") { CloseEnoughToChest = true; }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") { CloseEnoughToChest = false; }
    }

    private void OpenTreasureChest()
    {
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
        int EntreeGachaRoll = Random.Range(0, 9);
        if (EntreeGachaRoll == 0 && player.havePizza == false) { player.havePizza = true; CreateImagePopup(0); }
        else if (EntreeGachaRoll == 1 && player.haveCarrotCake == false) { player.haveCarrotCake = true; CreateImagePopup(1); }
        else if (EntreeGachaRoll == 2 && player.haveFlan == false) { player.haveFlan = true; CreateImagePopup(2); }
        else if (EntreeGachaRoll == 3 && player.haveCremeBrulee == false) { player.haveCremeBrulee = true; CreateImagePopup(3); }
        else if (EntreeGachaRoll == 4 && player.haveBanhmi == false) { player.haveBanhmi = true; CreateImagePopup(4); }
        else if (EntreeGachaRoll == 5 && player.haveCupcake == false) { player.haveCupcake = true; CreateImagePopup(5); }
        else if (EntreeGachaRoll == 6 && player.haveChickenNuggets == false) { player.haveChickenNuggets = true; CreateImagePopup(6); }
        else if (EntreeGachaRoll == 7 && player.havePastelDeChoclo == false) { player.havePastelDeChoclo = true; CreateImagePopup(7); }
        else if (EntreeGachaRoll == 8 && player.haveGarlicBread == false) { player.haveGarlicBread = true; CreateImagePopup(8); }
        else { ChooseEntree(); }
    }

    private void ChooseStat()
    {
        int StatGachaRoll = Random.Range(0, 20);
        if (StatGachaRoll <= 2 && player.MoveSpeed <= 3.4f) { player.MoveSpeed += Random.Range(0.1f, 0.3f); CreateImagePopup(9); }
        else if (StatGachaRoll <= 5) { int i = Random.Range(2, 6); player.maxHealth += i; player.healthBar.SetMaxHealth(player.maxHealth); player.UpdateHealth(i); CreateImagePopup(10); }
        else if (StatGachaRoll <= 8) { player.strength += Random.Range(0.25f, 0.75f); CreateImagePopup(11); }
        else if (StatGachaRoll <= 11) { player.defense += Random.Range(0.25f, 0.75f); CreateImagePopup(12); }
        else if (StatGachaRoll <= 13) { player.IncreaseDiceNumber(); CreateImagePopup(13); }
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
}
