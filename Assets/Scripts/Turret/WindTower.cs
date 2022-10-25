using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTower : Turret
{

    [SerializeField] public Vector3 shootDirection;
    [SerializeField] public float ActualTowerRange;

    // Start is called before the first frame update
    void Start()
    {
        range = ActualTowerRange;
        firingRate = 0.5f;
        rangeDetector.GetComponent<RangeDetector>().UpdateColliderRadius(ActualTowerRange);
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
    private void Update()
    {   
        if (Input.GetMouseButtonDown(0)) {  
            SelectingTurret();
        } 

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
        Vector3 spawnPosition = firePoint.position;
        GameObject currBullet = (GameObject) Instantiate(bulletPrefab, spawnPosition, firePoint.rotation);
        WindBullet bullet = currBullet.GetComponent<WindBullet>();
        
        if(bullet != null)
        {
            bullet.Seek(shootDirection, range);
        }
    }

    private void SelectingTurret()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);  
        RaycastHit hit;  
        if (Physics.Raycast(cam.transform.position, ray.direction, out hit, Mathf.Infinity, layer)) {  
            //Select stage    
            if (hit.transform.gameObject.tag == "Tower") {  

            }  
        } 
    }

    
}

