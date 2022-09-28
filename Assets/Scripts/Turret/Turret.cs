using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Attributes")]

    private float lerpSpeed = 10f;
    public float range = 15f;
    public float firingRate = 2f;
    private float fireCountdown = 0f;

    [Header("Parts")]
    private GameObject target;
    public string enemyTag = "Enemy";
    public Transform rotatingPart;
    public GameObject bulletPrefab;
    public Transform firePoint;

/*  For pause wave function
 *  [SerializeField]
 *  private GameObject canvas;
 */

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        /* canvas = GameObject.Find("Canvas (1)"); */
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;

            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy;
        } else {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotatingPart.rotation, lookRotation, Time.deltaTime * lerpSpeed).eulerAngles;
        rotatingPart.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(fireCountdown <= 0f) // && !canvas.GetComponent<PauseWaveButton>().WaveIsPaused
        {
            Shoot();
            fireCountdown = 1f/ firingRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject currBullet = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = currBullet.GetComponent<Bullet>();
        
        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void onDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
