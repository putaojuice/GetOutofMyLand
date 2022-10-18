using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
/*    public GameObject objectToActivate;
    public GameObject objectToDeactivate;*/

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            /*            objectToActivate.SetActive(true);
                        this.objectToDeactivate.SetActive(false);*/
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("A key or mouse click has been detected");
        }
    }
}
