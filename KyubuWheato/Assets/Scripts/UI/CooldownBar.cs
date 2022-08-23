using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CooldownBar : MonoBehaviour
{
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private GameObject cooldownFill;
    [SerializeField] private GameObject cooldownBorder;
    private float CDBarPlayerCooldownTime;

    void Start()
    {
        LoadData();
        cooldownSlider.maxValue = CDBarPlayerCooldownTime;
        cooldownSlider.value = CDBarPlayerCooldownTime;
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

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);

        CDBarPlayerCooldownTime = loadedPlayerData.playerCooldownTime;
    }   
    private class PlayerData
    {
        public float playerCooldownTime;
    }
}
