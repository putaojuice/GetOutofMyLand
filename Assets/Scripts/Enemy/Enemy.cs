using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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

    [SerializeField] public Material destructionMat;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Physics.IgnoreLayerCollision(7,9, true);  // ignores all the enemies
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0.0f){
            EnemyDestroy();
            UpdateNumOfEnemies();
            return;
        }

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

}
