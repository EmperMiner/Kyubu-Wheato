using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitHoeContainer : MonoBehaviour
{

    private PlayerController player;

    public int EnemiesKilled;
    [SerializeField] private int EnemyLimit;
    [SerializeField] private GameObject ExitHoe;
    public bool notBossFight;
    public bool ArrowVisible;

    public bool haveRedWheat;
    public bool haveBlueWheat;
    public bool haveGreenWheat;

    private Transform BotLeft;
    private Transform TopRight;
    private Transform playerTransform;

    private GameObject playerMap;
    private GameObject MapImageObject;
    public int Level;


    private Image MapImage;
    [SerializeField] private Sprite[] MapSprites;

    private void Awake() 
    {
        MapImage = GameObject.Find("MapDisplay").GetComponent<Image>();
        MapImageObject = GameObject.Find("MapDisplay");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerMap = GameObject.Find("PlayerOnMap");
        playerMap.SetActive(false);
        if (SceneManager.GetActiveScene().buildIndex != 15) { BotLeft = GameObject.FindGameObjectWithTag("BL").transform; }
        if (SceneManager.GetActiveScene().buildIndex != 15) { TopRight = GameObject.FindGameObjectWithTag("TR").transform; }
        MapImageObject.SetActive(false);
    }

    private void Start()
    {
        ArrowVisible = false; 
        if (SceneManager.GetActiveScene().buildIndex != 14) { EnemiesKilled = 0; }
        notBossFight = true;
        haveRedWheat = false;
        haveBlueWheat = false;
        haveGreenWheat = false;
    }

    private void Update()
    {
        if (EnemiesKilled >= EnemyLimit && ExitHoe.activeSelf == false && notBossFight) { ExitHoe.SetActive(true); FindObjectOfType<AudioManager>().PlaySound("Victory"); ArrowVisible = true; }

        if (player.haveHornScallop && EnemiesKilled >= Mathf.RoundToInt(EnemyLimit/5) && MapImageObject.activeSelf == false && SceneManager.GetActiveScene().buildIndex != 14 && SceneManager.GetActiveScene().buildIndex != 15) { ActivateMap(); }
        else if (player.haveHornScallop == false && EnemiesKilled >= Mathf.RoundToInt(EnemyLimit*3/10) && MapImageObject.activeSelf == false && SceneManager.GetActiveScene().buildIndex != 14 && SceneManager.GetActiveScene().buildIndex != 15) { ActivateMap(); }
        
        if (player.haveHornScallop && EnemiesKilled >= Mathf.RoundToInt(EnemyLimit/2) && MapImage.sprite == MapSprites[2*(SceneManager.GetActiveScene().buildIndex - 4)] 
        && SceneManager.GetActiveScene().buildIndex != 4 && SceneManager.GetActiveScene().buildIndex != 9 && SceneManager.GetActiveScene().buildIndex != 14 && SceneManager.GetActiveScene().buildIndex != 15)  
        { 
            player.StartCoroutine(player.NotifTextKeyUnlock());
            MapImage.sprite = MapSprites[2*(SceneManager.GetActiveScene().buildIndex - 4) + 1];
        }

        if (player.haveHornScallop && EnemiesKilled >= Mathf.RoundToInt(EnemyLimit/5) && SceneManager.GetActiveScene().buildIndex != 15) { MovePlayerOnMap(); }
        else if (player.haveHornScallop == false && EnemiesKilled >= Mathf.RoundToInt(EnemyLimit*3/10) && SceneManager.GetActiveScene().buildIndex != 15) { MovePlayerOnMap(); }
    }

    private void LateUpdate() 
    {
        if (SceneManager.GetActiveScene().buildIndex == 14 && MapImageObject.activeSelf == false) { ActivateMap(); }
        if (SceneManager.GetActiveScene().buildIndex == 14) { MovePlayerOnMap(); }
    }

    private void MovePlayerOnMap()
    {
        var playerOnMap = playerMap.GetComponent<RectTransform>();
        float xLerped = Mathf.InverseLerp(BotLeft.position.x, TopRight.position.x, playerTransform.position.x);
        float yLerped = Mathf.InverseLerp(BotLeft.position.y, TopRight.position.y, playerTransform.position.y);

        float xValue = Mathf.Lerp(-337f, 334f, xLerped);
        float yValue = Mathf.Lerp(-141f, 128f, yLerped);

        playerOnMap.anchoredPosition = new Vector3(xValue, yValue, 0.5f);
    }

    private void ActivateMap()
    {
        playerMap.SetActive(true);
        player.StartCoroutine(player.NotifTextMapUnlock());
        MapImageObject.SetActive(true);
        MapImage.sprite = MapSprites[2*(SceneManager.GetActiveScene().buildIndex - 4)];
    }
}
