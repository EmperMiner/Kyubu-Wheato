using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EZCameraShake;
using System;

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
    [SerializeField] private Transform pfDamagePopupCrit;
    [SerializeField] private TextMeshPro pfDamagePopupTextCrit;

    [SerializeField] private GameObject GoldenWheat;
    private GameObject GoldenWheatOnMap;
    private GameObject Arrow;
    private GameObject bossHealthBorder;
    private GameObject bossHealthFill;

    private bool alreadyDamaged;
    [SerializeField] private int mouseHealth;
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

        if (player.maxHealth*50 < 25000) { mouseHealth = 25000; }
        else { mouseHealth = player.maxHealth*50; }

        if (Mathf.FloorToInt(player.strength*10) < 100) { bossDamage = 100; }
        else { bossDamage = Mathf.FloorToInt(player.strength*10); }
        
        agent.speed = 3;
        RollAttackDelay = 5f;
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
            PlayerPrefs.SetInt("BossDefeated", 1);
            Instantiate(GoldenWheat, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().PlaySound("FarmerShake");
            bossHealthBar.SetActive(false);
            FindObjectOfType<AudioManager>().StopSound("HEAVYFARMER");
            int i = PlayerPrefs.GetInt("WinCounter");
            PlayerPrefs.SetInt("WinCounter", (i + 1));
            UnlockEntree();
            Destroy(gameObject);  
        }
    }

    private void UnlockEntree()
    {
        bool AllEntree = Convert.ToBoolean(PlayerPrefs.GetInt("Ramen")) && Convert.ToBoolean(PlayerPrefs.GetInt("Salmon")) && Convert.ToBoolean(PlayerPrefs.GetInt("Steak")) && Convert.ToBoolean(PlayerPrefs.GetInt("Cheese"));
        if (!AllEntree)
        {
            int Entree = UnityEngine.Random.Range(0,4);
            if (Entree == 0 && PlayerPrefs.GetInt("Ramen") == 0) { PlayerPrefs.SetInt("Ramen", 1); PlayerPrefs.SetInt("NewEntreeScreen", 1); }
            else if (Entree == 1 && PlayerPrefs.GetInt("Salmon") == 0) { PlayerPrefs.SetInt("Salmon", 1); PlayerPrefs.SetInt("NewEntreeScreen", 1); }
            else if (Entree == 2 && PlayerPrefs.GetInt("Steak") == 0) { PlayerPrefs.SetInt("Steak", 1); PlayerPrefs.SetInt("NewEntreeScreen", 1); }
            else if (Entree == 3 && PlayerPrefs.GetInt("Cheese") == 0) { PlayerPrefs.SetInt("Cheese", 1); PlayerPrefs.SetInt("NewEntreeScreen", 1); }
            else { UnlockEntree(); }
        }
    }

    public IEnumerator RollAttackFunc()
    {
        if (RollAttack)
        {   
            yield return new WaitForSeconds(RollAttackDelay);
            AttackRolled = UnityEngine.Random.Range(1,11);
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
                   other.gameObject.tag == "FakeDice4" || other.gameObject.tag == "FakeDice5" || other.gameObject.tag == "FakeDice6" || other.gameObject.tag == "BroomAttack" ||
                   other.gameObject.tag == "ChargedDice2" || other.gameObject.tag == "ChargedDice4" || other.gameObject.tag == "ChargedDice6" || 
                   other.gameObject.tag == "FakeDice8" || other.gameObject.tag == "FakeDice10" || other.gameObject.tag == "FakeDice12" ||
                   other.gameObject.tag == "FakeDice20" || other.gameObject.tag == "100sidedDice" && inChargingState;
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
            if (other.gameObject.tag == "ChargedDice2") { mouseTakeDamage(2); ChargeUlt(24); }   
            if (other.gameObject.tag == "ChargedDice4") { mouseTakeDamage(4); ChargeUlt(20);  }   
            if (other.gameObject.tag == "ChargedDice6") { mouseTakeDamage(6); ChargeUlt(16); }   
            if (other.gameObject.tag == "FakeDice8") { mouseTakeDamage(8); ChargeUlt(12); }     
            if (other.gameObject.tag == "FakeDice10") { mouseTakeDamage(10); ChargeUlt(8); }     
            if (other.gameObject.tag == "FakeDice12") { mouseTakeDamage(12); ChargeUlt(4); }      
            if (other.gameObject.tag == "FakeDice20") { mouseTakeDamage(20); }     

            if (other.gameObject.tag == "Star") { mouseTakeDamage(UnityEngine.Random.Range(1,4) + Mathf.RoundToInt(((player.maxHealth - player.playerHealth)/player.maxHealth)*5)); }  
            if (other.gameObject.tag == "100sidedDice") { mouseTakeDamage(UnityEngine.Random.Range(100,200)); }   
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag != "Player") return;

        if (bossCanAttack >= 0.7f && player.playerAttacked == false)
        {
            if (player.Invincible == false) { FindObjectOfType<AudioManager>().PlaySound("PlayerHurt"); }
            else { FindObjectOfType<AudioManager>().PlaySound("Iframe"); }

            int playerDamageAmount = bossDamage;
            if (playerDamageAmount < Mathf.FloorToInt(player.maxHealth/100f)) { playerDamageAmount = Mathf.FloorToInt(player.maxHealth/100f); }
            player.UpdateHealth(-playerDamageAmount);

            bossCanAttack = 0f;
            player.playerAttacked = true;
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
        if (collider.gameObject.tag == "ChargedDice2") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "ChargedDice4") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "ChargedDice6") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice8") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice10") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }
        if (collider.gameObject.tag == "FakeDice12") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
        if (collider.gameObject.tag == "FakeDice20") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
        
        if (collider.gameObject.tag == "Star") { alreadyDamaged = false; }  
        if (collider.gameObject.tag == "100sidedDice") { alreadyDamaged = false; mouseSpriteRenderer.material.color = new Color32(255, 255, 255, 255); }  

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
        inChargingState = false;
        StartCoroutine(RemoveHeatRiserBuff());
        animator.ResetTrigger("Hurt");
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
        pfDamagePopupTextCrit.text = damageAmount.ToString();
        if (damageAmount/player.strength <= 6) { Instantiate(pfDamagePopup, transform.position, Quaternion.identity); }
        else { Instantiate(pfDamagePopupCrit, transform.position, Quaternion.identity); }
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
        bossHealthBarScript.HealthBarFlash(1);
        yield return new WaitForSeconds(0.3f);
        bossHealthBarScript.HealthBarFlash(0);
        yield return null;
    }

    public void ChargedShake() { CameraShaker.Instance.ShakeOnce(3f, 7f, .1f, 2f); }
    public void PlayHEAVYFARMER() { FindObjectOfType<AudioManager>().PlayJingle("HEAVYFARMER"); }
    public void PlayHeatRiser() { FindObjectOfType<AudioManager>().PlaySound("HeatRiser"); }
    public void PlayScytheSwing() { FindObjectOfType<AudioManager>().PlaySound("ScytheSwing"); }
    public void PlayEyeGlare() { FindObjectOfType<AudioManager>().PlaySound("EyeGlare"); }
}
