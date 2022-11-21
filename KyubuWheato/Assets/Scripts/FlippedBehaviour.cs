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
    [SerializeField] private GameObject worseWheatDrop;
    [SerializeField] private Transform pfDamagePopup;
    [SerializeField] private TextMeshPro pfDamagePopupText;
    [SerializeField] private Transform pfDamagePopupCrit;
    [SerializeField] private TextMeshPro pfDamagePopupTextCrit;

    [SerializeField] private bool isA;
    [SerializeField] private bool isB;
    [SerializeField] private bool isC;
    [SerializeField] private bool isD;
    [SerializeField] private bool isE;
    [SerializeField] private bool isF;
    [SerializeField] private GameObject cornRay;
    private float stoppingDistance = 5.1f;
    private float BStoppingDistance = 3.1f;
    private float AStoppingDistance = 10f;
    private bool isMoving;

    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private GameObject crowPrefab;
    [SerializeField] private GameObject WallPrefab;
    private bool CrowCooldown = false;
    private float crowCooldownTimer;
    private bool ShotWall;

    private int enemyIndex;
    private bool CheckForValidSpawn;

    private Transform betterEnemySpawner;
    private UltimateBarCharge ultimateBar;
    private bool stopped;
    private DiceThrow diceThrowScript;
    [SerializeField] private GameObject Supernova;

    private void Start()
    {
        stopped = false;
        isMoving = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        diceThrowScript = GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceThrow>();
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
        else if (isB) { enemyIndex = 1; FindObjectOfType<AudioManager>().PlaySound("BHello"); }
        else if (isC) { enemyIndex = 2; }
        else if (isD) { enemyIndex = 3; }
        else if (isE) { enemyIndex = 4; StartCoroutine(ShootCornRays()); }
        else if (isF) { enemyIndex = 5; StartCoroutine(ChangeSpeedATK()); }
        else { enemyIndex = 0; }

        CheckForValidSpawn = true;
        ShotWall = false;
        
        StartCoroutine(ValidSpawn());
        StartCoroutine(RandomSpeedOffset());
    }

    private void Update()
    {
        if (isA) { if (Vector2.Distance(transform.position, playerTransform.position) <= AStoppingDistance && ShotWall == false) { StartCoroutine(AHand()); } }
        if (isB) { if (Vector2.Distance(transform.position, playerTransform.position) <= BStoppingDistance && ShotWall == false) { StartCoroutine(BWall()); } }
        if (isD) { if (Vector2.Distance(transform.position, playerTransform.position) <= stoppingDistance && CrowCooldown == false) { ShootCrow(); }  }

        if (CrowCooldown)
        {   
            crowCooldownTimer += Time.deltaTime; 
            if (crowCooldownTimer >= 0.15f)
            {
                CrowCooldown = false;
                crowCooldownTimer = 0;
            }
        }

        if(mouseHealth <= 0)
        {   
            if (PlayerPrefs.GetInt("IngameRamen") == 1 && Random.Range(0f, 100f) < 3f + PlayerPrefs.GetInt("ChargedAttacks")*0.1f) 
            { 
                Instantiate(Supernova, transform.position, Quaternion.identity); 
                FindObjectOfType<AudioManager>().PlaySound("Supernova");
            }
            if (player.InHealMode == true) { player.UpdateHealth(15); }

            if (isC) { for (int i = 0; i < Random.Range(0,5); i++) { Instantiate(worseWheatDrop, transform.position, Quaternion.identity); } }
            else { for (int i = 0; i < Random.Range(1,4); i++) { Instantiate(wheatDrop, transform.position, Quaternion.identity); } }

            Destroy(gameObject);
        }
    }

    private void FixedUpdate() 
    {
        if (badSpawn == false && isMoving && stopped == false) { agent.SetDestination(playerTransform.position); } 
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
        if (CheckForValidSpawn == true && UndesirableSpawn == true)
        {
            badSpawn = true;
            Instantiate(enemiesPrefabs[enemyIndex], new Vector3(betterEnemySpawner.position.x + Random.Range(-10f, 10f), betterEnemySpawner.position.y + Random.Range(-10f, 10f), 0), Quaternion.identity);
            Destroy(gameObject);
        }  

        if (collider.gameObject.tag == "TimeCrescent")
        {
            stopped = true;
            diceThrowScript.CrescentCrack(400);
            FindObjectOfType<AudioManager>().PlaySound("CrescentBreak" + UnityEngine.Random.Range(1,7));
        }

        if (alreadyDamaged == false)
        {
            if (collider.gameObject.tag == "6sidedDice1" && isD == false && isF == false) { mouseTakeDamage(1); ChargeUlt(6); }
            if (collider.gameObject.tag == "6sidedDice2" && isD == false && isF == false) { mouseTakeDamage(2); ChargeUlt(5); } 
            if (collider.gameObject.tag == "6sidedDice3" && isD == false && isF == false) { mouseTakeDamage(3); ChargeUlt(4); }
            if (collider.gameObject.tag == "6sidedDice4" && isD == false && isF == false) { mouseTakeDamage(4); ChargeUlt(3); }
            if (collider.gameObject.tag == "6sidedDice5" && isD == false && isF == false) { mouseTakeDamage(5); ChargeUlt(2); }
            if (collider.gameObject.tag == "6sidedDice6" && isD == false && isF == false) { mouseTakeDamage(6); ChargeUlt(1); }
            if (collider.gameObject.tag == "BroomAttack") { mouseTakeDamage(10); ChargeUlt(8); }        

            if (collider.gameObject.tag == "FakeDice1") { mouseTakeDamage(1); }
            if (collider.gameObject.tag == "FakeDice2") { mouseTakeDamage(2); } 
            if (collider.gameObject.tag == "FakeDice3") { mouseTakeDamage(3); }
            if (collider.gameObject.tag == "FakeDice4") { mouseTakeDamage(4); }
            if (collider.gameObject.tag == "FakeDice5") { mouseTakeDamage(5); }
            if (collider.gameObject.tag == "FakeDice6") { mouseTakeDamage(6); }      

            if (collider.gameObject.tag == "6sidedDice1" && isD  == true) { PlayGlitchedSound(); }
            if (collider.gameObject.tag == "6sidedDice2" && isD  == true) { PlayGlitchedSound(); }
            if (collider.gameObject.tag == "6sidedDice3" && isD  == true) { PlayGlitchedSound(); }
            if (collider.gameObject.tag == "6sidedDice4" && isD  == true) { PlayGlitchedSound(); }
            if (collider.gameObject.tag == "6sidedDice5" && isD  == true) { PlayGlitchedSound(); }
            if (collider.gameObject.tag == "6sidedDice6" && isD  == true) { PlayGlitchedSound(); }

            if (collider.gameObject.tag == "6sidedDice1" && isF  == true) { Destroy(collider.gameObject); FindObjectOfType<AudioManager>().PlaySound("Burp"); }
            if (collider.gameObject.tag == "6sidedDice2" && isF  == true) { Destroy(collider.gameObject); FindObjectOfType<AudioManager>().PlaySound("Burp");  }
            if (collider.gameObject.tag == "6sidedDice3" && isF  == true) { Destroy(collider.gameObject); FindObjectOfType<AudioManager>().PlaySound("Burp");  }
            if (collider.gameObject.tag == "6sidedDice4" && isF  == true) { Destroy(collider.gameObject); FindObjectOfType<AudioManager>().PlaySound("Burp");  }
            if (collider.gameObject.tag == "6sidedDice5" && isF  == true) { Destroy(collider.gameObject); FindObjectOfType<AudioManager>().PlaySound("Burp");  }
            if (collider.gameObject.tag == "6sidedDice6" && isF  == true) { Destroy(collider.gameObject); FindObjectOfType<AudioManager>().PlaySound("Burp");  }
            if (collider.gameObject.tag == "Star") { mouseTakeDamage(UnityEngine.Random.Range(1,4) + Mathf.RoundToInt(((player.maxHealth - player.playerHealth)/player.maxHealth)*5)); }  
            if (collider.gameObject.tag == "100sidedDice") { mouseTakeDamage(UnityEngine.Random.Range(50,100)); }   
        }
        if (collider.gameObject.tag == "Player") { mouseCanAttack = mouseAttackSpeed; }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag != "Player") return;

        if (mouseCanAttack >= mouseAttackSpeed)
        {
            if (player.Invincible == false) { FindObjectOfType<AudioManager>().PlaySound("PlayerHurt"); }
            else { FindObjectOfType<AudioManager>().PlaySound("Iframe"); }
            player.UpdateHealth(-mouseStrength + Mathf.RoundToInt((mouseStrength * player.defense)/10));
            mouseCanAttack = 0f;
            if (isC) { StartCoroutine(CSplit()); player.HitByC(); FindObjectOfType<AudioManager>().PlaySound("DefenseDown"); }
            if (isF) { FindObjectOfType<AudioManager>().PlaySound("FSay");  }
        }    

        mouseCanAttack += Time.deltaTime; 
        if (mouseCanAttack <= 0.3f) {player.spriteRenderer.material.color = new Color32(255, 150, 150, 255);}
        else { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "TimeCrescent") 
        { 
            stopped = false;
        }
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
        if (collider.gameObject.tag == "FakeDice8") { alreadyDamaged = false; }  
        if (collider.gameObject.tag == "FakeDice10") { alreadyDamaged = false; }  
        if (collider.gameObject.tag == "FakeDice12") { alreadyDamaged = false; }  
        if (collider.gameObject.tag == "FakeDice20") { alreadyDamaged = false; }  
        if (collider.gameObject.tag == "Star") { alreadyDamaged = false; }  
        if (collider.gameObject.tag == "100sidedDice") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  

        if (collider.gameObject.tag == "Player") { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
    }

    private IEnumerator RandomSpeedOffset()
    {
        yield return new WaitForSeconds(Random.Range(1f, 18f));
        agent.speed += Random.Range(-0.3f, 2f);
        if (agent.speed < 1f) { agent.speed = 1; }
        yield return null;
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

    private IEnumerator BWall()
    {
        ShotWall = true;
        isMoving = false;
        agent.isStopped = true;
        agent.ResetPath();
        yield return new WaitForSeconds(4f);
        FindObjectOfType<AudioManager>().PlaySound("4thWall");
        Instantiate(WallPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2.5f);
        isMoving = true;
        agent.isStopped = false;
        ShotWall = false;
        yield return null;
    }

    private IEnumerator AHand()
    {
        ShotWall = true;
        isMoving = false;
        agent.isStopped = true;
        agent.ResetPath();
        StartCoroutine(PlayHandSounds());
        Vector2 enemyTargetVector = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y);
        float HandAngleOffset = Vector2.Angle(Vector2.right, enemyTargetVector);
        if (playerTransform.position.y < transform.position.y) { Instantiate(WallPrefab, transform.position, Quaternion.Euler(0f, 0f, 360f - HandAngleOffset)); }
        else { Instantiate(WallPrefab, transform.position, Quaternion.Euler(0f, 0f, HandAngleOffset)); }
        yield return new WaitForSeconds(10f);
        isMoving = true;
        agent.isStopped = false;
        ShotWall = false;
        yield return null;
    }
    
    private IEnumerator PlayHandSounds()
    {
        for (int i = 0; i < 7; i++)
        {
            FindObjectOfType<AudioManager>().PlaySound("Bone");
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    private IEnumerator CSplit()
    {
        isMoving = false;
        agent.isStopped = true;
        agent.ResetPath();
        yield return new WaitForSeconds(2f);
        Instantiate(enemiesPrefabs[enemyIndex], new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z), Quaternion.identity);
        Instantiate(enemiesPrefabs[enemyIndex], new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z), Quaternion.identity);
        Instantiate(enemiesPrefabs[enemyIndex], new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Quaternion.identity);
        Instantiate(enemiesPrefabs[enemyIndex], new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z), Quaternion.identity);
        FindObjectOfType<AudioManager>().PlaySound("CSplit");
        isMoving = true;
        agent.isStopped = false;
        yield return null;
    }

    private void PlayGlitchedSound()
    {
        int haha = UnityEngine.Random.Range(0,2);
        if (haha == 0) { FindObjectOfType<AudioManager>().PlaySound("HahaIgnore"); }
        else if (haha == 1) { FindObjectOfType<AudioManager>().PlaySound("HahaIgnore2"); }
    }

    private IEnumerator ChangeSpeedATK()
    {
        agent.speed = UnityEngine.Random.Range(3f, 8f);
        agent.acceleration = UnityEngine.Random.Range(2f, 6f);
        mouseStrength = UnityEngine.Random.Range(10, 50);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.4f, 3f));
        StartCoroutine(ChangeSpeedATK());
        yield return null;
    }
}

