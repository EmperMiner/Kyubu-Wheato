using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DiceThrow : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    [SerializeField] private Text DiceCounterNumber;
    [SerializeField] private GameObject[] fakeDiceTypes;
    [SerializeField] private GameObject[] dicetypes;
    [SerializeField] private GameObject[] MultishotDiceTypes;
    [SerializeField] private GameObject[] FakeMultishotLeftDiceTypes;
    [SerializeField] private GameObject[] FakeMultishotRightDiceTypes;
    [SerializeField] private Transform diceTransform;
    public int diceNumber;
    private bool inCooldown = false;
    private float cooldownTimer;
    private float playerCooldownTime;
    private bool haveFlan;
    private bool haveCremeBrulee;

    [SerializeField] private CooldownBar cooldownBar;
    [SerializeField] private UltimateBarCharge ultimateBar;
    
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

            if (Input.GetMouseButton(0) && diceNumber > 0 && inCooldown==false && ultimateBar.ultimateInProgress == false)
            {
                inCooldown = true;
                if (haveFlan == true && haveCremeBrulee == false) { ShootTwoDice(); }
                else if (haveCremeBrulee) { ShootThreeDice(); }
                else { ShootOneDice(); }
                DiceCounterNumber.text = diceNumber.ToString();
            }
            if (Input.GetKeyDown(KeyCode.Q) && ultimateBar.currentUltimateCharge == ultimateBar.maxUltimateCharge && ultimateBar.havePizza == true)
            {
                ultimateBar.IncreaseUltimateCharge(-ultimateBar.maxUltimateCharge);
                StartCoroutine(ActivateUltimate());
            }
        }  
    }

    private void ShootOneDice()
    {
        diceNumber--;
        Instantiate(dicetypes[Random.Range(0,dicetypes.Length)], diceTransform.position, Random.rotation);
    }
    private void ShootTwoDice()
    {
        if (diceNumber >= 2)
        {
            diceNumber -= 2;
            for (int i = 0; i < 2; i++) { Instantiate(MultishotDiceTypes[Random.Range(0,MultishotDiceTypes.Length)], diceTransform.position, Random.rotation); }
        }
        else { ShootOneDice(); }
    }
    private void ShootThreeDice()
    {
        if (diceNumber >= 3)
        {
            diceNumber -= 3;
            for (int i = 0; i < 3; i++) { Instantiate(MultishotDiceTypes[Random.Range(0,MultishotDiceTypes.Length)], diceTransform.position, Random.rotation); }
        }
        else if (diceNumber == 2) { ShootTwoDice(); }
        else { ShootOneDice(); }
    }

    IEnumerator ActivateUltimate()
    {
        ultimateBar.ultimateInProgress = true;
        for (float i = 0; i < 5f; i += 0.2f)
        {
            if (haveFlan == true && haveCremeBrulee == false) 
            { 
                Instantiate(FakeMultishotLeftDiceTypes[Random.Range(0,FakeMultishotLeftDiceTypes.Length)], diceTransform.position, Random.rotation);
                Instantiate(FakeMultishotRightDiceTypes[Random.Range(0,FakeMultishotRightDiceTypes.Length)], diceTransform.position, Random.rotation);
            }
            else if (haveCremeBrulee) 
            { 
                Instantiate(fakeDiceTypes[Random.Range(0,fakeDiceTypes.Length)], diceTransform.position, Random.rotation);
                Instantiate(FakeMultishotLeftDiceTypes[Random.Range(0,FakeMultishotLeftDiceTypes.Length)], diceTransform.position, Random.rotation);
                Instantiate(FakeMultishotRightDiceTypes[Random.Range(0,FakeMultishotRightDiceTypes.Length)], diceTransform.position, Random.rotation);
            }
            else { Instantiate(fakeDiceTypes[Random.Range(0,fakeDiceTypes.Length)], diceTransform.position, Random.rotation); }
            yield return new WaitForSeconds(0.2f);
        }
        ultimateBar.ultimateInProgress = false;
        yield return null;
    }

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        diceNumber = loadedPlayerData.diceNumber;
        playerCooldownTime = loadedPlayerData.playerCooldownTime;
        haveFlan = loadedPlayerData.haveFlan;
        haveCremeBrulee = loadedPlayerData.haveCremeBrulee;
    }   
    
    private class PlayerData
    {
        public int diceNumber;
        public float playerCooldownTime;
        public bool haveFlan;
        public bool haveCremeBrulee;
    }
}
