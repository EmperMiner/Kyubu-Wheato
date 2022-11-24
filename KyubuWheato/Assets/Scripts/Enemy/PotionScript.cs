using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private SpriteRenderer blackCatSprite;
    [SerializeField] private Sprite[] potionVariants;
    [SerializeField] private GameObject surge;
    [SerializeField] private Vector2 target;
    [SerializeField] private float potionSpeed;
    private int potionVariant;
    private void Start()
    {
        potionSpeed = 4f;
        potionVariant = Random.Range(0,4);
        blackCatSprite.sprite = potionVariants[potionVariant];
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(playerTransform.position.x + Random.Range(-3f,3f), playerTransform.position.y + Random.Range(-3f,3f));
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, potionSpeed * Random.Range(0.75f, 1.25f) * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y ) { PotionCrack(); }
    }

    private void PotionCrack()
    {
        Instantiate(surge, transform.position, Quaternion.Euler(0, 0, potionVariant*90));
        FindObjectOfType<AudioManager>().PlaySound("WitchPotion");
        Destroy(gameObject);
    }

}
