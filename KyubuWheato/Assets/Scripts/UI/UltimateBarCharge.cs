using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UltimateBarCharge : MonoBehaviour
{
    [SerializeField] private Slider ultimateChargeSlider;
    [SerializeField] private GameObject ultimateCharge;

    private int maxUltimateCharge = 200;

    private int currentUltimateCharge = 0;

    private void Start()
    {
        ultimateChargeSlider.maxValue = maxUltimateCharge;
        ultimateChargeSlider.value = 0;
        UltimateBarFlash(false);
    }

    public void IncreaseUltimateCharge(int UltimateChargeAmount)
    {
        currentUltimateCharge += UltimateChargeAmount;
        if (currentUltimateCharge > maxUltimateCharge) { currentUltimateCharge = maxUltimateCharge;}
        ultimateChargeSlider.value = currentUltimateCharge;
        StartCoroutine(FlashingUltimateBar());
    } 

    public void UltimateBarFlash(bool flashIndex)
    {
        if (flashIndex) { ultimateCharge.GetComponent<Image>().color = new Color32(200, 200, 200, 255); }
        else { ultimateCharge.GetComponent<Image>().color = new Color32(255, 255, 255, 200); }
    }

    IEnumerator FlashingUltimateBar()
    {
        UltimateBarFlash(true);
        yield return new WaitForSeconds(0.25f);
        UltimateBarFlash(false);
        yield return null;
    }
}
