using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterChargeBar : MonoBehaviour
{
    [SerializeField] private float maxCharge;
    [SerializeField] private float chargePoints;
    [SerializeField] private Eater eater;
    [SerializeField] private RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        maxCharge = eater.maxChargePoints;
        chargePoints = eater.chargePoints;
        rect = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        chargePoints = eater.chargePoints;
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, chargePoints/maxCharge * 90);
    }
}
