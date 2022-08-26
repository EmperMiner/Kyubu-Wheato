using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DamagePopupDestroy : MonoBehaviour
{
    [SerializeField] private float timeBeforeDestroy;
    [SerializeField] private bool isKyubuTile3Left;
    [SerializeField] private bool isKyubuTile3Right;
    [SerializeField] private bool isKyubuTile6;
    [SerializeField] private GameObject Dice3;
    [SerializeField] private GameObject[] Dice6;
    
    void Start()
    {
        if (isKyubuTile3Left) { StartCoroutine(Summon3Dice(-4.0f)); }
        if (isKyubuTile3Right) { StartCoroutine(Summon3Dice(4.0f)); }
        if (isKyubuTile6) { StartCoroutine(Summon6Dice()); }
        Destroy(gameObject, timeBeforeDestroy);
    }

    void Update()
    {
        
    }

    IEnumerator Summon3Dice(float offset)
    {
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < 3; i++) { Instantiate(Dice3, new Vector3(transform.position.x + offset, transform.position.y, transform.position.z), Random.rotation);  }
        yield return null;
    }

    IEnumerator Summon6Dice()
    {
        yield return new WaitForSeconds(2.85f);
        for (int i = 0; i < 6; i++) { Instantiate(Dice6[i], transform.position, Random.rotation);  }
        yield return null;
    }
}
