using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheat : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private int WheatAmount;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            player.UpdateWheat(WheatAmount + Random.Range(0,3));
            Destroy(gameObject);
        }
    }

}
