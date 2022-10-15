using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerSummon : StateMachineBehaviour
{
    [SerializeField] private GameObject[] summonObject;
    private Transform player;
    private Rigidbody2D bossRB;
    private BossRandomAttackGenerator randomNumberScript;

    [SerializeField] private float[] HaybaleOffsetX;
    [SerializeField] private float[] HaybaleOffsetY;

    private int randAttack;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossRB = animator.GetComponent<Rigidbody2D>();
        randomNumberScript = animator.GetComponent<BossRandomAttackGenerator>();

        randAttack = Random.Range(0,6);
        Summon(randAttack);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void Summon(int attackIndex)
    {
        if (attackIndex == 0)
        {
            for (int i = 0; i < 10; i++)
            {
            Instantiate(summonObject[0], new Vector3(player.position.x + Random.Range(-6f, 6f), player.position.y + Random.Range(-6f, 6f), player.position.z), Quaternion.identity);
            }
        }
        if (attackIndex == 1)
        {
            randomNumberScript.TumbleweedSpawn();
        }
        if (attackIndex == 2)
        {
            for (int i = 0; i < HaybaleOffsetX.Length; i++)
            {
            Instantiate(summonObject[1], new Vector3(player.position.x + HaybaleOffsetX[i], player.position.y + HaybaleOffsetY[i], player.position.z), Quaternion.identity);
            }
        }
        if (attackIndex == 3)
        {
            float RandomAngleOffset = Random.Range(0,2)*45;
            for (int i = 0; i < 4; i++)
            {
                Instantiate(summonObject[2], bossRB.position, Quaternion.Euler(0f, 0f, i*90 + RandomAngleOffset), bossRB.transform);
            }
        }
        if (attackIndex == 4)
        {
            int numObjects = 10;
            int randomEnemyIndex = Random.Range(7,11);
 
       //     Vector3 center = player.position;
            for (int i = 0; i < numObjects; i++)
            {
      //          Vector3 pos = RandomCircle(center, 8f);
     //           Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos);
                Instantiate(summonObject[randomEnemyIndex], player.position, Quaternion.identity);
            }
/*
            Vector3 RandomCircle ( Vector3 center ,float radius)
            {
                float ang = Random.value * 360;
                Vector3 pos;
                pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
                pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
                pos.z = center.z;
                return pos;
            }*/
        }
        if (attackIndex == 5)
        {
            for (int i = 0; i < 2; i++)
            {
                Instantiate(summonObject[Random.Range(3,7)], new Vector3(player.position.x + Random.Range(-7f, 7f), player.position.y + Random.Range(-7f, 7f), player.position.z), Quaternion.identity);
            }
        }
    }
}
