using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsControlScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip landBuildSound;
    [SerializeField] private AudioClip turretBuildSound;
    [SerializeField] private AudioClip enemyDeathSound;

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

    public void PlayEnemyDeathSound()
    {
        Debug.Log("SOUND SHOULD BE PLAYING");
        audioSource.clip = enemyDeathSound;
        audioSource.Play();
    }
}
