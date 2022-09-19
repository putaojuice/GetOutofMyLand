using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect")]
public class StatusData : ScriptableObject
{
    [SerializeField] public float DOTPoints;
    [SerializeField] public float statusDuration;
    [SerializeField] public float DOTInterval;
    [SerializeField] public string StatusType;
}
