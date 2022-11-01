using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect")]
public class StatusData : ScriptableObject
{
    [SerializeField] public string statusType;
    [SerializeField] public float statusDuration;
    [SerializeField] public float DOT;
    [SerializeField] public float DOTInterval;
    [SerializeField] public float slowDebuff;
    [SerializeField] public float damage;
    [SerializeField] public float towerLevel;
    
    void OnEnable() {
        if (UpgradeManager.instance != null) {
            damage += UpgradeManager.instance.data.damage;
        } 
    }
}
