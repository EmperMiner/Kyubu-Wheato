using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class mouseBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    public int maxMouseHealth = 5;
    public int mouseHealth;
    public float mouseSpeed = 1f;

    private bool alreadyDamaged;

    public PlayerController player;
    public Transform playerTransform;
    private Rigidbody2D mouseRB;
    private Vector2 movement;

    public Text MouseHealthCounterNumber;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        mouseHealth = maxMouseHealth;
        MouseHealthCounterNumber.text = mouseHealth.ToString();
        alreadyDamaged = false;

        mouseRB = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        

        if(mouseHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() 
    {
        agent.SetDestination(playerTransform.position);
    }

    private void mouseTakeDamage(int i)
    {
        i = (int)Mathf.Round(i * player.strength);
        mouseHealth -= i;
        MouseHealthCounterNumber.text = mouseHealth.ToString();
        alreadyDamaged = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (alreadyDamaged == false)
        {
            if (collider.gameObject.tag == "6sidedDice1") { mouseTakeDamage(1); }
            if (collider.gameObject.tag == "6sidedDice2") { mouseTakeDamage(2); } 
            if (collider.gameObject.tag == "6sidedDice3") { mouseTakeDamage(3); }
            if (collider.gameObject.tag == "6sidedDice4") { mouseTakeDamage(4); }
            if (collider.gameObject.tag == "6sidedDice5") { mouseTakeDamage(5); }
            if (collider.gameObject.tag == "6sidedDice6") { mouseTakeDamage(6); }        
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        alreadyDamaged = false;
    }
}
