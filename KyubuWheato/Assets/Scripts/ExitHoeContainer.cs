using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHoeContainer : MonoBehaviour
{
    public int EnemiesKilled;
    [SerializeField] private int EnemyLimit;
    [SerializeField] private GameObject ExitHoe;
    public bool notBossFight;
    public bool ArrowVisible;
    private void Start()
    {
        ArrowVisible = false;
        EnemiesKilled = 0;
        notBossFight = true;
    }

    private void Update()
    {
        if (EnemiesKilled >= EnemyLimit && ExitHoe.activeSelf == false && notBossFight) { ExitHoe.SetActive(true); FindObjectOfType<AudioManager>().PlaySound("Victory"); ArrowVisible = true; }
    }
}
