using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;

    private Renderer rend;
    public Color startColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void onMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    void onMouseExit()
    {
        rend.material.color = startColor;
    }
}
