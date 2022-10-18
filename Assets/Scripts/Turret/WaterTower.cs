using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class WaterTower : Turret
{

    [SerializeField] public StatusData rainStatusLevel1;
    [SerializeField] public StatusData rainStatusLevel2;
    [SerializeField] public StatusData rainStatusLevel3;
    [SerializeField] public float ActualTowerRange;

    private bool highlighted = false;

    // Start is called before the first frame update
    void Start()
    {   
        canvas = GameObject.FindGameObjectWithTag("canvas");
        statsPanel = canvas.transform.Find("StatsPanel").gameObject;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        range = ActualTowerRange;
        firingRate = 0.2f;
        InvokeRepeating("UpdateTarget", 0f, 2.0f);
        towerLevel = 3;
        rangeDetector.GetComponent<RangeDetector>().UpdateColliderRadius(ActualTowerRange);
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

    public override void UpdateTarget()
    {
        var enemies = GameObject.FindObjectsOfType<EnemyStatus>();

        foreach(EnemyStatus enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < range){
                switch (towerLevel){
                    case 1:
                        enemy.ApplyStatus(rainStatusLevel1);
                        break;
                    case 2:
                        enemy.ApplyStatus(rainStatusLevel2);
                        break;
                    default:
                        enemy.ApplyStatus(rainStatusLevel3);
                        break;
                }
            }
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
