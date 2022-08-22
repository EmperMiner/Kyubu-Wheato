using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DiceThrow : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public Text DiceCounterNumber;
    public GameObject[] dicetypes;
    public Transform diceTransform;
    public int diceNumber;
    public bool inCooldown = false;
    private float cooldownTimer;
    public float playerCooldownTime;

    public CooldownBar cooldownBar;
    
    void Start()
    {
        LoadData();
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
                cooldownBar.SetCooldown(cooldownTimer*10);
                cooldownTimer += Time.deltaTime; 
                if (cooldownTimer > playerCooldownTime/10)
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

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        diceNumber = loadedPlayerData.diceNumber;
        playerCooldownTime = loadedPlayerData.playerCooldownTime;
    }   
    

    private class PlayerData
    {
        public int diceNumber;
        public float playerCooldownTime;
    }
}
