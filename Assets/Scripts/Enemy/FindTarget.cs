using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class FindTarget : MonoBehaviour {

    [SerializeField] private GameObject healthBar;
    private Vector3 targetPos;
    private Camera gameCamera;

    // Use this for initialization
    void Start () {
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
 
    // Update is called once per frame
    void Update () {
        Vector3 targetPos = gameCamera.transform.position;
        targetPos.z = 0;
        healthBar.transform.LookAt(targetPos);
       
    }
}

