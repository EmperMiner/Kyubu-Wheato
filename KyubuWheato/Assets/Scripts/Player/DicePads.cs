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
    private Transform playerTransform;
    

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
        if (other.gameObject.tag == "Player") { StartCoroutine(DestroyPad()); }
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

    private IEnumerator DestroyPad()
    {
        yield return new WaitForSeconds(20f);
        if (Vector2.Distance(this.transform.position, playerTransform.position) < 8f ){ FindObjectOfType<AudioManager>().PlaySound("PadDestroy"); }
        Destroy(gameObject);
        yield return null;
    }
}
