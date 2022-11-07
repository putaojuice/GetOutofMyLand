using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicThemeControl : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        // Create a temporary reference to the current scene.
        
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
    }

    void Update()
    {
        // Retrieve the name of this scene.
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "WorkingDynamicSpawnPrototype") {
            Object.Destroy(this.transform.gameObject);
        }
    }
}

    

