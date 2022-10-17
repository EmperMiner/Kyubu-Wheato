using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInvis : MonoBehaviour
{
    [SerializeField] private SpriteRenderer AppleTreeUpperHalfSprite;
    [SerializeField] private GameObject AppleTreeRoots;
    private SpriteRenderer AppleTreeRootsSprite;

    private void Start()
    {
        AppleTreeRootsSprite = AppleTreeRoots.GetComponent<SpriteRenderer>();
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player") { AppleTreeUpperHalfSprite.color = new Color32(255, 255, 255, 180); AppleTreeRootsSprite.color = new Color32(255, 255, 255, 180); }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player") { AppleTreeUpperHalfSprite.color = new Color32(255, 255, 255, 255); AppleTreeRootsSprite.color = new Color32(255, 255, 255, 255); }
    }
}
