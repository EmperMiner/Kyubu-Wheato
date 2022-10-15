using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerScytheSwing : StateMachineBehaviour
{
    [SerializeField] private GameObject scytheSwingPrefab;
    private Transform player;
    private Rigidbody2D bossRB;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossRB = animator.GetComponent<Rigidbody2D>();

        Vector2 enemyTargetVector = new Vector2(player.position.x - bossRB.position.x, player.position.y - bossRB.position.y);
        float rotOffset = Vector2.Angle(Vector2.right, enemyTargetVector);
        Instantiate(scytheSwingPrefab, bossRB.position, Quaternion.Euler(0f, 0f, 360f - rotOffset));
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
