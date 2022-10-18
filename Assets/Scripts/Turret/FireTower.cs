using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireTower : Turret
{
    [SerializeField] public float ActualTowerRange;
    private bool highlighted = false;

    // Start is called before the first frame update
    void Start()
    {   
        canvas = GameObject.FindGameObjectWithTag("canvas");
        statsPanel = canvas.transform.Find("StatsPanel").gameObject;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        range = ActualTowerRange;
        firingRate = 1f;
        rangeDetector.GetComponent<RangeDetector>().UpdateColliderRadius(ActualTowerRange);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        towerLevel = 1;
        SetUpRange(ActualTowerRange);
    }

    void SetUpRange(float radius)
    {
        float Theta = 0f;
        int Size = (int)((1f / 0.01f) + 1f);
        rangeIndicator.SetVertexCount(Size);
        for (int i = 0; i < Size; i++) {
            Theta += (2.0f * Mathf.PI * 0.01f);
            float x = radius * Mathf.Cos(Theta);
            float y = radius * Mathf.Sin(Theta);
            rangeIndicator.SetPosition(i, new Vector3(x, y, 0.8f));
        }
    }

    // Update is called once per frame
    void Update()
    {   
        
        if (Input.GetMouseButtonDown(0)) {  
            SelectingTurret();
        } else if (Input.GetMouseButtonDown(1)) {
            highlighted = false;
            
        }
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


    private void SelectingTurret()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);  
        RaycastHit hit;  
        if (Physics.Raycast(cam.transform.position, ray.direction, out hit, Mathf.Infinity, layer)) { 
             // Compare turret hit and the current turret
            if (GameObject.ReferenceEquals(hit.transform.gameObject, gameObject)) {
                highlighted = true;
            } else {
                highlighted = false;
            } 
        } else {
            highlighted = false;
        }
    }

    private void HandleSelection() {
        if (highlighted) {
            rangeIndicator.gameObject.SetActive(true);
            statsPanel.SetActive(true);
        } else {
            rangeIndicator.gameObject.SetActive(false);
            statsPanel.SetActive(false);
        }
    }

}
