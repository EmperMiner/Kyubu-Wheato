using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crowThrow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer crowSpriteRenderer;
    [SerializeField] private Sprite crowLeft;
    [SerializeField] private Sprite crowRight;
    [SerializeField] private float crowSpeed;
    private Transform playerTransform;
    private PlayerController player;
    private Vector2 target;

    private float crowCanAttack;

    private int randValue;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Destroy(gameObject, 5f);

        target = new Vector2(playerTransform.position.x, playerTransform.position.y);
        randValue = Random.Range(0,2);
    }

    private void Update() 
    {
        if (playerTransform.position.x > transform.position.x) { crowSpriteRenderer.sprite = crowRight; }
        else { crowSpriteRenderer.sprite = crowLeft;  }

        if (randValue < 1) { transform.position = Vector2.MoveTowards(transform.position, target, crowSpeed * Time.deltaTime); }
        else { transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, crowSpeed * Time.deltaTime); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MapCollider" || other.gameObject.tag == "chest") { Destroy(gameObject); }
        crowCanAttack = 3f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag != "Player") return;

        if (crowCanAttack >= 3f)
        {
            FindObjectOfType<AudioManager>().PlaySound("PlayerHurt");
            player.UpdateHealth(-3 + Mathf.RoundToInt((3 * player.defense)/10));
            crowCanAttack = 0f;
        }    
        crowCanAttack += Time.deltaTime; 

        if (crowCanAttack <= 0.3f) {player.spriteRenderer.material.color = new Color32(255, 150, 150, 255);}
        else { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
    }
}
