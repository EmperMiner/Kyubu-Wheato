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

    private void Start()
    {
        isMoving = true;
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
        else if (isB) { enemyIndex = 1; FindObjectOfType<AudioManager>().PlaySound("BHello"); }
        else if (isC) { enemyIndex = 2; }
        else if (isD) { enemyIndex = 3; }
        else if (isE) { enemyIndex = 4; StartCoroutine(ShootCornRays()); }
        else if (isF) { enemyIndex = 5; StartCoroutine(ChangeSpeedATK()); }
        else { enemyIndex = 0; }

        CheckForValidSpawn = true;
        ShotWall = false;
        
        StartCoroutine(ValidSpawn());
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
            if (player.InHealMode == true) { player.UpdateHealth(15); }

            if (isC) { Instantiate(worseWheatDrop, transform.position, Quaternion.identity); }
            else { Instantiate(wheatDrop, transform.position, Quaternion.identity); }

            Destroy(gameObject);
        }
    }

    private void FixedUpdate() 
    {
       if (badSpawn == false && isMoving) { agent.SetDestination(playerTransform.position); } 
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
            if (isC) { StartCoroutine(CSplit()); player.HitByC(); FindObjectOfType<AudioManager>().PlaySound("DefenseDown"); }
            if (isF) { FindObjectOfType<AudioManager>().PlaySound("FSay");  }
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

