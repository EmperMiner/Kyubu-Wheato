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
        randomNumberScript.bossDamage += randomNumberScript.bossDamage;
        randomNumberScript.agent.speed = 6;
        randomNumberScript.RollAttackDelay = 3f;
        randomNumberScript.inChargingState = true;
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
            randomNumberScript.SetBossStrength();
            randomNumberScript.agent.speed = 3;
            randomNumberScript.RollAttackDelay = 5f;
            hitWhileCharging = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        randomNumberScript.inChargingState = false;
        if (hitWhileCharging == false) { randomNumberScript.StartCoroutine(randomNumberScript.RemoveHeatRiserBuff()); }
        animator.ResetTrigger("Hurt");
    }
}
