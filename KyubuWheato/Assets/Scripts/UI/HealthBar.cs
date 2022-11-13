using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject healthFill;
    [SerializeField] private bool isBossBattle;

    private void Start()
    {
        if (isBossBattle == false) { HealthBarFlash(0); }
    }

    public void SetMaxHealth(int playerHealth) 
    {
        healthSlider.maxValue = playerHealth;
        healthSlider.value = playerHealth;
    }

    public void SetHealth(int playerHealth)
    {
        healthSlider.value = playerHealth;
    } 

    public void HealthBarFlash(int flashIndex)
    {
        if (flashIndex == 1) { healthFill.GetComponent<Image>().color = new Color32(170, 70, 70, 200); }
        else if (flashIndex == 2) { healthFill.GetComponent<Image>().color = new Color32(255, 255, 255, 200); }
        else { healthFill.GetComponent<Image>().color = new Color32(135, 0, 0, 200); }
    }
}
