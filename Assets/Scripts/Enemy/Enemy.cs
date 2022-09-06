using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static WaveSpawning WaveSpawning;
    private float hp = 3f;

    // Start is called before the first frame update
    void Start()
    {
        WaveSpawning = GameObject.Find("GameManager").GetComponent<WaveSpawning>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0){
            Destroy(gameObject);
            UpdateNumOfEnemies();
            return;
        }
    }

    public void GetDamaged(float damage)
    {
        hp -= damage;
    }

    private void UpdateNumOfEnemies() {
        WaveSpawning.EnemyDied();
    }
}
