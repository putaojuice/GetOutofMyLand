using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{   
    [SerializeField]
    public int Health;

    [SerializeField]
    public int maxHealth;
    [SerializeField]
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {   
        maxHealth += UpgradeManager.instance.data.health;
        Health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {   
        // Game end condition

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            Health--;
            if (Health < 1) {
                gameManager.GameOver();
                Destroy(gameObject);
            }
        }
    }

}
