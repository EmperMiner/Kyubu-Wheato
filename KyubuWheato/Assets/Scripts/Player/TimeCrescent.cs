using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeCrescent : MonoBehaviour
{
 /*   [SerializeField] private DiceThrow diceThrowScript;
    private mouseBehaviour enemyScript;
    private FlippedBehaviour flippedEnemyScript; 
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "enemyMouse")
        {
            try 
            {
                enemyScript = other.gameObject.GetComponent<mouseBehaviour>();
                diceThrowScript.CrescentCrack(1);
                enemyScript.stopped = true;
            }
            catch (NullReferenceException) 
            {
                try 
                {
                    flippedEnemyScript = other.gameObject.GetComponent<FlippedBehaviour>();
                    diceThrowScript.CrescentCrack(100);
                    flippedEnemyScript.stopped = true;
                }
                catch (NullReferenceException) { Debug.Log("isBoss"); }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "enemyMouse")
        {
            try 
            {
                enemyScript = other.gameObject.GetComponent<mouseBehaviour>();
                enemyScript.stopped = false;
            }
            catch (NullReferenceException) 
            {
                try 
                {
                    flippedEnemyScript = other.gameObject.GetComponent<FlippedBehaviour>();
                    flippedEnemyScript.stopped = false;
                }
                catch (NullReferenceException) { Debug.Log("isBoss"); }
            }
        }

    } */
}
