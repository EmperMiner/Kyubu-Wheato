using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseBehaviour : MonoBehaviour
{
    public int maxMouseHealth = 5;
    public int mouseHealth;
    private bool alreadyDamaged;

    public PlayerController player;
    public Text MouseHealthCounterNumber;

    void Start()
    {
        mouseHealth = maxMouseHealth;
        MouseHealthCounterNumber.text = mouseHealth.ToString();
        alreadyDamaged = false;
    }

    void Update()
    {
        if(mouseHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void mouseTakeDamage(int i)
    {
        mouseHealth -= i;
        MouseHealthCounterNumber.text = mouseHealth.ToString();
        alreadyDamaged = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (alreadyDamaged == false)
        {
            if (collider.gameObject.tag == "6sidedDice1") { mouseTakeDamage(1); }
            if (collider.gameObject.tag == "6sidedDice2") { mouseTakeDamage(2); } 
            if (collider.gameObject.tag == "6sidedDice3") { mouseTakeDamage(3); }
            if (collider.gameObject.tag == "6sidedDice4") { mouseTakeDamage(4); }
            if (collider.gameObject.tag == "6sidedDice5") { mouseTakeDamage(5); }
            if (collider.gameObject.tag == "6sidedDice6") { mouseTakeDamage(6); }        
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        alreadyDamaged = false;
    }
}
