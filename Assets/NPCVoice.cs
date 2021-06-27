using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NPCVoice : MonoBehaviour
{
    public static bool talking;
    private AudioSource voice;
    public AudioClip[] voiceSamples;
    AudioClip sample;
    public float minPitch = 1f;
    public float maxPitch = 1.2f;

    void Start()
    {
        voice = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(talking)
        {
            if (voiceSamples.Length > 0 && !voice.isPlaying)
            {
                AudioClip newSample;
                do //prevents repeating clips
                {
                    newSample = voiceSamples[Random.Range(0, voiceSamples.Length)];
                }
                while (sample == newSample);

                sample = newSample;
                voice.clip = sample;
                voice.pitch = Random.Range(minPitch, maxPitch);
                voice.Play();
            }

        }
        else 
        {
            voice.Stop();
        }
    }
    public static void SetTalking(bool state) //called by Yarn DialougeUI to start/end with lines of dialouge.
    {
        talking = state;
    }
}
