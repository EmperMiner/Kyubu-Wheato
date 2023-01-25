using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    private bool CloseEnoughToTotem;
    [SerializeField] private bool SecretTotem;
    private PlayerController player; 
    
    FlippedChestScript flippedChestScript;
    
    private void Start()
    {
        CloseEnoughToTotem = false;
        int destroyOrNot = Random.Range(0,2);
        if (destroyOrNot == 0) { Destroy(gameObject); }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CloseEnoughToTotem && SecretTotem == false) 
        { 
            player.SummonChanceWhale();
            TreasureChestScript chestScript = GameObject.FindGameObjectWithTag("TotemChest").GetComponent<TreasureChestScript>();
            chestScript.OpenTreasureChest();
            Destroy(gameObject);
        }
        
        if (Input.GetKeyDown(KeyCode.E) && CloseEnoughToTotem && SecretTotem == true) 
        { 
            player.StartCoroutine(player.SummonDevilishWhale());
            FlippedChestScript flippedChestScript = GameObject.FindGameObjectWithTag("TotemChest").GetComponent<FlippedChestScript>();
            flippedChestScript.OpenFlippedChest();
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
