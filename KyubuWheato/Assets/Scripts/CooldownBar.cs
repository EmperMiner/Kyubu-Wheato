using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public Slider cooldownSlider;

    void Start()
    {
        cooldownSlider.maxValue = 1f;
        cooldownSlider.value = 1f;
    }    

    public void SetCooldown(float timeUntilCooldownFinish)
    {
        cooldownSlider.value = timeUntilCooldownFinish;
    } 
}
