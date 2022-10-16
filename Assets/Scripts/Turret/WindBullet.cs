using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBullet : Bullet
{

    [SerializeField] public Status _statusData;
    [SerializeField] public Vector3 origin;
    [SerializeField] public Vector3 direction;
    [SerializeField] public float range;

    public void Seek(Vector3 direction, float range)
    {
        this.direction = direction;
        this.range = range;
    }

    // Start is called before the first frame update
    void Start()
    {
        attackPoints = 4.0f;
        speed = 5.0f;
        origin = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(transform.position, origin);

        if(distance >= range)
        {
            Destroy(this.gameObject);
            return;
        }
        float distanceThisFrame = speed * Time.deltaTime;

        transform.Translate(-direction * distanceThisFrame, Space.World);
    }

   public void OnTriggerEnter(Collider collider)
   {
        Enemy currEnemy = collider.gameObject.GetComponent<Enemy>();
        if(currEnemy != null){
            currEnemy.GetDamaged(attackPoints);
            currEnemy.ApplyStatus(_statusData);
        }
        
   }



}
