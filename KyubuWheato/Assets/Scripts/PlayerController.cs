using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float tileMoveSpeed = 5f;
    public Transform movePoint;
    public LayerMask whatStopsMovement;

    public int maxHealth = 15;
    public int playerHealth;

    public float strength = 1f;

    public HealthBar healthBar;

    public DiceThrow diceThrowScript;
    public Text DiceCounterNumber;

    public Text victoryScreen;
    public GameObject crosshair;
    public GameObject diceThrower;

    private void Start(){
        movePoint.parent = null;

        playerHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        victoryScreen.enabled = false;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, tileMoveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f && diceThrowScript.inCooldown == false) 
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {   
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    diceThrowScript.inCooldown = true;
                }
            }  else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, whatStopsMovement))
                    {
                        movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        diceThrowScript.inCooldown = true;
                    }
                }
        }
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
