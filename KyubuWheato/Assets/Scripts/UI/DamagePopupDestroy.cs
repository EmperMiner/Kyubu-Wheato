using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DamagePopupDestroy : MonoBehaviour
{
    [SerializeField] private float timeBeforeDestroy;
    [SerializeField] private bool isKyubuTile6;
    [SerializeField] private GameObject[] Dice6;
    void Start()
    {
        if (isKyubuTile6) { StartCoroutine(Summon6Dice()); }
        Destroy(gameObject, timeBeforeDestroy);
    }

    void Update()
    {
        
    }

    IEnumerator Summon6Dice()
    {
        yield return new WaitForSeconds(2.85f);
        for (int i = 0; i < 6; i++) { Instantiate(Dice6[i], transform.position, Random.rotation);  }
        yield return null;
    }
}
