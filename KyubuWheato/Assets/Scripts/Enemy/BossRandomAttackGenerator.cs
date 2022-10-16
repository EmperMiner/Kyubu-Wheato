using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EZCameraShake;

public class BossRandomAttackGenerator : MonoBehaviour
{
    private GameObject bossHealthBar;
    private HealthBar bossHealthBarScript;
    public UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private Animator animator;
    private FarmerWalk farmerWalkScript;
    [SerializeField] private GameObject tumbleWeed;
    private PlayerController player;
    private UltimateBarCharge ultimateBar;

    [SerializeField] private SpriteRenderer mouseSpriteRenderer;
    [SerializeField] private GameObject pfDamagePopup;
    [SerializeField] private TextMeshPro pfDamagePopupText;

    [SerializeField] private GameObject GoldenWheat;
    private GameObject GoldenWheatOnMap;
    private GameObject Arrow;
    private GameObject bossHealthBorder;
    private GameObject bossHealthFill;

    private bool alreadyDamaged;
    [SerializeField] private int mouseHealth = 12000;
    public float RollAttackDelay;

    private float bossCanAttack;
    public int bossDamage;

    public bool RollAttack;
    public int AttackRolled;
    public bool hit;
    public bool inChargingState;

    private void Start()
    {
        Arrow = GameObject.FindGameObjectWithTag("Arrow");
        Arrow.SetActive(false);
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ultimateBar = GameObject.FindGameObjectWithTag("Ultimate Bar").GetComponent<UltimateBarCharge>();

        bossDamage = 20;
        agent.speed = 2;
        RollAttackDelay = 6f;
        alreadyDamaged = false;
        inChargingState = false;

        bossHealthBar = GameObject.FindGameObjectWithTag("BossHealthBar");
        bossHealthBorder = GameObject.FindGameObjectWithTag("BossHealthBorder");
        bossHealthFill = GameObject.FindGameObjectWithTag("BossHealthFill");
        bossHealthBorder.GetComponent<Image>().color = new Color32(0, 0, 0, 200);
        bossHealthFill.GetComponent<Image>().color = new Color32(135, 0, 0, 200);
        
        bossHealthBarScript = bossHealthBar.GetComponent<HealthBar>();
        bossHealthBarScript.SetMaxHealth(mouseHealth);
        bossHealthBar.SetActive(false);
    }

    private void Update()
    {
        if(mouseHealth <= 0)
        { 
            Instantiate(GoldenWheat, transform.position, Quaternion.identity);
            bossHealthBar.SetActive(false);
            Destroy(gameObject);  
        }
    }

    public IEnumerator RollAttackFunc()
    {
        if (RollAttack)
        {   
            yield return new WaitForSeconds(RollAttackDelay);
            AttackRolled = Random.Range(1,11);
            StartCoroutine(RollAttackFunc());
        }
        else { yield return null; }
    }

    public IEnumerator TumbleweedSpawn()
    {
        FindObjectOfType<AudioManager>().PlaySound("TumbleweedFly");
        float TumbleweedOffset = 0f;
        for (int i = 0; i < 8; i++)
        {
            Instantiate(tumbleWeed, transform.position, Quaternion.Euler(0f, 0f, TumbleweedOffset));
            yield return new WaitForSeconds(0.5f);
            TumbleweedOffset += 45f;
        }
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player") { bossCanAttack = 0.5f; }
             hit = other.gameObject.tag == "6sidedDice1" || other.gameObject.tag == "6sidedDice2" || other.gameObject.tag == "6sidedDice3" || 
                   other.gameObject.tag == "6sidedDice4" || other.gameObject.tag == "6sidedDice5" || other.gameObject.tag == "6sidedDice6" || 
                   other.gameObject.tag == "FakeDice1" || other.gameObject.tag == "FakeDice2" || other.gameObject.tag == "FakeDice3" || 
                   other.gameObject.tag == "FakeDice4" || other.gameObject.tag == "FakeDice5" || other.gameObject.tag == "FakeDice6" || other.gameObject.tag == "BroomAttack" && inChargingState;
        if (alreadyDamaged == false)
        {
            if (other.gameObject.tag == "6sidedDice1") { mouseTakeDamage(1); ChargeUlt(6); }
            if (other.gameObject.tag == "6sidedDice2") { mouseTakeDamage(2); ChargeUlt(5); } 
            if (other.gameObject.tag == "6sidedDice3") { mouseTakeDamage(3); ChargeUlt(4); }
            if (other.gameObject.tag == "6sidedDice4") { mouseTakeDamage(4); ChargeUlt(3); }
            if (other.gameObject.tag == "6sidedDice5") { mouseTakeDamage(5); ChargeUlt(2); }
            if (other.gameObject.tag == "6sidedDice6") { mouseTakeDamage(6); ChargeUlt(1); }
            if (other.gameObject.tag == "BroomAttack") { mouseTakeDamage(10); ChargeUlt(8); }        

            if (other.gameObject.tag == "FakeDice1") { mouseTakeDamage(1); }
            if (other.gameObject.tag == "FakeDice2") { mouseTakeDamage(2); } 
            if (other.gameObject.tag == "FakeDice3") { mouseTakeDamage(3); }
            if (other.gameObject.tag == "FakeDice4") { mouseTakeDamage(4); }
            if (other.gameObject.tag == "FakeDice5") { mouseTakeDamage(5); }
            if (other.gameObject.tag == "FakeDice6") { mouseTakeDamage(6); }      
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag != "Player") return;

        if (bossCanAttack >= 0.7f)
        {
            FindObjectOfType<AudioManager>().PlaySound("PlayerHurt");
            player.UpdateHealth(-bossDamage);
            bossCanAttack = 0f;
        }    
        bossCanAttack += Time.deltaTime; 

        if (bossCanAttack <= 0.3f) {player.spriteRenderer.material.color = new Color32(255, 150, 150, 255);}
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
        if (collider.gameObject.tag == "BroomAttack") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  

        if (collider.gameObject.tag == "Player") { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
    }

    public IEnumerator RemoveHeatRiserBuff()
    {
        yield return new WaitForSeconds(20f);
        bossDamage = 20;
        agent.speed = 2;
        RollAttackDelay = 6f;
        yield return null;
    }

    public void ReturnToWalk()
    {
        animator.SetTrigger("Charged");
    }

    private void mouseTakeDamage(int i)
    {
        i = (int) (i * player.strength);
        CreateDamagePopup(i);
        mouseHealth -= i;
        mouseSpriteRenderer.material.color = new Color32(255, 150, 150, 255); 
        alreadyDamaged = true;
        bossHealthBarScript.SetHealth(mouseHealth);
        StartCoroutine(FlashingHealthBar());
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

    public void ActivateHealthBar()
    {
        bossHealthBar.SetActive(true);
    }

    private IEnumerator FlashingHealthBar()
    {
        bossHealthBarScript.HealthBarFlash(true);
        yield return new WaitForSeconds(0.3f);
        bossHealthBarScript.HealthBarFlash(false);
        yield return null;
    }

    public void ChargedShake() { CameraShaker.Instance.ShakeOnce(3f, 7f, .1f, 2f); }
    public void PlayHEAVYFARMER() { FindObjectOfType<AudioManager>().PlayJingle("HEAVYFARMER"); }
    public void PlayHeatRiser() { FindObjectOfType<AudioManager>().PlaySound("HeatRiser"); }
    public void PlayScytheSwing() { FindObjectOfType<AudioManager>().PlaySound("ScytheSwing"); }
    public void PlayEyeGlare() { FindObjectOfType<AudioManager>().PlaySound("EyeGlare"); }
}
