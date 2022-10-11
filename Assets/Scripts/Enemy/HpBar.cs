using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private Enemy enemy;
    [SerializeField] private RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = enemy.maxHp;
        hp = enemy.hp;
        rect = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        hp = enemy.hp;
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, hp/maxHp * 90);
    }
}
