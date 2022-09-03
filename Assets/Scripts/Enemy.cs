using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float hp = 3f;

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

    public void GetDamaged(float damage)
    {
        hp -= damage;
    }


}