using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class GridFloor : MonoBehaviour
{   
    
    
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject spawnPoint;
    private GridController controller;
    [Header("Grid Parameters")]
    [SerializeField] private int blockSize;

    public bool isOuterFloor = true;
    private MeshRenderer rend;
    private bool selected = false;
    private Color originalColor;
    
    // Start is called before the first frame update
    void Start()
    {   
        GenerateGrid();
        rend = GetComponent<MeshRenderer>();
        originalColor = rend.material.color;
        StartCoroutine(updateAfterSpawn());
        controller = GameManager.instance.gameObject.GetComponent<GridController>();
    }

    IEnumerator updateAfterSpawn() {
        yield return new WaitForEndOfFrame();
        controller.updateCurrentGrid();
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

    public void CheckSurroundingTiles() {
       
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.84f);
        int count = 0;
        foreach (Collider collider in colliders)
        {   
            if (collider.gameObject.tag == "GridFloor" || collider.gameObject.tag == "Environment") {
                count++;
            }
        }

        if (count <= 5) {
            isOuterFloor = true;
        } else {
            isOuterFloor = false;
        }
        
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
        for (int x = -4; x <= 4; x++)
		{
            for (int z = -4; z <= 4; z++)
			{   

                Vector3 spawnPos = new Vector3(transform.position.x + x +  blockSize / 2,
                 0f, transform.position.z + z + blockSize / 2);

                // Smaller overlap box to detect collision
                // Do not spawn object if occupied
                Collider[] colliders = Physics.OverlapBox(spawnPos, new Vector3( blockSize / 2.2f, blockSize / 2.2f,  blockSize / 2.2f), transform.rotation);
                if (spawnPos == transform.position) {
                    continue;
                }
                bool occupied = false;
                foreach (Collider collider in colliders) {
                    if (collider.gameObject.tag == "Detector") {
                        continue;
                    } else {
                        occupied = true;
                        break;
                    }
                    
                }
                if (occupied) {
                    continue;
                }
                
                GameObject block = Instantiate(blockPrefab, spawnPos, transform.rotation);
                block.GetComponent<GridBase>().SetGridController(controller);
                block.transform.SetParent(transform);
			}
		}
    }

    public GameObject generateSpawnPoint() {
        return Instantiate(spawnPoint, new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), transform.rotation);
    }




}
