using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFloor : MonoBehaviour
{   
    
    
    [SerializeField] private GameObject blockPrefab;

    
    [Header("Grid Parameters")]
    [SerializeField] private int maxX;
    [SerializeField] private int maxZ;
    [SerializeField] private int blockSize;

    private GridController controller;
    private MeshRenderer rend;
    private bool selected = false;
    private Color originalColor;
    
    // Start is called before the first frame update
    void Start()
    {   
        controller = GameObject.Find("GameManager").GetComponent<GridController>();
        GenerateGrid();
        rend = GetComponent<MeshRenderer>();
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


    void GenerateGrid() 
    {   
        // On each tile, generate grid around it when built
        for (int x = -maxX; x <= maxX; x++)
		{
            for (int z = -maxZ; z <= maxZ; z++)
			{   

                Vector3 spawnPos = new Vector3(transform.position.x + x +  blockSize / 2,
                 0.4f, transform.position.z + z + blockSize / 2);

                // Smaller overlap box to detect collision
                // Do not spawn object if occupied
                Collider[] colliders = Physics.OverlapBox(spawnPos, new Vector3( blockSize / 2.2f, blockSize / 2.2f,  blockSize / 2.2f), transform.rotation);
                if (spawnPos == transform.position || colliders.Length != 0) {
                    continue;
                }
                
                GameObject block = Instantiate(blockPrefab, spawnPos, transform.rotation);
                block.GetComponent<GridBase>().SetGridController(controller);
                block.transform.SetParent(transform);
			}
		}

        controller.updateCurrentGrid();


    }



}
