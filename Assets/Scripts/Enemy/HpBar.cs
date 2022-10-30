using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private Enemy enemy;
    [SerializeField] private RectTransform rect;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public Canvas mainCanvas;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = enemy.maxHp;
        hp = enemy.hp;
        rect = gameObject.GetComponent<RectTransform>();
        mainCamera = Camera.main;
        // if(rect.TryGetComponent<FaceCamera>(out FaceCamera faceCamera)){
        //     faceCamera.Camera = mainCamera;
        // }


    }

    // Update is called once per frame
    void Update()
    {
        mainCanvas.transform.LookAt(transform.position + mainCamera.GetComponent<Camera>().transform.rotation * Vector3.forward, mainCamera.GetComponent<Camera>().transform.rotation * Vector3.up);
        hp = enemy.hp;
        maxHp = enemy.maxHp;
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, hp/maxHp * 45);
    }
}
