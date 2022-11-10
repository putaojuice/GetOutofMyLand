using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    [SerializeField] public CapsuleCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        
        collider.height = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateColliderRadius(float input) {
        this.collider.radius = input;
    }
    
    /*
    public void SpawnAt(Vector3 v3Input, Quaternion rotationInput, float range)
    {
        Debug.Log("range detector spawned with range: " + range);
        Instantiate(this.transform, v3Input, rotationInput);
        UpdateColliderRadius(range);
    }
    */
}
