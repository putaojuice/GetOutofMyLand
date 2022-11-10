using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterChargeBar : MonoBehaviour
{
    [SerializeField] private float maxCharge;
    [SerializeField] private float chargePoints;
    [SerializeField] private Eater eater;
    [SerializeField] private RectTransform rect;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public Canvas mainCanvas;
    // Start is called before the first frame update
    void Start()
    {
        maxCharge = eater.maxChargePoints;
        chargePoints = eater.chargePoints;
        rect = gameObject.GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mainCanvas.transform.LookAt(transform.position + mainCamera.GetComponent<Camera>().transform.rotation * Vector3.forward, mainCamera.GetComponent<Camera>().transform.rotation * Vector3.up);
        chargePoints = eater.chargePoints;
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, chargePoints/maxCharge * 45);
    }
}
