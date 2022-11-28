using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    private bool CloseEnoughToTotem;
    private PlayerController player; 
    
    private void Start()
    {
        CloseEnoughToTotem = false;
        int destroyOrNot = Random.Range(0,2);
        if (destroyOrNot == 0) { Destroy(gameObject); }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CloseEnoughToTotem) 
        { 
            player.SummonChanceWhale();
            FindObjectOfType<AudioManager>().PlaySound("CSplit");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") { CloseEnoughToTotem = true; }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") { CloseEnoughToTotem = false; }
    }
}
