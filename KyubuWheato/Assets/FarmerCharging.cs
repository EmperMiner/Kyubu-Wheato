using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerCharging : StateMachineBehaviour
{
    private BossRandomAttackGenerator randomNumberScript;
    private bool hitWhileCharging;
    [SerializeField] private GameObject HeatRiserPrefab;
    private Rigidbody2D bossRB;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossRB = animator.GetComponent<Rigidbody2D>();
        randomNumberScript = animator.GetComponent<BossRandomAttackGenerator>();
        randomNumberScript.bossDamage = 20;
        randomNumberScript.agent.speed = 4;
        randomNumberScript.RollAttackDelay = 3f;
        hitWhileCharging = false;
        Instantiate(HeatRiserPrefab, bossRB.position, Quaternion.identity, bossRB.transform);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hitWhileCharging) { animator.SetTrigger("Hurt"); }
        if (randomNumberScript.hit) 
        {
            randomNumberScript.hit = false;
            GameObject HeatRiserOnMap = GameObject.FindGameObjectWithTag("HeatRiser");
            Destroy(HeatRiserOnMap);
            randomNumberScript.bossDamage = 50;
            randomNumberScript.agent.speed = 2;
            randomNumberScript.RollAttackDelay = 6f;
            hitWhileCharging = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hitWhileCharging == false) { randomNumberScript.StartCoroutine(randomNumberScript.RemoveHeatRiserBuff()); }
        animator.ResetTrigger("Hurt");
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        
    }
}
