using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBase : MonoBehaviour
{   
    [SerializeField]
    private MeshRenderer rend;

    private GridController gridController;
    private Color originalColor;
    private bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = rend.material.color;
    }
    
    
    
    public void OnMouseEnter()
	{   
        // Change alpha value to simulate highlight
        selected = true;
        SetSelectionColor();
	}

	public void OnMouseExit()
	{
        selected = false;
        SetSelectionColor();
    }


    public void SetGridController(GridController controller)
	{
        gridController = controller;
	}

    public void SetSelectionColor() 
    {
	
        if (selected)
        {
            Color temp = originalColor;
            temp.a = 0.3f;
            rend.material.color = temp;
        }
        else
        {
            rend.material.color = originalColor;
        }
	
    }

    // public GameObject generateSpawnPoint() {
    //     return Instantiate(spawnPoint, transform.position, transform.rotation);
    // }




}
