using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int Level;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ArrowVisible = false;
        EnemiesKilled = 0;
        notBossFight = true;
        haveRedWheat = false;
        haveBlueWheat = false;
        haveGreenWheat = false;
    }

    private void Update()
    {
        if (EnemiesKilled >= EnemyLimit && ExitHoe.activeSelf == false && notBossFight) { ExitHoe.SetActive(true); FindObjectOfType<AudioManager>().PlaySound("Victory"); ArrowVisible = true; }
       /* if (player.haveHornScallop && EnemiesKilled >= Mathf.RoundToInt(EnemyLimit/5)) 
       { 

       }
        if (player.haveHornScallop && EnemiesKilled >= Mathf.RoundToInt(EnemyLimit/2)) 
       { 
        
       }


        */
    }


}
