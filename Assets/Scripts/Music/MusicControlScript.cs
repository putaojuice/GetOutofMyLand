using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControlScript : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ambient;
    [SerializeField] private AudioClip funky;


    void Start() 
    {
        PlayAmbient();
    }


    public void PlayAmbient() {
        audioSource.clip = ambient;
        audioSource.Play();
    }

    public void PlayFunky()
    {
        audioSource.clip = funky;
        audioSource.Play();
    }

}
