using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextMeshProScaleUp : MonoBehaviour
{
    public TextMeshProUGUI textmeshPro;

    void OnMouseEnter()
    {
        textmeshPro.fontSize = 74;
    }

    void OnMouseExit()
    {
        textmeshPro.fontSize = 54;
    }
}
