using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHPBar : MonoBehaviour
{
    [SerializeField] public Image _bar;
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private BaseHealth baseHp;
    [SerializeField] private RectTransform rect;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public Canvas mainCanvas;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = baseHp.maxHealth;
        hp = baseHp.Health;
        rect = gameObject.GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mainCanvas.transform.LookAt(transform.position + mainCamera.GetComponent<Camera>().transform.rotation * Vector3.forward, mainCamera.GetComponent<Camera>().transform.rotation * Vector3.up);
        maxHp = baseHp.maxHealth;
        hp = baseHp.Health;
        float amount = (hp/maxHp);
        _bar.fillAmount = amount;
    }
}
