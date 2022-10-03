using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControlScript : MonoBehaviour
{
    public static MusicControlScript instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ambient;
    [SerializeField] private AudioClip funky;


    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            PlayAmbient();
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(gameObject);
        }
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
