using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomTileTransition : MonoBehaviour
{
    [SerializeField] private Image currentImage;
    [SerializeField] private Sprite[] tileSprites;
    void Start()
    {
        currentImage.sprite = tileSprites[Random.Range(0, tileSprites.Length)];
    }
}
