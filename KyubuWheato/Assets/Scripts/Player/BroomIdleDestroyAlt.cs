using UnityEngine;

public class BroomIdleDestroyAlt : MonoBehaviour
{
    private PlayerController player;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (player.CurrentMode != 3) { Destroy(gameObject); }
    }
}
