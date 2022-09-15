using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceHitbox : MonoBehaviour
{
    [SerializeField] private GameObject dice;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, dice.transform.position, .03f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Destroy(dice);
            Destroy(gameObject);
        } 
    }
}
