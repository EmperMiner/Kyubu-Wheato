using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using System.IO;

public class diceMagnetize : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    private float fireForce = 12;
    private float spinForce = 1000;
    private float rot;
    private PlayerController player;
    private DiceThrow diceThrowScript;
    private Transform playerTransform;
    
    [SerializeField] private bool DiceIsMultishot;
    [SerializeField] private bool Magnetizable;
    [SerializeField] private float directionX;
    [SerializeField] private float directionY;
    [SerializeField] private GameObject[] DiceRayPrefabs;
    [SerializeField] private GameObject ghost;
    private bool haveCupcake;
    private bool havePastelDeChoclo;
    private Vector2 enemyTargetVector;
    private float DiceRayAngleOffset;

    public bool pickupable;
    private bool LifeStolen;

    void Start()
    {
        LifeStolen = false;
        LoadData();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        diceThrowScript = GameObject.FindGameObjectWithTag("DiceManager").GetComponent<DiceThrow>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * fireForce;

        if (DiceIsMultishot) { rot = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 60f; }
        else { rot = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 90f; }

        AddTorqueImpulse(spinForce);
        transform.rotation = Quaternion.Euler(0, 0, rot);

        if (havePastelDeChoclo) { StartCoroutine(RollExplosionChance()); }

        if (gameObject.tag == "FakeDice1") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice2") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice3") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice4") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice5") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice6") { DestroyFakeDice(); }


        if (Magnetizable && haveCupcake) 
        {
            StartCoroutine(Wait());
        }
        pickupable = false;
        StartCoroutine(PickupDelay());
        StartCoroutine(SpawnGhost());
    }

    private void AddTorqueImpulse(float angularChangeInDegrees)
    {
        var impulse = (angularChangeInDegrees * Mathf.Deg2Rad) * rb.inertia;
        rb.AddTorque(impulse, ForceMode2D.Impulse);
    }

    private void DestroyFakeDice()
    {
        Destroy(gameObject, 1.5f);
    }

    private IEnumerator PickupDelay()
    {
        yield return new WaitForSeconds(0.15f);
        pickupable = true;
        yield return null;
    }

    private IEnumerator SpawnGhost()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(23f,27f));
        Instantiate(ghost, new Vector3(transform.position.x + UnityEngine.Random.Range(-10f, 10f), transform.position.y + UnityEngine.Random.Range(-10f, 10f), 0), Quaternion.identity); 
        StartCoroutine(SpawnGhost());
        yield return null;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(4f);
        FindObjectOfType<AudioManager>().PlaySound("MagnetizedDice");
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AudioManager>().PlaySound("DicePickup");
        player.IncreaseDiceNumber();
        Destroy(gameObject);
        yield return null;
    }
    
    private IEnumerator RollExplosionChance()
    {
        yield return new WaitForSeconds(1f);
        int i = UnityEngine.Random.Range(0,130);
        if (i < 10  + diceThrowScript.KyubuStack) { Explode(); }
        yield return null;
    }

    private void Explode()
    {
        player.IncreaseDiceNumber(); 
        if (this.gameObject.tag == "6sidedDice5")
        {
            FindObjectOfType<AudioManager>().PlaySound("DiceRay5");
        }
        else if (this.gameObject.tag == "6sidedDice3")
        {
            FindObjectOfType<AudioManager>().PlaySound("DiceRay3");
        }
        else 
        {
            FindObjectOfType<AudioManager>().PlaySound("DiceRayNot5");
        }
        if (this.gameObject.tag == "6sidedDice1") { Instantiate(DiceRayPrefabs[0], transform.position, Quaternion.identity); }
        if (this.gameObject.tag == "6sidedDice2") 
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
        if (this.gameObject.tag == "6sidedDice3") { Instantiate(DiceRayPrefabs[2], transform.position, Quaternion.Euler(0, 0 , UnityEngine.Random.Range(0f,360f))); }
        if (this.gameObject.tag == "6sidedDice4") { Instantiate(DiceRayPrefabs[UnityEngine.Random.Range(3,5)], transform.position, Quaternion.Euler(0, 0 , UnityEngine.Random.Range(0f,360f))); }
        if (this.gameObject.tag == "6sidedDice5") 
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
        if (this.gameObject.tag == "6sidedDice6") 
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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (pickupable && other.gameObject.tag == "Player")
        {
        pickupable = false;
        player.IncreaseDiceNumber(); 
        FindObjectOfType<AudioManager>().PlaySound("DicePickup");
        Destroy(this.gameObject);
        }
    } 

    private void OnTriggerEnter2D(Collider2D other) 
    {
        bool Lifesteal = this.gameObject.tag == "ChargedDice2" || this.gameObject.tag == "ChargedDice2" || this.gameObject.tag == "ChargedDice2" || this.gameObject.tag == "FakeDice8"|| this.gameObject.tag == "FakeDice8"|| this.gameObject.tag == "FakeDice8";
        if (!Lifesteal) { return; }
        if (other.gameObject.tag == "enemyMouse" && LifeStolen == false) { FindObjectOfType<AudioManager>().PlaySound("Lifesteal"); }
        if (other.gameObject.tag == "enemyMouse" && this.gameObject.tag == "ChargedDice2" && LifeStolen == false )
        { player.UpdateHealth(Mathf.RoundToInt(player.maxHealth*0.007f*(PlayerPrefs.GetFloat("DiceSpinLevelUp")) + 2.5f));}
        if (other.gameObject.tag == "enemyMouse" && this.gameObject.tag == "ChargedDice4" && LifeStolen == false )
        { player.UpdateHealth(Mathf.RoundToInt(player.maxHealth*0.007f*(PlayerPrefs.GetFloat("DiceSpinLevelUp")) + 2f));}
        if (other.gameObject.tag == "enemyMouse" && this.gameObject.tag == "ChargedDice6" && LifeStolen == false )
        { player.UpdateHealth(Mathf.RoundToInt(player.maxHealth*0.007f*(PlayerPrefs.GetFloat("DiceSpinLevelUp")) + 1.5f));}
        if (other.gameObject.tag == "enemyMouse" && this.gameObject.tag == "FakeDice8" && LifeStolen == false )
        { player.UpdateHealth(Mathf.RoundToInt(player.maxHealth*0.007f*(PlayerPrefs.GetFloat("DiceSpinLevelUp")) + 1f));}
        if (other.gameObject.tag == "enemyMouse" && this.gameObject.tag == "FakeDice10" && LifeStolen == false )
        { player.UpdateHealth(Mathf.RoundToInt(player.maxHealth*0.007f*(PlayerPrefs.GetFloat("DiceSpinLevelUp")) + 0.5f));}
        if (other.gameObject.tag == "enemyMouse" && this.gameObject.tag == "FakeDice12" && LifeStolen == false )
        { player.UpdateHealth(Mathf.RoundToInt(player.maxHealth*0.007f*(PlayerPrefs.GetFloat("DiceSpinLevelUp"))));}
        if (other.gameObject.tag == "enemyMouse") { LifeStolen = true; }
    }

    private void LoadData()
    {
        string json = File.ReadAllText(Application.dataPath + "/ingameSaveData.json");
        PlayerData loadedPlayerData = JsonUtility.FromJson<PlayerData>(json);
        
        haveCupcake = loadedPlayerData.haveCupcake;
        havePastelDeChoclo = loadedPlayerData.havePastelDeChoclo;
    }   

    private class PlayerData
    {
        public bool haveCupcake;
        public bool havePastelDeChoclo;
    }
}

