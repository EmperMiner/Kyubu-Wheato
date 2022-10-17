using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BG : MonoBehaviour
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Sprite normalBG;
    [SerializeField] private Sprite rareBG;
    void Start()
    {
        int rand = Random.Range(0,100);
        if (rand == 0) { bgImage.sprite = rareBG; }
        else { bgImage.sprite = normalBG; }
    }
}
