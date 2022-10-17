using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject healthFill;
    [SerializeField] private bool isBossBattle;

    private void Start()
    {
        if (isBossBattle == false) { HealthBarFlash(false); }
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

    public void HealthBarFlash(bool flashIndex)
    {
        if (flashIndex) { healthFill.GetComponent<Image>().color = new Color32(170, 70, 70, 200); }
        else { healthFill.GetComponent<Image>().color = new Color32(135, 0, 0, 200); }
    }
}
