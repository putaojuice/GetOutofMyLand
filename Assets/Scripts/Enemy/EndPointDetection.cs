using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointDetection : MonoBehaviour
{
    [SerializeField] public GameObject itself;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "Endpoint")
        {
            Destroy(itself);
        }
    }
}
