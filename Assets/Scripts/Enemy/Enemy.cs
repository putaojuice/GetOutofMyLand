using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEffectable
{
    [Header("Enemy Data")]
    public static WaveSpawning WaveSpawning;
    public float maxHp = 3f;
    public float hp = 3f;
    public float skillCoolDown = 5f;
    public float defence = 0f;
    public float currRound = 1f;

    [Header("Status Data")]
    public StatusData _statusData;
    public float currentStatusDuration = 0.0f;
    public float lastDOTInterval = 0.0f;
    public float statusCoolDown = 0.0f;
    public float originalSpeed;
    [SerializeField] public StatusData freezeEffectData;
    [SerializeField] public StatusData shockEffectData;

    // Start is called before the first frame update
    void Start()
    {
        WaveSpawning = GameObject.Find("GameManager").GetComponent<WaveSpawning>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0.0f){
            Destroy(gameObject);
            UpdateNumOfEnemies();
            return;
        }

        if(_statusData != null){
            UpdateStatusEffects();
        }

        statusCoolDown -= Time.deltaTime;
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

    public virtual void GetStatusDamaged(float damage)
    {
            hp -= damage;

    }

    public void UpdateNumOfEnemies() {
        WaveSpawning.EnemyDied();
    }

    public virtual void UseSkill() {
        return;
    }

    public void ApplyStatus (StatusData _statusData){
        
        if(statusCoolDown <= 0.0f){
            if(this._statusData == null){
                this._statusData = _statusData;
            } else {
                
                HandleStatus(_statusData);
            }
        }
        
        
    }

    public void UndoStatus () {
        currentStatusDuration = 0.0f;
        lastDOTInterval = 0.0f;
        statusCoolDown = 2.0f;
        
        //Used to reset the speed after slow / freeze debuff
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = originalSpeed;

        this._statusData = null;
    }

    public void ApplyNewStatus(StatusData newStatusData){
        currentStatusDuration = 0.0f;
        lastDOTInterval = 0.0f;
        this._statusData = newStatusData;
    }

    public void UpdateStatusEffects (){
        currentStatusDuration += Time.deltaTime;

        if(currentStatusDuration > _statusData.statusDuration) UndoStatus();

        if(_statusData == null) return;

        if(_statusData.DOTPoints != 0.0f && currentStatusDuration > lastDOTInterval)
        {
            GetStatusDamaged(_statusData.DOTPoints);
            lastDOTInterval += _statusData.DOTInterval;
        }

        if(_statusData.slowDebuff != 0.0f){
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.speed = originalSpeed / _statusData.slowDebuff;
        }

        if(_statusData.isFreeze){
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.speed = 0.0f;
        }
    }

    public void HandleStatus(StatusData newStatusData) {
        switch(newStatusData.StatusType) {
            case "FireEffect":
                if(_statusData.StatusType == "LightningEffect"){ // inflict Explosion
                    HandlePlasmaStatus(newStatusData);
                }

                else if(_statusData.StatusType == "WaterEffect"){
                    HandleSteamStatus();
                }

                else if(_statusData.StatusType == "WindEffect"){
                    HandleExplosionStatus(newStatusData);
                }

                break;

            case "LightningEffect":
                if(_statusData.StatusType == "WaterEffect"){
                    HandleShockStatus();
                }

                else if(_statusData.StatusType == "WindEffect"){
                    HandleStormStatus();
                }

                else if(_statusData.StatusType == "FireEffect"){ // inflict Explosion
                    HandlePlasmaStatus(_statusData);
                }
                
                break;

            case "WaterEffect":

                if(_statusData.StatusType == "WindEffect"){
                    HandleFreezeStatus();
                }

                else if(_statusData.StatusType == "FireEffect"){
                    HandleSteamStatus();
                }

                else if(_statusData.StatusType == "LightningEffect"){ // inflict Explosion
                    HandleShockStatus(); 
                }

                break;

            case "WindEffect":
                if(_statusData.StatusType == "FireEffect"){
                    HandleExplosionStatus(_statusData);
                }

                else if(_statusData.StatusType == "LightningEffect"){ // inflict Explosion
                    HandleStormStatus(); 
                }

                else if(_statusData.StatusType == "WaterEffect"){
                    HandleFreezeStatus();
                }

                break;

            default:
                break;

        }


    }

    public void HandlePlasmaStatus(StatusData burnStatusData){  //requires the burnstatusdata to calculate the damage points
        Debug.Log("PLASMA!");
        float damage = (burnStatusData.statusDuration/burnStatusData.DOTInterval) * burnStatusData.DOTPoints;
        GetStatusDamaged(damage);
        UndoStatus();
    }

    public void HandleExplosionStatus(StatusData burnStatusData){
        Debug.Log("EXPLOSION!!");
        float damage = (burnStatusData.statusDuration/burnStatusData.DOTInterval) * (burnStatusData.DOTPoints - 1.0f);
        GetStatusDamaged(damage);
        UndoStatus();

        float explosionAOE = 5.0f;
        var enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(var enemy in enemies) {
            // checks if enemy is not itself and if enemy is within explosionAOE
            if(transform.position != enemy.transform.position && (transform.position - enemy.transform.position).magnitude < explosionAOE){
                enemy.GetStatusDamaged(damage);
            }
        }

    }

    public void HandleFreezeStatus(){
        Debug.Log("FREEZE!!");
        ApplyNewStatus(freezeEffectData);
    }

    public void HandleShockStatus(){
        Debug.Log("SHOCK!!");
        ApplyNewStatus(shockEffectData);
    }

    public void HandleStormStatus(){
        Debug.Log("STORM!!");
    }

    public void HandleSteamStatus(){
        Debug.Log("STEAM!!");
    }
}
