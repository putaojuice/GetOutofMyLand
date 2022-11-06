using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TurretType
{
    Fire,
    Water,
    Lightning
}

public class Turret : MonoBehaviour
{   
    [SerializeField] protected LayerMask layer;
    protected Camera cam;
    public bool highlighted = false;
    public TurretType type; 
    [SerializeField] public LineRenderer rangeIndicator;

    [Header("Attributes")]
    [SerializeField] public float lerpSpeed = 10f;
    [SerializeField] public float range = 15f;
    [SerializeField] public float firingRate = 2f;
    [SerializeField] public float fireCountdown = 0f;

    [Header("Parts")]
    [SerializeField] public GameObject target;
    [SerializeField] public string enemyTag = "Enemy";
    [SerializeField] public Transform rotatingPart;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;
    [SerializeField] public int towerLevel;
    [SerializeField] public GameObject rangeDetector;
    [SerializeField] protected AudioSource AudioSource;



    /*  For pause wave function
     *  [SerializeField]
     *  private GameObject canvas;
     */

    // Start is called before the first frame update
    void Start()
    {   
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        /* canvas = GameObject.Find("Canvas (1)"); */
        towerLevel = 1;
    }

    public virtual void UpdateTarget()
    {
        // var enemies = GameObject.FindObjectsOfType<Enemy>();
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
            Enemy currEnemy = enemy.GetComponent<Enemy>();
            if(currEnemy is Tank && distance < range){
                Tank tank = (Tank) currEnemy;
                if(tank.skillToggled){
                    shortestDistance = distance;
                    nearestEnemy = enemy;
                    break;
                }
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {

            target = nearestEnemy;
        } else {
            target = null;
        }
    }

    // Update is called once per frame
    private void Update()
    {   

        if(target == null)
        {
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotatingPart.rotation, lookRotation, Time.deltaTime * lerpSpeed).eulerAngles;
        rotatingPart.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(fireCountdown <= 0f) // && !canvas.GetComponent<PauseWaveButton>().WaveIsPaused
        {
            Shoot();
            fireCountdown = 1f/ firingRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    public void GetDestroyed(){
        Destroy(gameObject);
    }

    public virtual float GetDamage() 
    {
        return 0;
    }

    public virtual void UpgradeTower()
    {
        
    }


    public virtual float GetLevel()
    {
        return towerLevel;
    }

    public virtual void Shoot()
    {
    }

    public virtual TurretType GetTurretType()
    {
        return type;
    }


}
