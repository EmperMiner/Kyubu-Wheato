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
    [SerializeField] private int mouseStrength = 1;
    [SerializeField] private float mouseAttackSpeed = 1f;
    private float mouseCanAttack;

    private bool alreadyDamaged;

    private PlayerController player;
    private Transform playerTransform;
    private HealthBar healthBar;
    private Rigidbody2D mouseRB;
    private Vector2 movement;
    public GameObject wheatDrop;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        mouseHealth = maxMouseHealth;
        alreadyDamaged = false;

        mouseRB = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(mouseHealth <= 0)
        {   
            int RNGWheat = Random.Range(0, 2);
            Debug.Log(RNGWheat);
            if (RNGWheat == 1) { Instantiate(wheatDrop, transform.position, Quaternion.Euler(0, 0, 10)); }
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() 
    {
        agent.SetDestination(playerTransform.position);
    }

    private void mouseTakeDamage(int i)
    {
        i = (int) (i * player.strength);
        mouseHealth -= i;
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
        if (collider.gameObject.tag == "Player") { mouseCanAttack = 1f; }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag != "Player") return;

        if (mouseCanAttack >= mouseAttackSpeed)
        {
            player.UpdateHealth(-mouseStrength);
            mouseCanAttack = 0f;
        }

        mouseCanAttack += Time.deltaTime; 
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "6sidedDice1") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "6sidedDice2") { alreadyDamaged = false; } 
        if (collider.gameObject.tag == "6sidedDice3") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "6sidedDice4") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "6sidedDice5") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "6sidedDice6") { alreadyDamaged = false; }     
    }
}
