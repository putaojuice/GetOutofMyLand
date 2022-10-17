using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatus : MonoBehaviour, IEffectable
{

    [SerializeField] public Enemy myEnemyObject;

    [Header("Status Data")]
    public float currentStatusDuration = 0.0f;
    public float lastDOTInterval = 0.0f;
    public float statusCoolDown = 0.0f;
    public float originalSpeed;

    // Used to store current status info
    public string currentStatus = "";
    public float DOT = 0.0f;
    public float DOTInterval = 0.0f;
    public float statusDuration = 0.0f;
    public float baseDamage = 0.0f;
    public float slowDebuff = 0.0f;
    public float towerLevel = 0.0f;


    void Start()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        originalSpeed = agent.speed;
    }

    void Update() 
    {

        if(currentStatus != ""){
            UpdateStatusEffects();
        }

        statusCoolDown -= Time.deltaTime;
    }

   public void ApplyStatus (StatusData _statusData){
        Debug.Log("Calling ApplyStatus");
        Debug.Log(myEnemyObject);
        if(statusCoolDown <= 0.0f){
            if(this.currentStatus == ""){ // fit in the current set of data first 
                this.currentStatus = _statusData.statusType;
                this.DOT = _statusData.DOT;
                this.DOTInterval = _statusData.DOTInterval;
                this.statusDuration = _statusData.statusDuration;
                this.baseDamage = _statusData.damage;
                this.slowDebuff = _statusData.slowDebuff;
                this.towerLevel = _statusData.towerLevel; 
            } else {
                
                HandleStatus(_statusData);
            }
        }
    }

    public void UndoStatus () {

        // removing the current set totally
        currentStatus = "";
        DOT = 0.0f;
        DOTInterval = 0.0f;
        statusDuration = 0.0f;
        baseDamage = 0.0f;
        slowDebuff = 0.0f;
        towerLevel = 0.0f; 

        currentStatusDuration = 0.0f;
        lastDOTInterval = 0.0f;
        statusCoolDown = 2.0f;
        
        //Used to reset the speed after slow / freeze debuff
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = originalSpeed;
        gameObject.transform.Find("ShockEffect").gameObject.SetActive(false);
        gameObject.transform.Find("BurnEffect").gameObject.SetActive(false);

        
    }

    // replace old status with updated status
    public void ApplyNewStatus(string _newStatus, float _DOT, float _DOTInterval, float _statusDuration, float _damage, float _slowDebuff, float _towerLevel){  
        lastDOTInterval = 0.0f;
        currentStatusDuration = 0.0f;
        this.currentStatus = _newStatus;
        this.DOT = _DOT;
        this.DOTInterval = _DOTInterval;
        this.statusDuration = _statusDuration;
        this.baseDamage = _damage;
        this.slowDebuff = _slowDebuff;
        this.towerLevel = _towerLevel; 
    }


    public void UpdateStatusEffects (){
        currentStatusDuration += Time.deltaTime;

        if(currentStatusDuration > statusDuration) UndoStatus();

        if(currentStatus == "") return;

        if(DOT > 0.0f && currentStatusDuration > lastDOTInterval)
        {
            // Debug.Log(myEnemyObject);
            myEnemyObject.GetStatusDamaged(DOT);
            lastDOTInterval += DOTInterval;
        }

        if(slowDebuff != 0.0f){
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.speed = originalSpeed * slowDebuff;
        }

    }

    public void HandleStatus(StatusData newStatusData) {
        switch(newStatusData.statusType) {
            case "FireEffect":
                if(currentStatus == "LightningEffect"){ // inflict Explosion
                    HandlePlasmaStatusIfCurrentLightning(newStatusData);
                }
                else if(currentStatus == "WaterEffect"){
                    // HandleExplosionStatusIfCurrentFire(newStatusData);
                }
                break;

            case "LightningEffect":
                if(currentStatus == "WaterEffect"){
                    HandleShockStatusIfCurrentWater(newStatusData);
                }
                else if(currentStatus == "FireEffect"){ // inflict Explosion
                    HandlePlasmaStatusIfCurrentFire(newStatusData);
                }
                break;
            case "WaterEffect":

                if(currentStatus == "FireEffect"){
                    // HandleExplosionStatusIfCurrentWater(newStatusData);
                }
                else if(currentStatus == "LightningEffect"){ // inflict Explosion
                    HandleShockStatusIfCurrentLightning(newStatusData); 
                }
                break;
            default:
                break;
        }
    }

    public void HandlePlasmaStatusIfCurrentLightning (StatusData fireStatusData)
    {  //requires the fireStatusData to calculate the damage points
        Debug.Log("PLASMA!");
        float plasmaDamage = fireStatusData.damage + baseDamage;
        myEnemyObject.GetStatusDamaged(plasmaDamage);
        UndoStatus();
        gameObject.transform.Find("PlasmaEffect").gameObject.SetActive(true);
        gameObject.transform.Find("PlasmaEffect").GetComponent<ParticleSystem>().Play();
    }

    public void HandlePlasmaStatusIfCurrentFire (StatusData lightningStatusData)
    {
        Debug.Log("PLASMA!");
        float plasmaDamage = lightningStatusData.damage + baseDamage;
        myEnemyObject.GetStatusDamaged(plasmaDamage);
        UndoStatus();
        gameObject.transform.Find("PlasmaEffect").gameObject.SetActive(true);
        gameObject.transform.Find("PlasmaEffect").GetComponent<ParticleSystem>().Play();
    }

    // public void HandleExplosionStatus(){
    //     Debug.Log("EXPLOSION!!");
    //     float damage = 5.0f;
    //     myEnemyObject.GetStatusDamaged(damage);
    //     UndoStatus();
    //     ApplyNewStatus(burnStatus);
    //     gameObject.transform.Find("BurnEffect").gameObject.SetActive(true);
    //     gameObject.transform.Find("BurnEffect").GetComponent<ParticleSystem>().Play();

    //     float explosionAOE = 5.0f;
    //     var enemies = GameObject.FindObjectsOfType<Enemy>();
    //     foreach(var enemy in enemies) {
    //         // checks if enemy is not itself and if enemy is within explosionAOE
    //         if(transform.position != enemy.transform.position && (transform.position - enemy.transform.position).magnitude < explosionAOE){
    //             enemy.GetStatusDamaged(damage);
    //             enemy.ApplyNewStatus(burnStatus);
    //             enemy.gameObject.transform.Find("BurnEffect").gameObject.SetActive(true);
    //             enemy.gameObject.transform.Find("BurnEffect").GetComponent<ParticleSystem>().Play();
    //         }
    //     }

    //     gameObject.transform.Find("ExplosionEffect").gameObject.SetActive(true);
    //     gameObject.transform.Find("ExplosionEffect").GetComponent<ParticleSystem>().Play();

    // }

    //string newStatus, float DOT, float DOTInterval, float statusDuration, float damage, float slowDebuff, float towerLevel
    public void HandleShockStatusIfCurrentLightning(StatusData waterStatusData){
        Debug.Log("SHOCK!!");

        ApplyNewStatus("ShockEffect", 0.0f, 0.0f, waterStatusData.statusDuration, 0.0f, 1.0f/baseDamage, towerLevel);

        gameObject.transform.Find("ShockEffect").gameObject.SetActive(true);
        gameObject.transform.Find("ShockEffect").GetComponent<ParticleSystem>().Play();
    }

    public void HandleShockStatusIfCurrentWater(StatusData lightningStatusData){
        Debug.Log("SHOCK!!");

        ApplyNewStatus("ShockEffect", 0.0f, 0.0f, statusDuration, 0.0f, 1.0f/lightningStatusData.damage, towerLevel);

        gameObject.transform.Find("ShockEffect").gameObject.SetActive(true);
        gameObject.transform.Find("ShockEffect").GetComponent<ParticleSystem>().Play();
    }
}