using UnityEngine;
using UnityEngine.UI;

public class UpgradesImageChanger : MonoBehaviour
{
    public Image UpgradesImage;
    public Sprite[] UpgradesSpritesVariants;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateUpgradesSprite()
    {

    }

    private class PlayerData
    {
        public float MoveSpeed;
        public int maxHealth;
        public int playerHealth;
        public float strength;
        public int Wheat;
        public int diceNumber;
        public float playerCooldownTime;
    }
}
