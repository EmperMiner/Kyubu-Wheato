using UnityEngine;

public class diceScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float fireForce = 12;
    public float spinForce = 1000;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * fireForce;
        AddTorqueImpulse(spinForce);

        float rot = (Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg) - 90f;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    
    public void AddTorqueImpulse(float angularChangeInDegrees)
    {
        var impulse = (angularChangeInDegrees * Mathf.Deg2Rad) * rb.inertia;
        rb.AddTorque(impulse, ForceMode2D.Impulse);
    }
}
