using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Bullet
{

    [SerializeField] public float burnDamage = 1.0f;
    // [SerializeField] public StatusData _statusData;
    [SerializeField] public StatusData lightningStatusLevel1;
    [SerializeField] public StatusData lightningStatusLevel2;
    [SerializeField] public StatusData lightningStatusLevel3;

    // Start is called before the first frame update
    void Start()
    {
        speed = 30.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame + 5.0f)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public override void HitTarget()
    {
        Enemy currEnemy = target.GetComponent<Enemy>();
        EnemyStatus currEnemyStatus = target.GetComponent<EnemyStatus>();
        switch(towerLevel){
            case 1:
                currEnemy.GetDamaged(lightningStatusLevel1.damage * towerLevel);
                currEnemyStatus.ApplyStatus(lightningStatusLevel1);
                break;
            case 2:
                currEnemy.GetDamaged(lightningStatusLevel2.damage * towerLevel);
                currEnemyStatus.ApplyStatus(lightningStatusLevel2);
                break;
            default:
                currEnemy.GetDamaged(lightningStatusLevel3.damage * towerLevel);
                currEnemyStatus.ApplyStatus(lightningStatusLevel3);
                break;
        }

        Destroy(this.gameObject);

    }

    public override void UpgradeTower()
    {
        if (towerLevel < 3) {
            towerLevel++;
        }
    }

    public float GetDamage()
    {
        switch(towerLevel){
            case 1:
                return lightningStatusLevel1.damage;
            case 2:
                return lightningStatusLevel2.damage;
            case 3:
                return lightningStatusLevel3.damage;
            default:
                return lightningStatusLevel1.damage;
        }
    }

}
