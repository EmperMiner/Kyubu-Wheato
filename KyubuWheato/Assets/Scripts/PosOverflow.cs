using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosOverflow : MonoBehaviour
{
    private Transform posOverflowDestination;

    public bool isOrange;
    public float distance = 0.2f;

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
            other.transform.position = new Vector2 (posOverflowDestination.position.x + 0.1f, posOverflowDestination.position.y + 0.1f);
        }
    }
}
