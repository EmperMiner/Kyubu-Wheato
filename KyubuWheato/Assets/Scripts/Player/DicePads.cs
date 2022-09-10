using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DicePads : MonoBehaviour
{
    private DicePadSpawner dicePadSpawnerScript;
    [SerializeField] private SpriteRenderer DiceTileRenderer;
    [SerializeField] private Sprite[] DiceTileSprites;
    private bool CheckForValidSpawn = true;
    private bool UndesirableSpawn = false;

    void Start()
    {
        dicePadSpawnerScript = GameObject.FindGameObjectWithTag("DicePadManager").GetComponent<DicePadSpawner>();
        Destroy(gameObject, 60f);
        StartCoroutine(ValidSpawn());
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MapCollider" || other.gameObject.tag == "enemyMouse") { UndesirableSpawn = true;}
        if (CheckForValidSpawn == true && UndesirableSpawn == true)
        {
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

    IEnumerator ValidSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        CheckForValidSpawn = false;
        yield return null;
    }
}
