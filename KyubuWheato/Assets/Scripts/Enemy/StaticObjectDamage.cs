using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectDamage : MonoBehaviour
{
    private PlayerController player;
    private float objectCanAttack;
    [SerializeField] private int damage;

    [SerializeField] private bool isCornRay;
    [SerializeField] private bool isBossCornRay;
    [SerializeField] private bool isInvertedCornray;
    [SerializeField] private bool isScytheAttack;

    private void Start()
    {
        if (isCornRay) { StartCoroutine(PlayRaySound()); }
        if (isBossCornRay) { StartCoroutine(PlayRaySoundFast()); }
        if (isInvertedCornray) { StartCoroutine(PlayRaySoundInverted()); }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") { objectCanAttack = 0.8f; }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        if (other.gameObject.tag != "Player") return;

        if (objectCanAttack >= 0.8f)
        {
            FindObjectOfType<AudioManager>().PlaySound("PlayerHurt");
            if (isScytheAttack) { player.HitByScythe(); }
            player.UpdateHealth(-damage + Mathf.RoundToInt((damage * player.defense)/10));
            objectCanAttack = 0f;
            
        }    
        objectCanAttack += Time.deltaTime; 

        if (objectCanAttack <= 0.3f) {player.spriteRenderer.material.color = new Color32(255, 150, 150, 255);}
        else { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") { player.spriteRenderer.material.color = new Color32(255, 255, 255, 255); }  
    }

    private IEnumerator PlayRaySound()
    {
        yield return new WaitForSeconds(2.1f);
        FindObjectOfType<AudioManager>().PlaySound("DiceRay3");
        yield return null;
    }
    private IEnumerator PlayRaySoundFast()
    {
        yield return new WaitForSeconds(1.05f);
        FindObjectOfType<AudioManager>().PlaySound("DiceRay3");
        yield return null;
    }
    private IEnumerator PlayRaySoundInverted()
    {
        yield return new WaitForSeconds(0.35f);
        FindObjectOfType<AudioManager>().PlaySound("InvertedCornray");
        yield return null;
    }
}
