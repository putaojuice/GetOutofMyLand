using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class GridController : MonoBehaviour
{
    
	[SerializeField] private LayerMask layer;
	[SerializeField] private NavMeshSurface surf;

	private DeckController DeckController;
	private GameObject previewPrefab;
	private GridTile gridTile;
	private bool isBuilding = false;
	private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
		// BuildNavMesh on start up
		surf.BuildNavMesh();
		cam = GameObject.Find("Camera").GetComponent<Camera>();
		DeckController = gameObject.GetComponent<DeckController>();
    }

    private void Update()
	{
		BuildLogic();
	}

	public void BuildLogic()
	{
		if (Input.GetMouseButton(0) && isBuilding && gridTile.GetBuildable())
		{
			CompleteBuild();
		}
		
		if (Input.GetMouseButton(1) && isBuilding)
		{
			StopBuild();
		}

		// Rotation feature for the preview tile that player is going to build
		if (Input.GetKeyDown(KeyCode.R) && isBuilding)
		{
			previewPrefab.transform.Rotate(0f, 90f, 0f);
			previewPrefab.GetComponent<GridTile>().Rotate();
		}

		// Calls method GenerateRay in order for player to place the tile
		if (isBuilding)
		{	
			GenerateRay();
		}
	}

	public void StartBuild(GameObject obj)
	{
		previewPrefab = Instantiate(obj, Vector3.zero, Quaternion.identity);
		gridTile = previewPrefab.GetComponent<GridTile>();
		isBuilding = true;
	}

	private void StopBuild()
	{
		Destroy(previewPrefab);
		previewPrefab = null;
		gridTile = null;
		isBuilding = false;
		DeckController.StopPlayCard();
	}

	private void CompleteBuild()
	{
		gridTile.Build();
		DeckController.CompleteCard();
		// update navmesh data in run time
		surf.UpdateNavMesh(surf.navMeshData);
		StopBuild();
	}

	// This method casts a ray from player's mouse to the position on the screen in order for positioning and snapping of tile to work
	private void GenerateRay()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, layer))
		{	
			PositionObj(hit.point);
		}
	}

	private void PositionObj(Vector3 position)
	{
		int x = Mathf.RoundToInt(position.x);
		int z = Mathf.RoundToInt(position.z);

		if (previewPrefab.GetComponent<GridTile>().GetRotateState())
		{
			previewPrefab.transform.position = new Vector3(x, 0, z);
		}
		else
		{
			previewPrefab.transform.position = new Vector3(x, 0, z);
		}
		
	}

}
