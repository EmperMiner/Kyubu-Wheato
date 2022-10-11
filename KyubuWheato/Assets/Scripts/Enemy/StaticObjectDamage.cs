using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectDamage : MonoBehaviour
{
    private PlayerController player;
    private float objectCanAttack;

    [SerializeField] private bool isCornRay;

    private void Start()
    {
        if (isCornRay) { StartCoroutine(PlayRaySound()); }
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
            player.UpdateHealth(-10 + Mathf.RoundToInt((10 * player.defense)/10));
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
}
