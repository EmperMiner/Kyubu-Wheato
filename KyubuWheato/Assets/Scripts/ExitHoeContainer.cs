using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHoeContainer : MonoBehaviour
{
    public int EnemiesKilled;
    [SerializeField] private int EnemyLimit;
    [SerializeField] private GameObject ExitHoe;
    public bool ArrowVisible;
    private void Start()
    {
        ArrowVisible = false;
        EnemiesKilled = 0;
    }

    private void Update()
    {
        if (EnemiesKilled >= EnemyLimit && ExitHoe.activeSelf == false) { ExitHoe.SetActive(true); FindObjectOfType<AudioManager>().PlaySound("Victory"); ArrowVisible = true; }
    }
}
