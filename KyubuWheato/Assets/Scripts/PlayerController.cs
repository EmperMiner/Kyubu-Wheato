using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float MoveSpeed = 3f;
    public Rigidbody2D playerRB ;
    Vector2 movement;
    public Animator animator;

    public int maxHealth = 15;
    public int playerHealth;

    public float strength = 1f;

    public HealthBar healthBar;

    public DiceThrow diceThrowScript;
    public Text DiceCounterNumber;

    public Text victoryScreen;
    public GameObject crosshair;
    public GameObject diceThrower;

    private void Start()
    {
        playerHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        victoryScreen.enabled = false;
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if ( movement.x!=0 ) { animator.SetFloat("Horizontal", movement.x); }
        animator.SetFloat("Speed", movement.sqrMagnitude);
        Debug.Log("Horizontal is " + movement.x);
        Debug.Log("Speed is" + movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
            playerRB.MovePosition(playerRB.position + movement * MoveSpeed * Time.fixedDeltaTime);
    }

    void TakeDamage(int damage)
    {
        playerHealth -= damage;
        healthBar.SetHealth(playerHealth);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "6sidedDice1") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice2") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice3") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice4") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice5") { IncreaseDiceNumber(); } 
        if (collider.gameObject.tag == "6sidedDice6") { IncreaseDiceNumber(); } 

        if (collider.gameObject.tag == "enemyMouse") { TakeDamage(1);}
        if (collider.gameObject.tag == "enemySnake") { TakeDamage(1);}
        if (collider.gameObject.tag == "enemyBoss") { TakeDamage(2);}

        if (collider.gameObject.tag == "Wheat") 
        { 
            victoryScreen.enabled = true;
            Time.timeScale = 0f;
            crosshair.SetActive(false);
            diceThrower.SetActive(false);
        }
    }

    private void IncreaseDiceNumber()
    {
        diceThrowScript.diceNumber++;
        DiceCounterNumber.text = diceThrowScript.diceNumber.ToString();
    }
}
