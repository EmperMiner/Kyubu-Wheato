using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public void SetMaxHealth(int playerHealth) 
    {
        healthSlider.maxValue = playerHealth;
        healthSlider.value = playerHealth;
    }

    public void SetHealth(int playerHealth)
    {
        healthSlider.value = playerHealth;
    } 
}
