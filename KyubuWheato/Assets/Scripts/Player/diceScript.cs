using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using System.IO;

public class diceScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    private float fireForce = 12;
    private float spinForce = 1000;
    private float rot;
    private DiceThrow diceThrowScript;
    [SerializeField] private bool DiceIsMultishot;
    [SerializeField] private bool DiceIsFakeMultishotLeft;
    [SerializeField] private bool DiceIsFakeMultishotRight;
    [SerializeField] private bool DiceIsKyubuTile3;
    [SerializeField] private bool DiceIsKyubuTile6;
    [SerializeField] private float directionX;
    [SerializeField] private float directionY;
    [SerializeField] private GameObject[] DiceRayPrefabs;
    [SerializeField] private bool isStar;
    [SerializeField] private bool isDiceSpin;
    private byte[,] starColors = new byte[18,3] {
        { 255, 51, 51 },
        { 255, 153, 51 },
        { 255, 255, 51 },
        { 153, 255, 51 },
        { 51, 255, 51 },
        { 51, 255, 153 },
        { 51, 255, 255 },
        { 51, 153, 255 },
        { 51, 51, 255 },
        { 153, 51, 255 },
        { 255, 51, 255 },
        { 255, 51, 153 },
        { 255, 255, 255 },
        { 153, 76, 0 },
        { 204, 255, 255 },
        { 255, 204, 255 },
        { 204, 255, 204 },
        { 255, 255, 204 },
    };
    private bool havePastelDeChoclo;
    private Vector2 enemyTargetVector;
    private float DiceRayAngleOffset;
    private SpriteRenderer StarSprite;

    [SerializeField] private bool dontExplode;
    [SerializeField] private GameObject starItself;

    void Start()
    {
        LoadData();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        diceThrowScript = GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceThrow>();

        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;

        if (DiceIsFakeMultishotLeft) { rb.velocity = Quaternion.Euler(0, 0, -20) * new Vector2(direction.x, direction.y).normalized * fireForce; }
        else if (DiceIsFakeMultishotRight) { rb.velocity = Quaternion.Euler(0, 0, 20) * new Vector2(direction.x, direction.y).normalized * fireForce; }
        else if (DiceIsKyubuTile3) { rb.velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * 40; }
        else if (DiceIsKyubuTile6) { rb.velocity = new Vector2(directionX, directionY).normalized * 4; }
        else { rb.velocity = new Vector2(direction.x, direction.y).normalized * fireForce; }

        if (DiceIsMultishot) { rot = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 60f; }
        else { rot = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 90f; }

        AddTorqueImpulse(spinForce);
        transform.rotation = Quaternion.Euler(0, 0, rot);

        if (havePastelDeChoclo) { StartCoroutine(RollExplosionChance());}

        if (isStar)
        {
            StarSprite = starItself.GetComponent<SpriteRenderer>();
            int i = UnityEngine.Random.Range(0,starColors.GetLength(0));
            StarSprite.color = new Color32(starColors[i,0], starColors[i,1], starColors[i,2], 255);
            float sizeRandom = UnityEngine.Random.Range(0.4f, 2.5f);
            transform.localScale = new Vector3(sizeRandom, sizeRandom, 1f);
            rb.velocity = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-70f, 70f)) * new Vector2(direction.x, direction.y).normalized * (fireForce * UnityEngine.Random.Range(0.25f, 1f));
        }

        if (gameObject.tag == "FakeDice1") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice2") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice3") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice4") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice5") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice6") { DestroyFakeDice(); }
    }
    private void AddTorqueImpulse(float angularChangeInDegrees)
    {
        var impulse = (angularChangeInDegrees * Mathf.Deg2Rad) * rb.inertia;
        rb.AddTorque(impulse, ForceMode2D.Impulse);
    }
    private void DestroyFakeDice()
    {
        if (!isDiceSpin) { Destroy(gameObject, 1.5f); }
    }
    IEnumerator RollExplosionChance()
    {
        
        yield return new WaitForSeconds(1f);
        int i = UnityEngine.Random.Range(0,130);
        if (i < 30 + diceThrowScript.KyubuStack && dontExplode == false) { Explode(); }
        yield return null;
    }

    private void Explode()
    {
        if (this.gameObject.tag == "FakeDice5")
        {
            FindObjectOfType<AudioManager>().PlaySound("DiceRay5");
        }
        else if (this.gameObject.tag == "FakeDice3")
        {
            FindObjectOfType<AudioManager>().PlaySound("DiceRay3");
        }
        else 
        {
            FindObjectOfType<AudioManager>().PlaySound("DiceRayNot5");
        }
        if (this.gameObject.tag == "FakeDice1") { Instantiate(DiceRayPrefabs[0], transform.position, Quaternion.identity); }
        if (this.gameObject.tag == "FakeDice2") 
        { 
            try
            { 
                Transform enemyPosition = GameObject.FindWithTag("enemyMouse").GetComponent<Transform>(); 
                enemyTargetVector = enemyPosition.position - transform.position;
                DiceRayAngleOffset = Vector2.Angle(Vector2.left, enemyTargetVector);
                if (enemyPosition.position.y > transform.position.y) { Instantiate(DiceRayPrefabs[1], transform.position, Quaternion.Euler(0, 0, 360 - DiceRayAngleOffset)); }
                else { Instantiate(DiceRayPrefabs[1], transform.position, Quaternion.Euler(0, 0, DiceRayAngleOffset)); }
            }
            catch (NullReferenceException) 
            { 
                Instantiate(DiceRayPrefabs[1], transform.position, Quaternion.identity); 
            }
        }
        if (this.gameObject.tag == "FakeDice3") { Instantiate(DiceRayPrefabs[2], transform.position, Quaternion.Euler(0, 0 , UnityEngine.Random.Range(0f,360f))); }
        if (this.gameObject.tag == "FakeDice4") { Instantiate(DiceRayPrefabs[UnityEngine.Random.Range(3,5)], transform.position, Quaternion.Euler(0, 0 , UnityEngine.Random.Range(0f,360f))); }
        if (this.gameObject.tag == "FakeDice5") 
        { 
            try
            { 
                Transform enemyPosition = GameObject.FindWithTag("enemyMouse").GetComponent<Transform>(); 
                enemyTargetVector = enemyPosition.position - transform.position;
                DiceRayAngleOffset = Vector2.Angle(Vector2.left, enemyTargetVector);
                if (enemyPosition.position.y > transform.position.y) { Instantiate(DiceRayPrefabs[5], transform.position, Quaternion.Euler(0, 0, 180 - DiceRayAngleOffset)); }
                else { Instantiate(DiceRayPrefabs[5], transform.position, Quaternion.Euler(0, 0, DiceRayAngleOffset + 180)); }
            }
            catch (NullReferenceException) 
            { 
                Instantiate(DiceRayPrefabs[5], transform.position, Quaternion.identity); 
            }
        }
        if (this.gameObject.tag == "FakeDice6") 
        { 
            try
            { 
                Transform enemyPosition = GameObject.FindWithTag("enemyMouse").GetComponent<Transform>(); 
                enemyTargetVector = enemyPosition.position - transform.position;
                DiceRayAngleOffset = Vector2.Angle(Vector2.left, enemyTargetVector);
                if (enemyPosition.position.y > transform.position.y) { Instantiate(DiceRayPrefabs[6], transform.position, Quaternion.Euler(0, 0, 180 - DiceRayAngleOffset)); }
                else { Instantiate(DiceRayPrefabs[6], transform.position, Quaternion.Euler(0, 0, DiceRayAngleOffset + 180)); }
            }
            catch (NullReferenceException) 
            { 
                Instantiate(DiceRayPrefabs[6], transform.position, Quaternion.identity); 
            }
        }
        Destroy(gameObject);
    }

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/gameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);
        
        havePastelDeChoclo = loadedPlayerData.havePastelDeChoclo;
    }   

    private class PlayerData
    {
        public bool havePastelDeChoclo;
    }
}
