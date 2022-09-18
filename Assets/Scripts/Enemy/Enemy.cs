using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEffectable
{
    public static WaveSpawning WaveSpawning;
    public float maxHp = 3f;
    public float hp = 3f;
    public float skillCoolDown = 5f;
    public float defence = 0f;
    public float currRound = 1f;

    public StatusData _statusData;
    public float currentStatusDuration = 0.0f;
    public float lastDOTInterval = 0.0f;

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

        if(_statusData != null){
            UpdateStatusEffects();
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


    public void ApplyStatus (StatusData _statusData){
        Debug.Log("BURN BITCH");
        this._statusData = _statusData;
    }

    public void UndoStatus () {
        currentStatusDuration = 0.0f;
        lastDOTInterval = 0.0f;
        this._statusData = null;
    }

    public void UpdateStatusEffects (){
        currentStatusDuration += Time.deltaTime;


        if(currentStatusDuration > _statusData.statusDuration) UndoStatus();

        if(_statusData == null) return;

        else if(_statusData.DOTPoints != 0.0f && currentStatusDuration > lastDOTInterval)
        {
            hp -= _statusData.DOTPoints;
            lastDOTInterval += _statusData.DOTInterval;
        }

    }
}
