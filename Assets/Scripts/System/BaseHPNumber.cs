using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseHPNumber : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private BaseHealth baseHp;
    [SerializeField] public Text textbox;

    // Start is called before the first frame update
    void Start()
    {
        hp = baseHp.Health;
        textbox = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        hp = baseHp.Health;
        textbox.text = "x " + hp;
    }
}
