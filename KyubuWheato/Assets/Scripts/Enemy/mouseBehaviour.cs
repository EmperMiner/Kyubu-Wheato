using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class mouseBehaviour : MonoBehaviour
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
    private ExitHoeContainer ExitHoeWinCondition;
    private Transform playerTransform;
    private HealthBar healthBar;

    private Rigidbody2D mouseRB;
    private Vector2 movement;

    private bool badSpawn;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject wheatDrop;
    [SerializeField] private Transform pfDamagePopup;
    [SerializeField] private TextMeshPro pfDamagePopupText;
    [SerializeField] private Transform pfDamagePopupCrit;
    [SerializeField] private TextMeshPro pfDamagePopupTextCrit;
    [SerializeField] private GameObject[] dicetypes;

    [SerializeField] private bool isMiniBoss;
    [SerializeField] private bool isMouse;
    [SerializeField] private bool isCowman;
    [SerializeField] private bool isHenor;
    [SerializeField] private bool isScawy;
    [SerializeField] private bool isSchwein;
    [SerializeField] private bool isSpooder;
    [SerializeField] private GameObject cornRay;
    private float stoppingDistance = 9f;
    [SerializeField] private bool isGhost;
    [SerializeField] private bool isBlackCat;

    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private GameObject crowPrefab;
    private DiceThrow diceThrowScript;
    private bool CrowCooldown = false;
    private float crowCooldownTimer;

    private int enemyIndex;
    private bool CheckForValidSpawn;

    private Transform betterEnemySpawner;
    private UltimateBarCharge ultimateBar;
    private bool stopped;
    [SerializeField] private GameObject Supernova;
    private float ghostSpeed;
    private bool firing;
    private bool spriteFiring;
    [SerializeField] private SpriteRenderer blackCatSpriteRenderer;

    private void Start()
    {
        stopped = false;
        firing = false;
        spriteFiring = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        diceThrowScript = GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceThrow>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ultimateBar = GameObject.FindGameObjectWithTag("Ultimate Bar").GetComponent<UltimateBarCharge>();
        ExitHoeWinCondition = GameObject.FindGameObjectWithTag("ExitHoeContainer").GetComponent<ExitHoeContainer>();
        betterEnemySpawner = GameObject.FindGameObjectWithTag("betterEnemySpawner").transform;

        if (isGhost == false)
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        UpdateStats();
        mouseHealth = maxMouseHealth;
        alreadyDamaged = false;

        mouseRB = this.GetComponent<Rigidbody2D>();

        if (isMouse) { enemyIndex = 0; }
        else if (isCowman) { enemyIndex = 1; }
        else if (isHenor) { enemyIndex = 2; }
        else if (isScawy) { enemyIndex = 3; }
        else if (isBlackCat) { enemyIndex = 4; }
        else if (isSchwein) { StartCoroutine(ShootCornRays()); }
        else { enemyIndex = 0; }

        CheckForValidSpawn = true;
        
        StartCoroutine(ValidSpawn());
        StartCoroutine(RandomSpeedOffset());
    }

    private void Update()
    {
        if (isScawy == false && isSpooder == false)
        {
            if (playerTransform.position.x > transform.position.x) { animator.SetFloat("moveX", 1); }
            else { animator.SetFloat("moveX", -1);  }
        }
        else
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= stoppingDistance && CrowCooldown == false && isMiniBoss == false && isSpooder == false) { ShootCrow(); }
            else if (Vector2.Distance(transform.position, playerTransform.position) <= stoppingDistance && CrowCooldown == false && isMiniBoss == true && isSpooder == false) { StartCoroutine(ShootMultipleCrow()); }
            else {}
        }

        if (CrowCooldown)
        {   
            crowCooldownTimer += Time.deltaTime; 
            if (crowCooldownTimer >= 2f)
            {
                CrowCooldown = false;
                crowCooldownTimer = 0;
            }
        }

        

        

        if (isBlackCat && firing == false && Vector2.Distance(transform.position, playerTransform.position) <= 5f) { StartCoroutine(ShootPotion()); }

        if (isBlackCat && spriteFiring == true) 
        { 
            if (playerTransform.position.x > transform.position.x) { blackCatSpriteRenderer.flipX = true; }
            else { blackCatSpriteRenderer.flipX = false; }
        }
        else if (isBlackCat && spriteFiring == false) { blackCatSpriteRenderer.flipX = false; }
       

        if(mouseHealth <= 0)
        {   
            if (PlayerPrefs.GetInt("IngameRamen") == 1 && Random.Range(0f, 100f) < 1f + PlayerPrefs.GetInt("ChargedAttacks")*0.07f) 
            { 
                Instantiate(Supernova, transform.position, Quaternion.identity); 
                FindObjectOfType<AudioManager>().PlaySound("Supernova");
            }
            if (player.InHealMode == true) { player.UpdateHealth(Mathf.RoundToInt(player.maxHealth*0.01f + 0.7f)); FindObjectOfType<AudioManager>().PlaySound("Lifesteal"); }

            float RNGWheat = Random.Range(0f, 10f);
            if (RNGWheat <= player.wheatDroprate/10) { Instantiate(wheatDrop, transform.position, Quaternion.identity); }
            if (isMiniBoss) { for (int i = 0; i < Random.Range(2,7); i++) { Instantiate(wheatDrop, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), transform.position.y + UnityEngine.Random.Range(-0.5f, 0.5f), transform.position.z), Quaternion.identity); } }

            int RNGDice = Random.Range(0, player.diceDroprate);
            if (isMiniBoss == true) { RNGDice = Random.Range(0, 3); }
            if (RNGDice == 0) { Instantiate(dicetypes[Random.Range(0,dicetypes.Length)], transform.position, Quaternion.identity); }

            if (isGhost == false) { ExitHoeWinCondition.EnemiesKilled++; }
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() 
    {
        if (badSpawn == false && stopped == false && isGhost == false) { agent.SetDestination(playerTransform.position); } 
        else if (stopped == false && isGhost) { transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, ghostSpeed * Time.deltaTime);}
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }
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
        pfDamagePopupTextCrit.text = damageAmount.ToString();
        if (damageAmount/player.strength <= 6) { Instantiate(pfDamagePopup, transform.position, Quaternion.identity); }
        else { Instantiate(pfDamagePopupCrit, transform.position, Quaternion.identity); }
    }

    private void ChargeUlt(int ChargeAmount)
    {
        if (ultimateBar.havePizza == true) { ultimateBar.IncreaseUltimateCharge(ChargeAmount); }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        bool UndesirableSpawn = collider.gameObject.tag == "MapCollider" || collider.gameObject.tag == "chest" || collider.gameObject.tag == "Player";
        if (CheckForValidSpawn == true && UndesirableSpawn == true && isGhost == false && isSpooder == false)
        {
            badSpawn = true;

            float RandomXOffset;
            float RandomYOffset;   

            int rand1 = Random.Range(0,2);
            if (rand1 == 0) { RandomXOffset = Random.Range(-10f,-3f); }
            else { RandomXOffset = Random.Range(3f,10f); }

            int rand2 = Random.Range(0,2);
            if (rand2 == 0) { RandomYOffset = Random.Range(-10f,-3f); }
            else { RandomYOffset = Random.Range(3f,10f); }

            Instantiate(enemiesPrefabs[enemyIndex], new Vector3(betterEnemySpawner.position.x + RandomXOffset, betterEnemySpawner.position.y + RandomYOffset, 0), Quaternion.identity);

            Destroy(gameObject);
        }  

        if (collider.gameObject.tag == "TimeCrescent")
        {
            stopped = true;
            diceThrowScript.CrescentCrack(1);
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
            if (collider.gameObject.tag == "ChargedDice2") { mouseTakeDamage(2); ChargeUlt(2); }   
            if (collider.gameObject.tag == "ChargedDice4") { mouseTakeDamage(4); ChargeUlt(4); }   
            if (collider.gameObject.tag == "ChargedDice6") { mouseTakeDamage(6); ChargeUlt(6); }   
            if (collider.gameObject.tag == "FakeDice8") { mouseTakeDamage(8); ChargeUlt(8); }     
            if (collider.gameObject.tag == "FakeDice10") { mouseTakeDamage(10); ChargeUlt(10); }     
            if (collider.gameObject.tag == "FakeDice12") { mouseTakeDamage(12); ChargeUlt(12); }     
            if (collider.gameObject.tag == "FakeDice20") { mouseTakeDamage(20); }   

            if (collider.gameObject.tag == "Star") { mouseTakeDamage(UnityEngine.Random.Range(1,4) + Mathf.RoundToInt(((player.maxHealth - player.playerHealth)/player.maxHealth)*5)); }   
            if (collider.gameObject.tag == "100sidedDice") { mouseTakeDamage(UnityEngine.Random.Range(100,200)); }   
        }
        if (collider.gameObject.tag == "Player") { mouseCanAttack = mouseAttackSpeed; }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag == "TimeCrescent") { mouseSpriteRenderer.material.color = new Color32(255, 187, 0, 255); }
        if (other.gameObject.tag != "Player") return;

        if (mouseCanAttack >= mouseAttackSpeed && player.playerAttacked == false)
        {
            if (player.Invincible == false) { FindObjectOfType<AudioManager>().PlaySound("PlayerHurt"); }
            else { FindObjectOfType<AudioManager>().PlaySound("Iframe"); }
            int playerDamageAmount = mouseStrength + Mathf.RoundToInt((mouseStrength * player.defense)/10);
            if (playerDamageAmount < 1) { playerDamageAmount = 1; }
            player.UpdateHealth(-playerDamageAmount);
            mouseCanAttack = 0f;
            player.playerAttacked = true;
        }    

        mouseCanAttack += Time.deltaTime; 
        if (mouseCanAttack <= 0.3f) {player.spriteRenderer.material.color = new Color32(255, 150, 150, 255);}
        else { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "TimeCrescent") 
        { 
            if (spriteFiring == false) { stopped = false; }
            mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); 
        }
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
        if (collider.gameObject.tag == "BroomAttack") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
        if (collider.gameObject.tag == "ChargedDice2") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "ChargedDice4") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "ChargedDice6") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice8") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice10") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice12") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
        if (collider.gameObject.tag == "FakeDice20") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
        if (collider.gameObject.tag == "Star") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
        if (collider.gameObject.tag == "100sidedDice") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  

        if (collider.gameObject.tag == "Player") { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
    }

    private IEnumerator ValidSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        CheckForValidSpawn = false;
        yield return null;
    }

    private IEnumerator RandomSpeedOffset()
    {
        yield return new WaitForSeconds(Random.Range(3f, 20f));
        agent.speed += Random.Range(-0.5f, 1.2f);
        if (agent.speed < 1f) { agent.speed = 1; }
        yield return null;
    }

    private void UpdateStats()
    {
        int level = SceneManager.GetActiveScene().buildIndex - 4;
        if (isMouse && isMiniBoss == false) 
        { 
            mouseStrength = Mathf.FloorToInt(level*1f + 1.5f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 1.88f) + 4f));
            agent.speed = level*0.2f + 1.5f;
        }
        if (isCowman && isMiniBoss == false) 
        { 
            mouseStrength = Mathf.FloorToInt(level*1f + 4f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 2f) + 2f));
            agent.speed = level*0.1f + 1.3f;
            agent.acceleration = level*0.1f + 6f;
        }
        if (isHenor && isMiniBoss == false) 
        { 
            mouseStrength = Mathf.FloorToInt(level*2f + 0.5f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 1.73f) + 1f));
            agent.speed = level*0.5f + 1.5f;
        }
        if (isScawy && isMiniBoss == false) 
        { 
            mouseStrength = Mathf.FloorToInt(level*0.4f + 2f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 1.75f) + 2f));
            agent.speed = level*0.1f + 2f;
            agent.acceleration = level*0.3f + 4f;
        }
        if (isMouse && isMiniBoss == true) 
        { 
            mouseStrength = Mathf.FloorToInt(level*3f + 1f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 2.7f) + 50f));
            agent.speed = level*0.2f + 1f;
        }
        if (isCowman && isMiniBoss == true) 
        { 
            mouseStrength = Mathf.FloorToInt(level*3.6f + 3f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 2.9f) + 90f));
            agent.speed = level*0.15f + 1f;
            agent.acceleration = level*0.2f + 5f;
        }
        if (isHenor && isMiniBoss == true) 
        { 
            mouseStrength = Mathf.FloorToInt(level*1.8f + 1f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 2.53f) + 10f));
            agent.speed = level*1f + 3f;
        }
        if (isScawy && isMiniBoss == true) 
        { 
            mouseStrength = Mathf.FloorToInt(level*2.8f + 2f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 2.6f) + 40f));
            agent.speed = level*0.1f + 2f;
            agent.acceleration = level*0.3f + 4f;
        }
        if (isSpooder && isMiniBoss == false) 
        { 
            mouseStrength = Mathf.FloorToInt(level*2f + 1f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 1.8f) + 15f));
            agent.speed = level*0.5f + 3f;
        }
        if (isSchwein) 
        { 
            mouseStrength = Mathf.FloorToInt(level*1.5f + 10f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 2.3f) + 350f));
            agent.speed = level*0.1f + 0.8f;
            agent.acceleration = level*0.1f + 3.5f;
        }
        if (isGhost)
        {
            mouseStrength = Mathf.FloorToInt(level*7f + 12f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 2.5f) + 24f));
            ghostSpeed = level*0.2f + 2f;
        }
        if (isBlackCat) 
        { 
            mouseStrength = Mathf.FloorToInt(level*1f + 5f);
            maxMouseHealth = Mathf.FloorToInt((Mathf.Pow(level, 2.4f) + 50f));
            agent.speed = level*0.4f + 1.2f;
            agent.acceleration = level*0.1f + 1f;
        }
    }

    private IEnumerator ShootPotion()
    {
        firing = true;
        spriteFiring = true;
        stopped = true;
        animator.SetBool("Firing", true);
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < Random.Range(1,4); i++) { Instantiate(cornRay, transform.position, Quaternion.identity); }
        yield return new WaitForSeconds(5.3f);
        animator.SetBool("Firing", false);
        if (mouseSpriteRenderer.material.color != new Color32(255, 187, 0, 255)) { stopped = false; } 
        spriteFiring = false;
        yield return new WaitForSeconds(2f);
        firing = false;
        yield return null;
    }

    private void ShootCrow()
    {
        Instantiate(crowPrefab, transform.position, Quaternion.identity);
        CrowCooldown = true;
    }

    private IEnumerator ShootMultipleCrow()
    {
        Instantiate(crowPrefab, transform.position, Quaternion.identity);
        CrowCooldown = true;
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.3f);
            Instantiate(crowPrefab, transform.position, Quaternion.identity);
        }   
        yield return null;
    }

    private IEnumerator ShootCornRays()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 6f));
        float AngleToShoot = Random.Range(0,2)*90;
        Instantiate(cornRay, transform.position, Quaternion.Euler(0f, 0f, AngleToShoot), this.transform);
        Instantiate(cornRay, transform.position, Quaternion.Euler(0f, 0f, AngleToShoot + 180f), this.transform);
        StartCoroutine(ShootCornRays());
        yield return null;
    }
}
