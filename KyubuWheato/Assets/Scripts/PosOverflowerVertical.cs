using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosOverflowerVertical : MonoBehaviour
{
    private Transform posOverflowDestination;
    private PlayerController player;

    [SerializeField] private bool isOrange;
    private float distance = 0.2f;

    private Rigidbody2D enteredRigidbody;
    private float enterVelocity, exitVelocity;

    private string nameOfDestination;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (isOrange)
        {
            posOverflowDestination = GameObject.FindGameObjectWithTag("Blue PosFlower 2").GetComponent<Transform>();
        } else
        {
            posOverflowDestination = GameObject.FindGameObjectWithTag("Orange PosFlower 2").GetComponent<Transform>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Vector2.Distance(transform.position, other.transform.position) > distance)
        {
            if (other.gameObject.tag == "Player")
            {
                if (isOrange)
                {
                    other.transform.position = new Vector2 (posOverflowDestination.position.x + 0.5f, posOverflowDestination.position.y - 1f);
                }
                else
                {
                    other.transform.position = new Vector2 (posOverflowDestination.position.x, posOverflowDestination.position.y + 1f);
                }
            }
        }
    }
}
