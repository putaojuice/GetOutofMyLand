using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static WaveSpawning WaveSpawning;
    public float maxHp = 3f;
    public float hp = 3f;
    public float skillCoolDown = 5f;
    public float defence = 0f;
    public float currRound = 1f;

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

    // Creating method for healers to call
    public void GetHealed(float healPoints){
        if(hp < maxHp){
            hp += healPoints;
        }
    }

    public virtual void GetDamaged(float damage)
    {
        if(damage - defence > 0){
            hp -= damage;
        }

    }

    public void UpdateNumOfEnemies() {
        WaveSpawning.EnemyDied();
    }

    public virtual void UseSkill() {
        return;
    }
}
