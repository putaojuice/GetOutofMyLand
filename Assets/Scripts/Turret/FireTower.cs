using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Turret
{
    [SerializeField] public float ActualTowerRange;

    // Start is called before the first frame update
    void Start()
    {   
        type = TurretType.Fire;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        range = ActualTowerRange + UpgradeManager.instance.data.range;
        firingRate = 1f;
        rangeDetector.GetComponent<RangeDetector>().UpdateColliderRadius(range);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        towerLevel = 1;
        SetUpRange(range);
    }

    void SetUpRange(float radius)
    {
        float Theta = 0f;
        int Size = (int)((1f / 0.01f) + 1f);
        rangeIndicator.positionCount = Size;
        for (int i = 0; i < Size; i++) {
            Theta += (2.0f * Mathf.PI * 0.01f);
            float x = radius * Mathf.Cos(Theta);
            float y = radius * Mathf.Sin(Theta);
            rangeIndicator.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {   
        
        HandleSelection();

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


    private void HandleSelection() {
        if (highlighted) {
            rangeIndicator.gameObject.SetActive(true);
        } else {
            rangeIndicator.gameObject.SetActive(false);
        }
    }

    public override float GetDamage()
    {
        return bulletPrefab.GetComponent<FireBall>().GetDamage();
    }

    public override void UpgradeTower()
    {
        if (towerLevel < 3) {
            towerLevel++;
            bulletPrefab.GetComponent<Bullet>().UpgradeTower();
        }
    }

    public override float GetLevel()
    {
        return towerLevel;
    }

    public override TurretType GetTurretType()
    {
        return type;
    }



}
