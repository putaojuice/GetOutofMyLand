using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject objectToDeactivate;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            objectToActivate.SetActive(true);
            this.objectToDeactivate.SetActive(false);
            Debug.Log("A key or mouse click has been detected");
        }
    }
}
