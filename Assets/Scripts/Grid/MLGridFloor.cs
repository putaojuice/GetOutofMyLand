using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class MLGridFloor : MonoBehaviour
{   
    
    
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject spawnPoint;
    private GridController controller;
    
    [Header("Grid Parameters")]
    [SerializeField] private int blockSize;

    public bool isOuterFloor = false;
    public bool isTurretFloor = false;
    
    private MeshRenderer rend;
    private bool selected = false;
    private Color originalColor;
    
    // Start is called before the first frame update
    void Start()
    {   
        rend = GetComponent<MeshRenderer>();
        originalColor = rend.material.color;
        GameObject environment = GameObject.Find("EnvironmentManager");
        controller = environment.GetComponent<GridController>();
    }

}
