using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosOverflowerVertical : MonoBehaviour
{
    private Transform posOverflowDestination;

    public bool isOrange;
    private float distance = 0.2f;

    private Rigidbody2D enteredRigidbody;
    private float enterVelocity, exitVelocity;

    private string nameOfDestination;

    void Start()
    {
        if (isOrange)
        {
            posOverflowDestination = GameObject.FindGameObjectWithTag("Blue PosFlower").GetComponent<Transform>();
        } else
        {
            posOverflowDestination = GameObject.FindGameObjectWithTag("Orange PosFlower").GetComponent<Transform>();
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
                    other.transform.position = new Vector2 (posOverflowDestination.position.x + 0.5f, posOverflowDestination.position.y + 1f);
                }
            }
            else if (other.gameObject.tag != "Player")
            {
                if (isOrange)
                {
                    other.transform.position = new Vector2 (posOverflowDestination.position.x + 0.1f, posOverflowDestination.position.y - 1.5f);
                    enteredRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
                    enteredRigidbody.velocity = new Vector2(0, -15);
                }
                else
                {
                    other.transform.position = new Vector2 (posOverflowDestination.position.x + 0.1f, posOverflowDestination.position.y + 1.5f);
                    enteredRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
                    enteredRigidbody.velocity = new Vector2(0, 15);
                }
                
            }
        }
       
    }
}
