using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGoldenWheat : MonoBehaviour

{   
    [SerializeField] private GameObject GoldenWheat;

    public void ThrowGoldenWheat()
    {
        Instantiate(GoldenWheat, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().PlaySound("GoldenWheatSpawn");
    }

    public void PlayFarmerLimp() { FindObjectOfType<AudioManager>().PlaySound("FarmerLimp"); }
    
}
