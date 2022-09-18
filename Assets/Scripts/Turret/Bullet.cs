using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public Enemy target;
    [SerializeField] public float speed = 300f;
    [SerializeField] public float attackPoints = 1f;

    public void Seek(Enemy _target)
    {
        target = _target;
    }
    // Start is called before the first frame update
    void Start()
    {
        
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

    public virtual void HitTarget()
    {
        Enemy currEnemy = target.GetComponent<Enemy>();
        currEnemy.GetDamaged(attackPoints);
        Destroy(this.gameObject);

    }



}
