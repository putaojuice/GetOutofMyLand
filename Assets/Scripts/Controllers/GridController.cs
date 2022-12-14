using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class GridController : MonoBehaviour
{
    
	[SerializeField] private LayerMask layer;
	[SerializeField] private NavMeshSurface surf;
	[SerializeField] private Camera cam;
	[SerializeField] private SoundEffectsControlScript SoundEffectsController;

	private List<GameObject> currentOuterFloor = new List<GameObject>();
	private DeckController DeckController;
	private GameObject previewPrefab;
	private GridTile gridTile;
	private bool isBuilding = false;
	private float maxNorth = 1f;
	private float maxSouth = 1f;
	private float maxEast = 1f;
	private float maxWest = 1f;

	private 

    // Start is called before the first frame update
    void Start()
    {
		// BuildNavMesh on start up
		surf.BuildNavMesh();
		DeckController = gameObject.GetComponent<DeckController>();
		initializeBoundary();
	}

    private void Update()
	{	
		if (isBuilding) {
			BuildLogic();
		}
		
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

	public void StopBuild()
	{	
		if (previewPrefab != null) {
			Destroy(previewPrefab);
		}
		
		previewPrefab = null;
		gridTile = null;
		isBuilding = false;
		DeckController.StopPlayCard();
	}

	private void CompleteBuild()
	{
		gridTile.Build();
		SoundEffectsController.PlayLandBuildingSound();
		DeckController.CompleteCard();
		// update navmesh data in run time
		surf.UpdateNavMesh(surf.navMeshData);
		updateCurrentGrid();
		StopBuild();
	}

	// This method casts a ray from player's mouse to the position on the screen in order for positioning and snapping of tile to work
	private void GenerateRay()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, ray.direction, out hit, Mathf.Infinity, layer))
		{
			PositionObj(hit.point);
		}
	}

	private void PositionObj(Vector3 position)
	{
		int x = Mathf.RoundToInt(position.x);
		int z = Mathf.RoundToInt(position.z);
		previewPrefab.transform.position = new Vector3(x, 0.1f, z);
	}

	// call once before the start of the game
	public void initializeBoundary() {
		GameObject[] floors = GameObject.FindGameObjectsWithTag("GridFloor");
		foreach (GameObject go in floors) {
			if (go.GetComponent<GridFloor>().isTurretFloor) {
				continue;
			}

			if (go.GetComponent<GridFloor>().isOuterFloor) {
				if (go.transform.position.x >= maxEast) {
					maxEast = go.transform.position.x;
				} 

				if (go.transform.position.x <= maxWest) {
					maxWest = go.transform.position.x;
				}

				if (go.transform.position.z >= maxNorth) {
					maxNorth = go.transform.position.z;
				} 

				if (go.transform.position.z <= maxSouth) {
					maxSouth = go.transform.position.z;
				}

				currentOuterFloor.Add(go);
			}
		}
	}


	public void updateCurrentGrid() {
		GameObject[] floors = GameObject.FindGameObjectsWithTag("GridFloor");
		// Init outfloor list
		
		foreach (GameObject go in floors) {
			// ignore the floor that has turrets
			if (go.GetComponent<GridFloor>().isTurretFloor) {
				continue;
			}

			if (!go.GetComponent<GridFloor>().isOuterFloor) {
				if (go.transform.position.x >= maxEast) {
					maxEast = go.transform.position.x;
					go.GetComponent<GridFloor>().isOuterFloor = true;
				} 

				if (go.transform.position.x <= maxWest) {
					maxWest = go.transform.position.x;
					go.GetComponent<GridFloor>().isOuterFloor = true;
				}

				if (go.transform.position.z >= maxNorth) {
					maxNorth = go.transform.position.z;
					go.GetComponent<GridFloor>().isOuterFloor = true;
				} 

				if (go.transform.position.z <= maxSouth) {
					maxSouth = go.transform.position.z;
					go.GetComponent<GridFloor>().isOuterFloor = true;
				}

				if (go.GetComponent<GridFloor>().isOuterFloor) {
					currentOuterFloor.Add(go);
				}

			}
		}

		List<GameObject> newList = new List<GameObject>();
		foreach (GameObject go in currentOuterFloor) {
			if (go.GetComponent<GridFloor>().isTurretFloor) {
				continue;
			}
			if (go.transform.position.x < maxEast && go.transform.position.x > maxWest
			&& go.transform.position.z < maxNorth && go.transform.position.z > maxSouth) {
				go.GetComponent<GridFloor>().isOuterFloor = false;
			} else {
				newList.Add(go);
			}
		} 

		currentOuterFloor = newList;

    }

	public List<GameObject> GetPossibleSpawnPointPosition() {
		
		return currentOuterFloor;
	}

}
