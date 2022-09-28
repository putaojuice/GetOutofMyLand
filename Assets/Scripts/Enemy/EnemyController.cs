// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.UI;

// public class EnemyController : MonoBehaviour
// {
//     // SerializeField makes private variables visible in the inspector
//     [SerializeField] private NavMeshAgent enemy;
//     [SerializeField] private GameObject Base;
//     [SerializeField] private GameObject enemyObject;
//     [SerializeField] private Material destructionMat;
//     [SerializeField] private float enemyHP;
//     [SerializeField] private float hpModifier;
//     private float startHP;
//     private bool enemyDeath = false;

//     [SerializeField] private float enemyDamage = 1f;

//     [SerializeField] private Image healthBar;

//     // Start is called before the first frame update
//     void Start()
//     {
//         // Play spawn sound
//         GameObject.Find("Enemy Spawn").GetComponent<AudioSource>().Play();

//         // Initialise Base
//         Base = GameObject.Find("Base");

//         // Enemy will navigate to the Base
//         enemy.SetDestination(Base.transform.position);

//         // Initialise enemy HP based on which wave it is right now. Enemy HP increase by 10 every 5 rounds.
//         startHP = 15f + Mathf.Ceil(WaveSpawner.wave / 5) * 10f * hpModifier;
//         enemyHP = startHP;
//     }

//     IEnumerator PlayDissolve(float duration) 
//     {
//         float timeElapsed = 0f;

//         // Use either SkinnedMeshRenderer or MeshRenderer depending on the renderer component used in the enemy game object
//         // Most animated enemy assets uses SkinnedMeshRenderer while Unity has preset game objects set to MeshRenderer so take note
//         enemyObject.GetComponent<SkinnedMeshRenderer>().material = destructionMat;
//         // enemyObject.GetComponent<MeshRenderer>().material = destructionMat;
//         enemyObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

//         while (timeElapsed <= duration)
//         {
//             timeElapsed += Time.deltaTime;
//             // enemyObject.GetComponent<MeshRenderer>().material.SetFloat("_tConstant", Mathf.Lerp(1f, 0f, timeElapsed / duration));
//             enemyObject.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_tConstant", Mathf.Lerp(1f, 0f, timeElapsed / duration));
//             yield return new WaitForEndOfFrame();
//         }
//     }

//     public void Damaged(float damage)
//     {
//         if (enemyDeath == false) {
//             enemyHP -= damage;
//             healthBar.fillAmount = enemyHP / startHP;
//             HighScore.addDamageScore((int) damage);

//             if (enemyHP <= 0f)
//             {
//                 enemyDeath = true;
//                 HighScore.addDeathScore();
//                 EnemyDestroy();
//             }
//         }
//     }

//     public bool GetDeath()
// 	{
//         return enemyDeath;
// 	}

//     // Enemy self destruction method
// 	private void EnemyDestroy()
// 	{
//         // Stop enemy movement
//         gameObject.GetComponent<NavMeshAgent>().enabled = false;
//         WaveSpawner.numOfEnemyAlive--;
//         if (gameObject != null) {
//             // Play enemy dissolve animation from dissolve shader
//             StartCoroutine(PlayDissolve(1f));
//             GameObject.Find("Enemy Death").GetComponent<AudioSource>().Play();

//             // Destroy after 1 sec delay
//             Destroy(gameObject, 1.0f);
            
//             Debug.Log(WaveSpawner.numOfEnemyAlive);

//             if (WaveSpawner.numToSpawn == 0)
// 			{
//                 WaveSpawner.waveEnd = true;
// 			}
//         }
// 	}
//     void BaseHit()
//     {        
//         Base.GetComponent<BaseController>().Damaged(enemyDamage);
//         healthBar.fillAmount = 0f;
//     }

// 	// When enemy trigger the base's collider
//     private void OnTriggerEnter(Collider other)
// 	{
//         BaseHit();
//         Debug.Log("Enemy destroyed");
//         enemyDeath = true;
//         EnemyDestroy();
// 	}

// }
