using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diceMagnetize : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    private float fireForce = 12;
    private float spinForce = 1000;
    private float rot;
    private Transform playerTransform;
    private UnityEngine.AI.NavMeshAgent agent;
    private bool Magnetized = false;
    [SerializeField] private bool DiceIsMultishot;
    [SerializeField] private bool DiceIsFakeMultishotLeft;
    [SerializeField] private bool DiceIsFakeMultishotRight;
    [SerializeField] private bool DiceIsKyubuTile3;
    [SerializeField] private bool DiceIsKyubuTile6;
    [SerializeField] private bool Magnetizable;
    [SerializeField] private float directionX;
    [SerializeField] private float directionY;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;

        if (DiceIsFakeMultishotLeft) { rb.velocity = Quaternion.Euler(0, 0, -20) * new Vector2(direction.x, direction.y).normalized * fireForce; }
        else if (DiceIsFakeMultishotRight) { rb.velocity = Quaternion.Euler(0, 0, 20) * new Vector2(direction.x, direction.y).normalized * fireForce; }
        else if (DiceIsKyubuTile3) { rb.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 40; }
        else if (DiceIsKyubuTile6) { rb.velocity = new Vector2(directionX, directionY).normalized * 4; }
        else { rb.velocity = new Vector2(direction.x, direction.y).normalized * fireForce; }

        if (DiceIsMultishot) { rot = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 60f; }
        else { rot = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 90f; }

        AddTorqueImpulse(spinForce);
        transform.rotation = Quaternion.Euler(0, 0, rot);

        if (gameObject.tag == "FakeDice1") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice2") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice3") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice4") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice5") { DestroyFakeDice(); }
        if (gameObject.tag == "FakeDice6") { DestroyFakeDice(); }

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (Magnetizable) 
        {
            StartCoroutine(Wait());
        }
    }

    private void FixedUpdate()
    {
        if (Magnetized) { agent.SetDestination(playerTransform.position); }
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

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        Magnetized = true;
        yield return null;
    }
}
