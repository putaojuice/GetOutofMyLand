using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseWaveButton : MonoBehaviour
{
    public bool WaveIsPaused = false;

    [SerializeField]
    private Button Button;
    [SerializeField]
    private Sprite PauseWaveImage;
    [SerializeField]
    private Sprite ResumeWaveImage;

    void Start()
    {
        Button btn = Button.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
    
    void OnClick ()
    {
        if (WaveIsPaused)
        {
            Resume();
        } else
        {
            Pause();
        }
    }

    private void Resume()
    {
        Button.image.sprite = PauseWaveImage;
        WaveIsPaused = false;
    }

    private void Pause()
    {
        Button.image.sprite = ResumeWaveImage;
        WaveIsPaused = true;
    }
}
