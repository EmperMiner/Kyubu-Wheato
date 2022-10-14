using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceHitbox : MonoBehaviour
{
    [SerializeField] private GameObject dice;
    [SerializeField] private diceMagnetize diceScript;

    private void Start()
    {
        diceScript = dice.GetComponent<diceMagnetize>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, dice.transform.position, .03f);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (diceScript.pickupable == false) return;
            Destroy(dice);
            Destroy(gameObject);
        } 
    }
}
