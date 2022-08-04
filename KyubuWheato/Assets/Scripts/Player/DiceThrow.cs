using UnityEngine;
using UnityEngine.UI;

public class DiceThrow : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public Text DiceCounterNumber;
    public GameObject[] dicetypes;
    public Transform diceTransform;
    public int diceNumber = 3;
    public bool inCooldown = false;
    private float cooldownTimer;
    public float playerCooldownTime = 1f;

    public CooldownBar cooldownBar;
    
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
        DiceCounterNumber.text = diceNumber.ToString();
    }

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rotation = mousePos - transform.position;

            float rotZ = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (inCooldown)
            {   
                cooldownBar.SetCooldown(cooldownTimer);
                cooldownTimer += Time.deltaTime; 
                if (cooldownTimer > playerCooldownTime)
                {
                    inCooldown = false;
                    cooldownTimer = 0;
                    cooldownBar.CooldownBarInvisible();
                }
            }

            if (Input.GetMouseButton(0) && diceNumber > 0 && inCooldown==false)
            {
                diceNumber--;
                DiceCounterNumber.text = diceNumber.ToString();
                inCooldown = true;
                Instantiate(dicetypes[Random.Range(0,dicetypes.Length)],diceTransform.position, Random.rotation);
            }
        }     
    }
}
