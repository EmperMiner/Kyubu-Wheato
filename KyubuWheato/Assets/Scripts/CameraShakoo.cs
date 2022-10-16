using UnityEngine;
using EZCameraShake;
using System.Collections.Generic;
using System.Collections;

public class CameraShakoo : MonoBehaviour
{
    [SerializeField] private float Delay;
    [SerializeField] private bool isKyubuKombo6;
    private void Start()
    {
        if (isKyubuKombo6) {StartCoroutine(ShakeItHeavy(Delay)); }
        else { StartCoroutine(ShakeIt(Delay)); }
    }

    IEnumerator ShakeIt(float ShakeDelay)
    {
        yield return new WaitForSeconds(ShakeDelay);
        CameraShaker.Instance.ShakeOnce(2f, 5f, .1f, .5f);
        yield return null;
    }
    IEnumerator ShakeItHeavy(float ShakeDelay)
    {
        yield return new WaitForSeconds(ShakeDelay);
        CameraShaker.Instance.ShakeOnce(3f, 7f, .1f, 2f);
        yield return null;
    }

    public void PlayKK1() { FindObjectOfType<AudioManager>().PlaySound("KK1"); }
}
