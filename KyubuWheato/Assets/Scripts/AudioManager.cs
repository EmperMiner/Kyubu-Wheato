using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [SerializeField] private bool isMainMenu;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.group;
        }

        if (isMainMenu) { PlayJingle("MainMenu"); }
        else { StartCoroutine(PlayGameOST()); }
    }

    
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " Not Found");
            return;
        }
        s.source.PlayOneShot(s.clip);
    }
    public void PlayJingle(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " Not Found");
            return;
        }
        s.source.Play();
    }
    IEnumerator PlayGameOST()
    {
        Sound ErsteFirst = Array.Find(sounds, sound => sound.name == "ErsteFirst");
        ErsteFirst.source.Play();
        yield return new WaitForSeconds(8.64f);
        Sound ErsteLoop = Array.Find(sounds, sound => sound.name == "ErsteLoop");
        ErsteLoop.source.Play();
        yield return null;
    }
}
