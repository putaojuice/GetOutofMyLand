using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Turret
{
    [SerializeField] public float ActualTowerRange;
    // Start is called before the first frame update
    void Start()
    {
        range = ActualTowerRange;
        firingRate = 1f;
        rangeDetector.GetComponent<RangeDetector>().UpdateColliderRadius(ActualTowerRange);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        towerLevel = 1;
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

        if(fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f/ firingRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    public override void Shoot()
    {
        GameObject currBullet = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = currBullet.GetComponent<Bullet>();
        bullet.towerLevel = towerLevel;
        
        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }

}
