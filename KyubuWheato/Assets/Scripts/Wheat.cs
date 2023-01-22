using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheat : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private int WheatAmount;
    [SerializeField] private bool isSuperior;
    private int bonus;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            if (isSuperior) { bonus = Random.Range(0,3); }
            else { bonus = 0; }

            player.UpdateWheat(WheatAmount + bonus);
            Destroy(gameObject);
        }
    }

}
