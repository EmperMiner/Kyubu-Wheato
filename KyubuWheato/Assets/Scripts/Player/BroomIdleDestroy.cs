using UnityEngine;

public class BroomIdleDestroy : MonoBehaviour
{
    private PlayerController player;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (player.CurrentMode != 2) { Destroy(gameObject); }
    }
}
