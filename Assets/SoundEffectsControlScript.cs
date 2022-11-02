using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsControlScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip landBuildSound;
    [SerializeField] private AudioClip turretBuildSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayLandBuildingSound()
    {
        audioSource.clip = landBuildSound;
        audioSource.Play();
    }

    public void PlayTurretBuildingSound()
    {
        audioSource.clip = turretBuildSound;
        audioSource.Play();
    }
}
