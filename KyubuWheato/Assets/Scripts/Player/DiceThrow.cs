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
    [SerializeField] private CooldownBar cooldownBar;
    [SerializeField] private UltimateBarCharge ultimateBar;
    [SerializeField] private Image[] DicePreviewerImage;
    [SerializeField] private Sprite[] DiceSprites;
    [SerializeField] private GameObject[] KyubuTiles;
    public int diceNumber;
    private bool inCooldown = false;
    private float cooldownTimer;
    private float playerCooldownTime;
    private int dicePreviewerLevel;
    private bool haveFlan;
    private bool haveCremeBrulee;
    private bool haveBanhmi;

    
    private int[] DiceValues = new int[6];
    private int[] PreviousDiceValues = new int[6];
    
    void Start()
    {
        LoadData();
        GenerateRandomDiceArray();
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
            if (Input.GetKeyDown(KeyCode.Alpha1) && dicePreviewerLevel >= 2 && haveBanhmi == true) { DiceHotkey(1); }
            if (Input.GetKeyDown(KeyCode.Alpha2) && dicePreviewerLevel >= 3 && haveBanhmi == true) { DiceHotkey(2); }
            if (Input.GetKeyDown(KeyCode.Alpha3) && dicePreviewerLevel >= 4 && haveBanhmi == true) { DiceHotkey(3); }
            if (Input.GetKeyDown(KeyCode.Alpha4) && dicePreviewerLevel >= 5 && haveBanhmi == true) { DiceHotkey(4); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { StartCoroutine(KyubuKombo(6)); }
        }  
    }

    private void GenerateRandomDiceArray()
    {
        DiceValues[0] = Random.Range(0,dicetypes.Length);
        DiceValues[1] = Random.Range(0,dicetypes.Length);
        DiceValues[2] = Random.Range(0,dicetypes.Length);
        DiceValues[3] = Random.Range(0,dicetypes.Length);
        DiceValues[4] = Random.Range(0,dicetypes.Length);
        DiceValues[5] = Random.Range(0,dicetypes.Length);
        PreviousDiceValues[0] = 10;
        PreviousDiceValues[1] = 10;
        PreviousDiceValues[2] = 10;
        PreviousDiceValues[3] = 10;
        PreviousDiceValues[4] = 10;
        PreviousDiceValues[5] = 10;
        UpdateDicePreviewerUI();
    }

    private void CycleThroughDiceValueArray()
    {
        PreviousDiceValues[0] = PreviousDiceValues[1];
        PreviousDiceValues[1] = PreviousDiceValues[2];
        PreviousDiceValues[2] = PreviousDiceValues[3];
        PreviousDiceValues[3] = PreviousDiceValues[4];
        PreviousDiceValues[4] = PreviousDiceValues[5];
        PreviousDiceValues[5] =  DiceValues[0];
        DiceValues[0] = DiceValues[1];
        DiceValues[1] = DiceValues[2];
        DiceValues[2] = DiceValues[3];
        DiceValues[3] = DiceValues[4];
        DiceValues[4] = Random.Range(0,dicetypes.Length);
        UpdateDicePreviewerUI();
        KyubuKomboCheck();
    }

    private void UpdateDicePreviewerUI()
    {
        DicePreviewerImage[0].sprite = DiceSprites[DiceValues[0]];
        DicePreviewerImage[1].sprite = DiceSprites[DiceValues[1]];
        DicePreviewerImage[2].sprite = DiceSprites[DiceValues[2]];
        DicePreviewerImage[3].sprite = DiceSprites[DiceValues[3]];
        DicePreviewerImage[4].sprite = DiceSprites[DiceValues[4]];
        DicePreviewerLevelCheck();
    }

    private void DicePreviewerLevelCheck()
    {
        if (dicePreviewerLevel >= 1) { DicePreviewerImage[0].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[0].color = new Color32(255, 255, 255, 0); }

        if (dicePreviewerLevel >= 2) { DicePreviewerImage[1].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[1].color = new Color32(255, 255, 255, 0); }

        if (dicePreviewerLevel >= 3) { DicePreviewerImage[2].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[2].color = new Color32(255, 255, 255, 0); }

        if (dicePreviewerLevel >= 4) { DicePreviewerImage[3].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[3].color = new Color32(255, 255, 255, 0); }

        if (dicePreviewerLevel >= 5) { DicePreviewerImage[4].color = new Color32(255, 255, 255, 180); }
        else { DicePreviewerImage[4].color = new Color32(255, 255, 255, 0); }
    }

    private void DiceHotkey(int ChangingWithDice)
    {
        int mock = DiceValues[0];
        DiceValues[0] = DiceValues[ChangingWithDice];
        DiceValues[ChangingWithDice] = mock;
        UpdateDicePreviewerUI();
    }

    private void KyubuKomboCheck()
    {
        if (PreviousDiceValues[5] == 0 && PreviousDiceValues[4] == 0) { StartCoroutine(KyubuKombo(1)); }
        if (PreviousDiceValues[5] == 5 && PreviousDiceValues[4] == 5) { StartCoroutine(KyubuKombo(6)); }
    }

    IEnumerator KyubuKombo(int KyubuTileValue)
    {
        if (KyubuTileValue == 1) { Instantiate(KyubuTiles[0], new Vector3(transform.position.x, transform.position.y + 15.5f, transform.position.z), Quaternion.identity); }
        if (KyubuTileValue == 6) { Instantiate(KyubuTiles[5], new Vector3(transform.position.x + 9f, transform.position.y + 6f, transform.position.z), Quaternion.identity); }
        yield return null;
    }

    private void ShootOneDice()
    {
        diceNumber--;
        Instantiate(dicetypes[DiceValues[0]], diceTransform.position, Random.rotation);
        CycleThroughDiceValueArray();
    }
    private void ShootTwoDice()
    {
        if (diceNumber >= 2)
        {
            diceNumber -= 2;
            for (int i = 0; i < 2; i++) 
            { 
                Instantiate(MultishotDiceTypes[DiceValues[0]], diceTransform.position, Random.rotation); 
                CycleThroughDiceValueArray();
            }
        }
        else { ShootOneDice(); }
    }
    private void ShootThreeDice()
    {
        if (diceNumber >= 3)
        {
            diceNumber -= 3;
            for (int i = 0; i < 3; i++) 
            { 
                Instantiate(MultishotDiceTypes[DiceValues[0]], diceTransform.position, Random.rotation); 
                CycleThroughDiceValueArray();
            }
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
        dicePreviewerLevel = loadedPlayerData.dicePreviewerLevel;
        haveFlan = loadedPlayerData.haveFlan;
        haveCremeBrulee = loadedPlayerData.haveCremeBrulee;
        haveBanhmi = loadedPlayerData.haveBanhmi;
    }   
    
    private class PlayerData
    {
        public int diceNumber;
        public float playerCooldownTime;
        public int dicePreviewerLevel;
        public bool haveFlan;
        public bool haveCremeBrulee;
        public bool haveBanhmi;
    }
}
