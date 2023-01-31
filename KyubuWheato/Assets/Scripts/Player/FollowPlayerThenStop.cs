using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerThenStop : MonoBehaviour
{
    [SerializeField] private float TimeTillStopFollow;
    private bool Follow;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(StopFollow());
    }

    void Update()
    {
        if (Follow) { this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -2); }
    }

    private IEnumerator StopFollow()
    {
        Follow = true;
        yield return new WaitForSeconds(TimeTillStopFollow);
        Follow = false;
        yield return null;
    }
}
