using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public Slider cooldownSlider;
    [SerializeField] private GameObject cooldownFill;
    [SerializeField] private GameObject cooldownBorder;

    void Start()
    {
        cooldownSlider.maxValue = 1f;
        cooldownSlider.value = 1f;
        CooldownBarInvisible();
    }    

    public void SetCooldown(float timeUntilCooldownFinish)
    {
        cooldownFill.GetComponent<Image>().color = new Color32(140, 140, 150, 180);
        cooldownBorder.GetComponent<Image>().color = new Color32(30, 0, 0, 180);
        cooldownSlider.value = timeUntilCooldownFinish;
    } 

    public void CooldownBarInvisible()
    {
        cooldownFill.GetComponent<Image>().color = new Color32(140, 140, 150, 0);
        cooldownBorder.GetComponent<Image>().color = new Color32(30, 0, 0, 0);
    }
}
