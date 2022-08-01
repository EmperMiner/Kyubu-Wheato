using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class snakeBehaviour : MonoBehaviour
{
    public int maxSnakeHealth = 8;
    public int snakeHealth;
    private bool alreadyDamaged;

    public PlayerController player;
    public Text SnakeHealthCounterNumber;

    void Start()
    {
        snakeHealth = maxSnakeHealth;
        SnakeHealthCounterNumber.text = snakeHealth.ToString();
        alreadyDamaged = false;
    }

    void Update()
    {
        if(snakeHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void snakeTakeDamage(int i)
    {
        snakeHealth -= i;
        SnakeHealthCounterNumber.text = snakeHealth.ToString();
        alreadyDamaged = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (alreadyDamaged == false)
        {
            if (collider.gameObject.tag == "6sidedDice1") { snakeTakeDamage(1); }
            if (collider.gameObject.tag == "6sidedDice2") { snakeTakeDamage(2); } 
            if (collider.gameObject.tag == "6sidedDice3") { snakeTakeDamage(3); }
            if (collider.gameObject.tag == "6sidedDice4") { snakeTakeDamage(4); }
            if (collider.gameObject.tag == "6sidedDice5") { snakeTakeDamage(5); }
            if (collider.gameObject.tag == "6sidedDice6") { snakeTakeDamage(6); }        
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        alreadyDamaged = false;
    }
}
