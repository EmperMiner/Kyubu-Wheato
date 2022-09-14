using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DicePads : MonoBehaviour
{
    [SerializeField] private SpriteRenderer DiceTileRenderer;
    [SerializeField] private Sprite[] DiceTileSprites;

    private bool CheckForValidSpawn = true;

    [SerializeField] private GameObject[] DicePadTypes;
    private PlayerController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Destroy(gameObject, 60f);
        StartCoroutine(ValidSpawn());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool UndesirableSpawn = other.gameObject.tag == "MapCollider" || other.gameObject.tag == "enemyMouse";
        if (CheckForValidSpawn == true && UndesirableSpawn == true)
        {
            Instantiate(DicePadTypes[Random.Range(0,DicePadTypes.Length)], new Vector3(Random.Range(player.LeftMapLimit,player.RightMapLimit), Random.Range(player.LowerMapLimit, player.UpperMapLimit), 0), Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player") { Destroy(gameObject, 20f); }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            DiceTileRenderer.sprite = DiceTileSprites[1];
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            DiceTileRenderer.sprite = DiceTileSprites[0];
        }
    }

    private IEnumerator ValidSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        CheckForValidSpawn = false;
        yield return null;
    }
}
