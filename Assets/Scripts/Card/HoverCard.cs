using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform Button;

    // Start is called before the first frame update
    void Start()
    {
        Button.GetComponent<Animator>().Play("No Hover");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Button.GetComponent<Animator>().Play("Hover On");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Button.GetComponent<Animator>().Play("Hover Off");
    }
}
