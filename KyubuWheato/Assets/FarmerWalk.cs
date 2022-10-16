using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerWalk : StateMachineBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Transform player;
    private Rigidbody2D bossRB;
    private BossRandomAttackGenerator randomNumberScript;

    

    private bool StartedRollingAttack = false;
    private float attackRange = 8f;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossRB = animator.GetComponent<Rigidbody2D>();
        randomNumberScript = animator.GetComponent<BossRandomAttackGenerator>();

        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        randomNumberScript.RollAttack = true;
        randomNumberScript.AttackRolled = 0;
        if (StartedRollingAttack == false) { randomNumberScript.StartCoroutine(randomNumberScript.RollAttackFunc()); }
        StartedRollingAttack = true;

        animator.ResetTrigger("Charged");
        animator.ResetTrigger("Charging");
        animator.ResetTrigger("Slice");
        animator.ResetTrigger("Summon");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);

        if (randomNumberScript.AttackRolled == 1) { animator.SetTrigger("Charging"); }
        else if (randomNumberScript.AttackRolled <= 3 && randomNumberScript.AttackRolled >= 2 && Vector2.Distance(player.position, bossRB.position) <= attackRange) { animator.SetTrigger("Slice"); }
        else if (randomNumberScript.AttackRolled > 3) { animator.SetTrigger("Summon"); }
        else {}
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Charging");
        animator.ResetTrigger("Slice");
        animator.ResetTrigger("Summon");
        agent.isStopped = true;
        agent.ResetPath();
        randomNumberScript.RollAttack = false;
    }
}
