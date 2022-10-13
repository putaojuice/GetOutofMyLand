using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEffectable
{
    private GameManager gameManager;

    public AudioSource AudioSource;
    public float volume=0.5f;
    [Header("Enemy Data")]
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

    [SerializeField] public Material destructionMat;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0.0f){
            EnemyDestroy();
            UpdateNumOfEnemies();
            return;
        }

        if(_statusData != null){
            UpdateStatusEffects();
        }

        statusCoolDown -= Time.deltaTime;
    }

    IEnumerator PlayDissolve(float duration) 
    {
        float timeElapsed = 0f;

        // Use either SkinnedMeshRenderer or MeshRenderer depending on the renderer component used in the enemy game object
        // Most animated enemy assets uses SkinnedMeshRenderer while Unity has preset game objects set to MeshRenderer so take note
        gameObject.GetComponent<SkinnedMeshRenderer>().material = destructionMat;
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        while (timeElapsed <= duration)
        {
            timeElapsed += Time.deltaTime;
            // enemyObject.GetComponent<MeshRenderer>().material.SetFloat("_tConstant", Mathf.Lerp(1f, 0f, timeElapsed / duration));
            gameObject.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_tConstant", Mathf.Lerp(1f, 0f, timeElapsed / duration));
            yield return new WaitForEndOfFrame();
        }
    }

    private void EnemyDestroy()
	{
        // Stop enemy movement
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        if (gameObject != null) {
            // Play enemy dissolve animation from dissolve shader

            StartCoroutine(PlayDissolve(1f));
            AudioSource.Play();
            // Destroy after 1 sec delay
            Destroy(gameObject, 1.0f);

        }
	}

    // Creating method for healers to call
    public void GetHealed(float healPoints){
        gameObject.transform.Find("HealingEffect").gameObject.SetActive(true);
        gameObject.transform.Find("HealingEffect").GetComponent<ParticleSystem>().Play();
        if(hp < maxHp){
            hp += healPoints;
        }

    }

    public virtual void GetDamaged(float damage)
    {
        if(damage - defence > 0f){
            hp -= damage;
        }

    }

    public virtual void GetStatusDamaged(float damage)
    {
            hp -= damage;

    }

    public void UpdateNumOfEnemies() {

        GameManager.instance.UpdateWaveInfo();
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

        gameObject.transform.Find("ShockEffect").gameObject.SetActive(false);

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
        gameObject.transform.Find("PlasmaEffect").gameObject.SetActive(true);
        gameObject.transform.Find("PlasmaEffect").GetComponent<ParticleSystem>().Play();
    }

    public void HandleExplosionStatus(StatusData burnStatusData){
        Debug.Log("EXPLOSION!!");
        float damage = (burnStatusData.statusDuration/burnStatusData.DOTInterval) * (burnStatusData.DOTPoints);
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

        gameObject.transform.Find("ExplosionEffect").gameObject.SetActive(true);
        gameObject.transform.Find("ExplosionEffect").GetComponent<ParticleSystem>().Play();

    }

    public void HandleFreezeStatus(){
        Debug.Log("FREEZE!!");
        ApplyNewStatus(freezeEffectData);
    }

    public void HandleShockStatus(){
        Debug.Log("SHOCK!!");
        ApplyNewStatus(shockEffectData);
        gameObject.transform.Find("ShockEffect").gameObject.SetActive(true);
        gameObject.transform.Find("ShockEffect").GetComponent<ParticleSystem>().Play();
    }

    public void HandleStormStatus(){
        Debug.Log("STORM!!");
    }

    public void HandleSteamStatus(){
        Debug.Log("STEAM!!");
    }
}
