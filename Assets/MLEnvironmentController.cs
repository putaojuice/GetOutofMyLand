using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLEnvironmentController : MonoBehaviour
{
    Transform MLPLatform;
    List<Transform> tileList = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            MLPLatform = child;
        }
        foreach (Transform child in MLPLatform.transform)
        {
            tileList.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
