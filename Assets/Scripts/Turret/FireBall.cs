using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Bullet
{

    [SerializeField] public float burnDamage = 1.0f;
    // [SerializeField] public StatusData _statusData;
    [SerializeField] public StatusData fireStatusLevel1;
    [SerializeField] public StatusData fireStatusLevel2;
    [SerializeField] public StatusData fireStatusLevel3;



    // Start is called before the first frame update
    void Start()
    {
        speed = 5.0f;
        
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

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public override void HitTarget()
    {
        EnemyStatus currEnemyStatus = target.GetComponent<EnemyStatus>();
        Enemy currEnemy = target.GetComponent<Enemy>();
        switch(towerLevel){
            case 1:
                currEnemy.GetDamaged(fireStatusLevel1.damage * towerLevel);
                currEnemyStatus.ApplyStatus(fireStatusLevel1);
                break;
            case 2:
                currEnemy.GetDamaged(fireStatusLevel2.damage * towerLevel);
                currEnemyStatus.ApplyStatus(fireStatusLevel2);
                break;
            default:
                currEnemy.GetDamaged(fireStatusLevel3.damage * towerLevel);
                currEnemyStatus.ApplyStatus(fireStatusLevel3);
                break;
        }
        Destroy(this.gameObject);

    }

    public void OnCollisionEnter(Collision col)
    {
        // CollisionHandler.HandleCollision(gameObject, col);
        if(col.gameObject == target){
            EnemyStatus currEnemyStatus = target.GetComponent<EnemyStatus>();
            Enemy currEnemy = target.GetComponent<Enemy>();
            switch(towerLevel){
                case 1:
                    currEnemy.GetDamaged(fireStatusLevel1.damage * towerLevel);
                    currEnemyStatus.ApplyStatus(fireStatusLevel1);
                    break;
                case 2:
                    currEnemy.GetDamaged(fireStatusLevel2.damage * towerLevel);
                    currEnemyStatus.ApplyStatus(fireStatusLevel2);
                    break;
                default:
                    currEnemy.GetDamaged(fireStatusLevel3.damage * towerLevel);
                    currEnemyStatus.ApplyStatus(fireStatusLevel3);
                    break;
            }
            Destroy(this.gameObject);
        }

    }



}
