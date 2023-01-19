using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayASound : MonoBehaviour
{
    private AudioManager AudioPlayer;
    [SerializeField] private string WantedAudioClip;
    void Start()
    {
        AudioPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        AudioPlayer.PlaySound(WantedAudioClip);
    }

}
