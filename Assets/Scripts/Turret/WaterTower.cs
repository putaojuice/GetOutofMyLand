using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterTower : Turret
{

    [SerializeField] public StatusData rainStatusLevel1;
    [SerializeField] public StatusData rainStatusLevel2;
    [SerializeField] public StatusData rainStatusLevel3;
    [SerializeField] public float ActualTowerRange;

    private float permanentDamage = 0f;
    // Start is called before the first frame update
    void Start()
    {   
        type = TurretType.Water;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        range = ActualTowerRange + UpgradeManager.instance.data.range;
        firingRate = 0.2f;
        InvokeRepeating("UpdateTarget", 0f, 2.0f);
        towerLevel = 1;
        rangeDetector.GetComponent<RangeDetector>().UpdateColliderRadius(ActualTowerRange);
        if (UpgradeManager.instance != null) {
            permanentDamage = UpgradeManager.instance.data.damage;
        } 
        //SetUpRange(ActualTowerRange);
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

    public override void UpdateTarget()
    {
        var enemies = GameObject.FindObjectsOfType<EnemyStatus>();

        foreach(EnemyStatus enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            
            if(distance < range){
                switch (towerLevel){
                    case 1:
                        StatusData tempData1 = Instantiate(rainStatusLevel1);
                        tempData1.DOT += permanentDamage / 5f;
                        enemy.ApplyStatus(tempData1);
                        break;
                    case 2:
                        StatusData tempData2 = Instantiate(rainStatusLevel2);
                        tempData2.DOT += permanentDamage / 5f;
                        enemy.ApplyStatus(tempData2);
                        break;
                    default:
                        StatusData tempData3 = Instantiate(rainStatusLevel3);
                        tempData3.DOT += permanentDamage / 5f;
                        enemy.ApplyStatus(tempData3);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   
        HandleSelection();
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
        switch (towerLevel){
            case 1:
                return rainStatusLevel1.DOT + (permanentDamage / 5f);
            case 2:
                return rainStatusLevel2.DOT + (permanentDamage / 5f);
            case 3:
                return rainStatusLevel3.DOT + (permanentDamage / 5f);
            default:
                return 0;
        }
    }

    public override void UpgradeTower()
    {
        if (towerLevel < 3) {
            towerLevel++;
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
