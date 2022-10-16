using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FlippedBehaviour : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;

    [SerializeField] private int maxMouseHealth;
    private int mouseHealth;
    [SerializeField] private int mouseStrength;
    [SerializeField] private float mouseAttackSpeed = 1f;
    private float mouseCanAttack;

    private bool alreadyDamaged;
    [SerializeField] private Renderer mouseSpriteRenderer;

    private PlayerController player;
    private Transform playerTransform;
    private HealthBar healthBar;

    private Rigidbody2D mouseRB;
    private Vector2 movement;

    private bool badSpawn;

    [SerializeField] private GameObject wheatDrop;
    [SerializeField] private Transform pfDamagePopup;
    [SerializeField] private TextMeshPro pfDamagePopupText;

    [SerializeField] private bool isA;
    [SerializeField] private bool isB;
    [SerializeField] private bool isC;
    [SerializeField] private bool isD;
    [SerializeField] private bool isE;
    [SerializeField] private GameObject cornRay;
    private float stoppingDistance = 4f;

    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private GameObject crowPrefab;
    private bool CrowCooldown = false;
    private float crowCooldownTimer;

    private int enemyIndex;
    private bool CheckForValidSpawn;

    private Transform betterEnemySpawner;

    private UltimateBarCharge ultimateBar;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ultimateBar = GameObject.FindGameObjectWithTag("Ultimate Bar").GetComponent<UltimateBarCharge>();
        betterEnemySpawner = GameObject.FindGameObjectWithTag("betterEnemySpawner").transform;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        mouseHealth = maxMouseHealth;
        alreadyDamaged = false;

        mouseRB = this.GetComponent<Rigidbody2D>();

        if (isA) { enemyIndex = 0; }
        else if (isB) { enemyIndex = 1; }
        else if (isC) { enemyIndex = 2; }
        else if (isD) { enemyIndex = 3; }
        else if (isE) { StartCoroutine(ShootCornRays()); }
        else { enemyIndex = 0; }

        CheckForValidSpawn = true;
        
        StartCoroutine(ValidSpawn());
    }

    private void Update()
    {
        if (isD)
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= stoppingDistance && CrowCooldown == false) { ShootCrow(); }
        }

        if (CrowCooldown)
        {   
            crowCooldownTimer += Time.deltaTime; 
            if (crowCooldownTimer >= 0.3f)
            {
                CrowCooldown = false;
                crowCooldownTimer = 0;
            }
        }

        if(mouseHealth <= 0)
        {   
            if (player.InHealMode == true) { player.UpdateHealth(5); }

            for (int i = 0; i < 20; i++) { Instantiate(wheatDrop, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), transform.position.y + UnityEngine.Random.Range(-0.5f, 0.5f), transform.position.z), Quaternion.identity); }

            Destroy(gameObject);
        }
    }

    private void FixedUpdate() 
    {
       if (badSpawn == false) { agent.SetDestination(playerTransform.position); } 
    }

    private void mouseTakeDamage(int i)
    {
        i = (int) (i * player.strength);
        CreateDamagePopup(i);
        mouseHealth -= i;
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
        bool UndesirableSpawn = collider.gameObject.tag == "MapCollider" || collider.gameObject.tag == "chest" || collider.gameObject.tag == "Player";
        if (CheckForValidSpawn == true && UndesirableSpawn == true)
        {
            badSpawn = true;
            Instantiate(enemiesPrefabs[enemyIndex], new Vector3(betterEnemySpawner.position.x + Random.Range(-10f, 10f), betterEnemySpawner.position.y + Random.Range(-10f, 10f), 0), Quaternion.identity);
            Destroy(gameObject);
        }  

        if (alreadyDamaged == false)
        {
            if (collider.gameObject.tag == "6sidedDice1") { mouseTakeDamage(1); ChargeUlt(6); }
            if (collider.gameObject.tag == "6sidedDice2") { mouseTakeDamage(2); ChargeUlt(5); } 
            if (collider.gameObject.tag == "6sidedDice3") { mouseTakeDamage(3); ChargeUlt(4); }
            if (collider.gameObject.tag == "6sidedDice4") { mouseTakeDamage(4); ChargeUlt(3); }
            if (collider.gameObject.tag == "6sidedDice5") { mouseTakeDamage(5); ChargeUlt(2); }
            if (collider.gameObject.tag == "6sidedDice6") { mouseTakeDamage(6); ChargeUlt(1); }
            if (collider.gameObject.tag == "BroomAttack") { mouseTakeDamage(10); ChargeUlt(8); }        

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
            FindObjectOfType<AudioManager>().PlaySound("PlayerHurt");
            player.UpdateHealth(-mouseStrength + Mathf.RoundToInt((mouseStrength * player.defense)/10));
            mouseCanAttack = 0f;
        }    

        mouseCanAttack += Time.deltaTime; 
        if (mouseCanAttack <= 0.3f) {player.spriteRenderer.material.color = new Color32(255, 150, 150, 255);}
        else { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "6sidedDice1") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "6sidedDice2") { alreadyDamaged = false; } 
        if (collider.gameObject.tag == "6sidedDice3") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "6sidedDice4") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "6sidedDice5") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "6sidedDice6") { alreadyDamaged = false; }  

        if (collider.gameObject.tag == "FakeDice1") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "FakeDice2") { alreadyDamaged = false; } 
        if (collider.gameObject.tag == "FakeDice3") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "FakeDice4") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "FakeDice5") { alreadyDamaged = false; }
        if (collider.gameObject.tag == "FakeDice6") { alreadyDamaged = false; }  
        if (collider.gameObject.tag == "BroomAttack") { alreadyDamaged = false; }  

        if (collider.gameObject.tag == "Player") { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
    }

    private IEnumerator ValidSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        CheckForValidSpawn = false;
        yield return null;
    }

    private void ShootCrow()
    {
        Instantiate(crowPrefab, transform.position, Quaternion.identity);
        CrowCooldown = true;
    }

    private IEnumerator ShootCornRays()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 6f));
        float AngleToShoot = 0;
        for (int i = 0; i < 16; i++)
        {
            Instantiate(cornRay, transform.position, Quaternion.Euler(0f, 0f, AngleToShoot), this.transform);
            yield return new WaitForSeconds(0.1f);
            AngleToShoot += 22.5f;
        }
        
        StartCoroutine(ShootCornRays());
        yield return null;
    }
}

