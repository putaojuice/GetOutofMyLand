using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float hp = 3f;
    public Transform goal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0){
            Destroy(gameObject);
            return;
        }
    }

    void GetDamaged(float damage)
    {
        hp -= damage;
    }

    // Update is called once per frame
    void onCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Endpoint"){
            Destroy(gameObject);
            return;
        }
 
    }
}
