using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshProFade : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        bool isFadeOut = true;
        while (true) { 
            float duration = 2f; //Fade out over 2 seconds.
            float currentTime = 0f;
            if (isFadeOut)
            {
            while (currentTime < duration)
            {
                float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
                textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
                currentTime += Time.deltaTime;
                yield return null;
            }
            isFadeOut = false;
   
                continue;
        } else {
            while (currentTime < duration)
            {
                float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
                textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
                currentTime += Time.deltaTime;
                yield return null;
            }

            isFadeOut = true;
 
                continue;
        }
        }
    }


}
