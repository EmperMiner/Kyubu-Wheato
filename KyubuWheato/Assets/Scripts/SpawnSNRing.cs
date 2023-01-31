using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSNRing : MonoBehaviour
{
    [SerializeField] private GameObject ring;
    void Start()
    {
        StartCoroutine(SpawnRing());
    }
    private IEnumerator SpawnRing()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 3; i++)
        {
            Instantiate(ring, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
