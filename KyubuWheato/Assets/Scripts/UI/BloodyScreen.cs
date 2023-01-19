using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodyScreen : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private Image bloodBorder;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    void Update()
    {
        if ((player.maxHealth*0.4f - (float)player.playerHealth) > 0)
        {
            float t = Mathf.InverseLerp(player.maxHealth*0.4f, 0, (float)player.playerHealth);
            float a = Mathf.Lerp(50, 160, t);
            bloodBorder.color = new Color32(255, 255, 255, (byte)a);
        }
        else { bloodBorder.color = new Color32(255, 255, 255, 0); }
    }
}
