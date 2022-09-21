using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTower : Turret
{

    [SerializeField] public Vector3 shootDirection;

    // Start is called before the first frame update
    void Start()
    {
        range = 30.0f;
        firingRate = 0.5f;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public override void UpdateTarget()
    {
        // var enemies = GameObject.FindObjectsOfType<Enemy>();
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        Vector3 finalDirection = Vector3.zero;

        foreach(GameObject enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            
            Vector3 direction = (transform.position - enemy.transform.position).normalized;

            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
                finalDirection = direction;
                
            }

            Enemy currEnemy = enemy.GetComponent<Enemy>();
            
            if(currEnemy is Tank && distance < range){
                Tank tank = (Tank) currEnemy;
                if(tank.skillToggled){
                    shortestDistance = distance;
                    nearestEnemy = enemy;
                    finalDirection = direction;
                    break;
                }
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            shootDirection = finalDirection;
        } else {
            shootDirection = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(shootDirection == Vector3.zero)
        {
            return;
        }

        Vector3 dir = shootDirection;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotatingPart.rotation, lookRotation, Time.deltaTime * lerpSpeed).eulerAngles;
        rotatingPart.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f/ firingRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    public override void Shoot()
    {
        Debug.Log("Shooting!");
        Vector3 offset = new Vector3(0,5,0);
        Vector3 spawnPosition = firePoint.position - offset;
        GameObject currBullet = (GameObject) Instantiate(bulletPrefab, spawnPosition, firePoint.rotation);
        WindBullet bullet = currBullet.GetComponent<WindBullet>();
        
        if(bullet != null)
        {
            bullet.Seek(shootDirection, range);
        }
    }
}

