using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class testingLightning : MonoBehaviour
{

    [SerializeField] public GameObject lightning;
    [SerializeField] public GameObject endpoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ShootLightning", 5f, 2.0f);
    }

    void ShootLightning()
    {
        GameObject currBullet = (GameObject) Instantiate(lightning, transform.position, transform.rotation);
        Bullet bullet = currBullet.GetComponent<Bullet>();
        LightningBoltScript bulletPrefab = currBullet.GetComponent<LightningBoltScript>();
        bulletPrefab.StartPosition = transform.position;
        bulletPrefab.EndPosition = endpoint.transform.position;
        
        if(bullet != null)
        {
            bullet.Seek(endpoint);
        }
    }

    // Update is called once per frame
    void Update()
    {


    
    }
}
