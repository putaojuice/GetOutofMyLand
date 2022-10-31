using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{   
    [SerializeField]
    public int Health;

    [SerializeField]
    public int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {   
        // Game end condition
        if (Health < 1) {
            GameManager.instance.GameOver();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            Health--;
        }
    }

}
