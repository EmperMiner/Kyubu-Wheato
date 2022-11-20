using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class crowThrow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer crowSpriteRenderer;
    [SerializeField] private Sprite crowLeft;
    [SerializeField] private Sprite crowRight;
    [SerializeField] private float crowSpeed;

    [SerializeField] private bool isInverted;
    private Transform playerTransform;
    private PlayerController player;
    private Vector2 target;
    private byte x;
    private byte y;
    private byte z;

    private float crowCanAttack;

    private int randValue;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Destroy(gameObject, 5f);
        if (isInverted) 
        { 
            Destroy(gameObject, UnityEngine.Random.Range(1.5f, 5f)); 
            StartCoroutine(ChangeColor());
        }

        target = new Vector2(playerTransform.position.x, playerTransform.position.y);
        randValue = UnityEngine.Random.Range(0,2);
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
        if (other.gameObject.tag == "MapCollider" || other.gameObject.tag == "chest" || other.gameObject.tag == "100sidedDice" || other.gameObject.tag == "TimeCrescent") { Destroy(gameObject); }
        if (other.gameObject.tag == "Player") { crowCanAttack = 3f; }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag != "Player") return;

        int attackDamage = 3;
        if (isInverted) { attackDamage = 10; }

        if (crowCanAttack >= 3f)
        {
            if (player.Invincible == false) { FindObjectOfType<AudioManager>().PlaySound("PlayerHurt"); }
            else { FindObjectOfType<AudioManager>().PlaySound("Iframe"); }
            player.UpdateHealth(-attackDamage + Mathf.RoundToInt((attackDamage * player.defense)/10));
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

    IEnumerator ChangeColor()
    {
        crowSpriteRenderer.material.color = new Color32(x, y , z, 255);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.7f));
        x = Convert.ToByte(UnityEngine.Random.Range(0,256));
        y = Convert.ToByte(UnityEngine.Random.Range(0,256));
        z = Convert.ToByte(UnityEngine.Random.Range(0,256));
        StartCoroutine(ChangeColor());
        yield return null;
    }
}
