using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class FSCSummon : MonoBehaviour
{
    [SerializeField] private GameObject[] FSCRay;   
    [SerializeField] private GameObject KillPivot;
    void Start()
    {
        StartCoroutine(SummonRays());
        Instantiate(KillPivot, transform.position, Quaternion.identity);
        StartCoroutine(ShakeIt());
    }

    
    IEnumerator SummonRays()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < Random.Range(10,15); i++)
        {
            Instantiate(FSCRay[Random.Range(0,3)], new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y, transform.position.z), Quaternion.Euler(0, 0, Random.Range(0,2)*180));
            yield return new WaitForSeconds(Random.Range(0.03f, 0.18f));
        }
        yield return null;
    }

    private IEnumerator ShakeIt()
    {
        yield return new WaitForSeconds(1.8f);
        CameraShaker.Instance.ShakeOnce(10f, 10f, .2f, .5f);
        yield return new WaitForSeconds(0.4f);
        CameraShaker.Instance.ShakeOnce(10f, 10f, .2f, .5f);
        yield return new WaitForSeconds(0.4f);
        CameraShaker.Instance.ShakeOnce(10f, 10f, .2f, .5f);
        yield return null;
    }
}
