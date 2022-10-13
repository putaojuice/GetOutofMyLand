using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControlScript : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ambient;
    [SerializeField] private AudioClip funky;
    [SerializeField] private AudioSource CountDownSource;


    void Start() 
    {
        PlayAmbient();
    }


    public void PlayAmbient() {
        audioSource.clip = ambient;
        audioSource.Play();
    }

    public void PlayCountDown()
    {
        CountDownSource.Play();
    }

    public void PlayFunky()
    {
        audioSource.clip = funky;
        audioSource.Play();
    }

    public void fadeAudio()
    {
        StartCoroutine(StartFade(audioSource, 3f, 0));
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float originalVolume = audioSource.volume;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        audioSource.volume = originalVolume;
        yield break;
    }

}
