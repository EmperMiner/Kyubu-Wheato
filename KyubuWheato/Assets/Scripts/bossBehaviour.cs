using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bossBehaviour : MonoBehaviour
{
    public int maxBossHealth = 20;
    public int bossHealth;
    private bool alreadyDamaged;

    public PlayerController player;
    public Text BossHealthCounterNumber;
    public Text DiceCounterNumber;

    public DiceThrow diceThrowScript;

    void Start()
    {
        bossHealth = maxBossHealth;
        BossHealthCounterNumber.text = bossHealth.ToString();
        alreadyDamaged = false;
    }

    void Update()
    {
        if(bossHealth <= 0)
        {
            Destroy(gameObject);
            diceThrowScript.diceNumber++;
            DiceCounterNumber.text = diceThrowScript.diceNumber.ToString();
        }
    }

    private void bossTakeDamage(int i)
    {
        bossHealth -= i;
        BossHealthCounterNumber.text = bossHealth.ToString();
        alreadyDamaged = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (alreadyDamaged == false)
        {
            if (collider.gameObject.tag == "6sidedDice1") { bossTakeDamage(1); }
            if (collider.gameObject.tag == "6sidedDice2") { bossTakeDamage(2); } 
            if (collider.gameObject.tag == "6sidedDice3") { bossTakeDamage(3); }
            if (collider.gameObject.tag == "6sidedDice4") { bossTakeDamage(4); }
            if (collider.gameObject.tag == "6sidedDice5") { bossTakeDamage(5); }
            if (collider.gameObject.tag == "6sidedDice6") { bossTakeDamage(6); }        
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        alreadyDamaged = false;
    }
}
