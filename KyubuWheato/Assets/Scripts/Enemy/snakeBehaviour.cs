using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class snakeBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    public int maxSnakeHealth = 5;
    public int snakeHealth;
    public float snakeSpeed = 1f;

    private bool alreadyDamaged;

    private PlayerController player;
    private Transform playerTransform;
    private Rigidbody2D snakeRB;
    private Vector2 movement;

    public Text SnakeHealthCounterNumber;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        snakeHealth = maxSnakeHealth;
        SnakeHealthCounterNumber.text = snakeHealth.ToString();
        alreadyDamaged = false;

        snakeRB = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        

        if(snakeHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() 
    {
        agent.SetDestination(playerTransform.position);
    }

    private void snakeTakeDamage(int i)
    {
        i = (int)Mathf.Round(i * player.strength);
        snakeHealth -= i;
        SnakeHealthCounterNumber.text = snakeHealth.ToString();
        alreadyDamaged = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (alreadyDamaged == false)
        {
            if (collider.gameObject.tag == "6sidedDice1") { snakeTakeDamage(1); }
            if (collider.gameObject.tag == "6sidedDice2") { snakeTakeDamage(2); } 
            if (collider.gameObject.tag == "6sidedDice3") { snakeTakeDamage(3); }
            if (collider.gameObject.tag == "6sidedDice4") { snakeTakeDamage(4); }
            if (collider.gameObject.tag == "6sidedDice5") { snakeTakeDamage(5); }
            if (collider.gameObject.tag == "6sidedDice6") { snakeTakeDamage(6); }        
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        alreadyDamaged = false;
    }
}
