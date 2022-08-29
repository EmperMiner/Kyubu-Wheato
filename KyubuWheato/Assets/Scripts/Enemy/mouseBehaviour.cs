using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class mouseBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private int maxMouseHealth = 50;
    private int mouseHealth;
    [SerializeField] private int mouseStrength = 10;
    [SerializeField] private float mouseAttackSpeed = 1f;
    private float mouseCanAttack;

    private bool alreadyDamaged;
    [SerializeField] private Renderer mouseSpriteRenderer;

    private PlayerController player;
    private Transform playerTransform;
    private HealthBar healthBar;
    private Rigidbody2D mouseRB;
    private Vector2 movement;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject wheatDrop;
    [SerializeField] private Transform pfDamagePopup;
    [SerializeField] private TextMeshPro pfDamagePopupText;

    private UltimateBarCharge ultimateBar;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ultimateBar = GameObject.FindGameObjectWithTag("Ultimate Bar").GetComponent<UltimateBarCharge>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        mouseHealth = maxMouseHealth;
        alreadyDamaged = false;

        mouseRB = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (playerTransform.position.x > transform.position.x) { animator.SetFloat("moveX", 1); }
        else { animator.SetFloat("moveX", -1);  }

        if(mouseHealth <= 0)
        {   
            float RNGWheat = Random.Range(0, 10);
            if (RNGWheat <= player.wheatDroprate/10) { Instantiate(wheatDrop, transform.position, Quaternion.identity); }
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
        CreateDamagePopup(i);
        mouseHealth -= i;
        mouseSpriteRenderer.material.color = new Color32(255, 150, 150, 255); 
        alreadyDamaged = true;
    }

    private void CreateDamagePopup(int damageAmount)
    {
        pfDamagePopupText.text = damageAmount.ToString();
        Instantiate(pfDamagePopup, transform.position, Quaternion.identity);
    }

    private void ChargeUlt(int ChargeAmount)
    {
        if (ultimateBar.havePizza == true) { ultimateBar.IncreaseUltimateCharge(ChargeAmount); }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (alreadyDamaged == false)
        {
            if (collider.gameObject.tag == "6sidedDice1") { mouseTakeDamage(1); ChargeUlt(6); }
            if (collider.gameObject.tag == "6sidedDice2") { mouseTakeDamage(2); ChargeUlt(5); } 
            if (collider.gameObject.tag == "6sidedDice3") { mouseTakeDamage(3); ChargeUlt(4); }
            if (collider.gameObject.tag == "6sidedDice4") { mouseTakeDamage(4); ChargeUlt(3); }
            if (collider.gameObject.tag == "6sidedDice5") { mouseTakeDamage(5); ChargeUlt(2); }
            if (collider.gameObject.tag == "6sidedDice6") { mouseTakeDamage(6); ChargeUlt(1); }        

            if (collider.gameObject.tag == "FakeDice1") { mouseTakeDamage(1); }
            if (collider.gameObject.tag == "FakeDice2") { mouseTakeDamage(2); } 
            if (collider.gameObject.tag == "FakeDice3") { mouseTakeDamage(3); }
            if (collider.gameObject.tag == "FakeDice4") { mouseTakeDamage(4); }
            if (collider.gameObject.tag == "FakeDice5") { mouseTakeDamage(5); }
            if (collider.gameObject.tag == "FakeDice6") { mouseTakeDamage(6); }      
        }
        if (collider.gameObject.tag == "Player") { mouseCanAttack = mouseAttackSpeed; }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag != "Player") return;

        if (mouseCanAttack >= mouseAttackSpeed)
        {
            player.UpdateHealth(-mouseStrength + Mathf.RoundToInt((mouseStrength * player.defense)/10));
            mouseCanAttack = 0f;
        }    

        mouseCanAttack += Time.deltaTime; 
        if (mouseCanAttack <= 0.3f) {player.spriteRenderer.material.color = new Color32(255, 150, 150, 255);}
        else { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "6sidedDice1") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "6sidedDice2") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); } 
        if (collider.gameObject.tag == "6sidedDice3") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "6sidedDice4") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "6sidedDice5") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "6sidedDice6") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  

        if (collider.gameObject.tag == "FakeDice1") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice2") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); } 
        if (collider.gameObject.tag == "FakeDice3") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice4") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice5") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice6") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  

        if (collider.gameObject.tag == "Player") { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
    }
}
