using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class bossBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    public int maxBossHealth = 5;
    public int bossHealth;
    public float bossSpeed = 1f;

    private bool alreadyDamaged;

    private PlayerController player;
    private DiceThrow diceThrowScript;
    private Text DiceCounterNumber;
    private Transform playerTransform;
    private Rigidbody2D bossRB;
    private Vector2 movement;

    public Text BossHealthCounterNumber;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        diceThrowScript = GameObject.FindGameObjectWithTag("DiceLogic").GetComponent<DiceThrow>();
        DiceCounterNumber = GameObject.FindGameObjectWithTag("DiceNumberCounter").GetComponent<Text>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        bossHealth = maxBossHealth;
        BossHealthCounterNumber.text = bossHealth.ToString();
        alreadyDamaged = false;

        bossRB = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        

        if(bossHealth <= 0)
        {
            Destroy(gameObject);
            diceThrowScript.diceNumber++;
            DiceCounterNumber.text = diceThrowScript.diceNumber.ToString();
        }
    }

    private void FixedUpdate() 
    {
        agent.SetDestination(playerTransform.position);
    }

    private void bossTakeDamage(int i)
    {
        i = (int)Mathf.Round(i * player.strength);
        bossHealth -= i;
        BossHealthCounterNumber.text = bossHealth.ToString();
        alreadyDamaged = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (alreadyDamaged == false)
        {
            if (collider.gameObject.tag == "6sidedDice1") { bossTakeDamage(1); }
            if (collider.gameObject.tag == "6sidedDice2") { bossTakeDamage(2); } 
            if (collider.gameObject.tag == "6sidedDice3") { bossTakeDamage(3); }
            if (collider.gameObject.tag == "6sidedDice4") { bossTakeDamage(4); }
            if (collider.gameObject.tag == "6sidedDice5") { bossTakeDamage(5); }
            if (collider.gameObject.tag == "6sidedDice6") { bossTakeDamage(6); }        
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        alreadyDamaged = false;
    }
}
