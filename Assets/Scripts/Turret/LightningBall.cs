using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Bullet
{

    [SerializeField] public float burnDamage = 1.0f;
    [SerializeField] public StatusData _statusData;

    // Start is called before the first frame update
    void Start()
    {
        attackPoints = 5.0f;
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

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public override void HitTarget()
    {
        Enemy currEnemy = target.GetComponent<Enemy>();
        currEnemy.GetDamaged(attackPoints * towerLevel);
        currEnemy.ApplyStatus(_statusData);
        Destroy(this.gameObject);

    }



}
