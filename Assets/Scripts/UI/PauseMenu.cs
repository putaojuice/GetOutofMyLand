using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField]
    private PauseWaveButton PauseWaveButton;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTogglePause()
    {
        if (GameIsPaused) { Resume(); }
        else { Pause(); }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;

        if (!PauseWaveButton.WaveIsPaused)
        {
            Time.timeScale = 1f;
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
