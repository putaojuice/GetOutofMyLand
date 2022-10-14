using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;


public class TurretController : MonoBehaviour
{
	[SerializeField] private LayerMask layer;
	[SerializeField] private NavMeshSurface surf;
	[SerializeField] private GameObject playerBase;
	[SerializeField] Camera cam;

	private GameObject previewPrefab;
	private TurretBase turretBase;
	private DeckController DeckController;
	private bool isBuilding = false;

	private int topRightTurretCount = 0;
	private int topLeftTurretCount = 0;
	private int botRightTurretCount = 0;
	private int botLeftTurretCount = 0;

	// Start is called before the first frame update
	void Start()
    {
		// BuildNavMesh on start up
		surf.BuildNavMesh();
		DeckController = gameObject.GetComponent<DeckController>();
	}

    private void Update()
	{   
		BuildLogic();
	}

	public void BuildLogic()
	{   

		if (Input.GetMouseButton(0) && isBuilding && turretBase.GetBuildable())
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
			previewPrefab.GetComponent<TurretBase>().Rotate();
		}

		// Calls method GenerateRay in order for player to place the tile
		if (isBuilding)
		{	
			GenerateRay();
		}
	}

	public void StartBuild(GameObject obj)
	{
		previewPrefab = Instantiate(obj, new Vector3(0, 1.5f, 0), Quaternion.identity);
		turretBase = previewPrefab.GetComponent<TurretBase>();
		isBuilding = true;
	}

	public void StopBuild()
	{	
		if (previewPrefab != null) {
			Destroy(previewPrefab);
		}
		
		previewPrefab = null;
		turretBase = null;
		isBuilding = false;
		DeckController.StopPlayCard();
	}

	private void CompleteBuild()
	{
		//turretBase.Build();
		Vector3 newTurretPosition = turretBase.BuildAndReturnPosition();
		UpdateTurretCount(newTurretPosition);

		DeckController.CompleteCard();
		// update navmesh data in run time
		surf.UpdateNavMesh(surf.navMeshData);
		isBuilding = false;
		StopBuild();
	}

	private void UpdateTurretCount(Vector3 turretPosition) {
		float xPos = turretPosition.x;
		float zPos = turretPosition.z;
		if (xPos > 0 && zPos > 0)
		{
			// turret exists in the top right portion
			topRightTurretCount += 1;
			Debug.Log("added one to top right");
		}
		else if (xPos < 0 && zPos < 0)
		{
			// turret exists in the bot left portion
			botLeftTurretCount += 1;
			Debug.Log("added one to bot left");

		}
		else if (xPos > 0 && zPos < 0)
		{
			// turret exists in the bot right portion
			botRightTurretCount += 1;
			Debug.Log("added one to bot right");

		}
		else if (xPos < 0 && zPos > 0)
		{
			// turret exists in the top left portion
			topLeftTurretCount += 1;
			Debug.Log("added one to top left");

		}
	}

	public int GetTopRightTurretCount() { 
		return topRightTurretCount;
	}

	public int GetBotRightTurretCount()
	{ 
		return botRightTurretCount;
	}

	public int GetTopLeftTurretCount()
	{ 
		return topLeftTurretCount;
	}

	public int GetBotLeftTurretCount()
	{ 
		return botLeftTurretCount;
	}

	// This method casts a ray from player's mouse to the position on the screen in order for positioning and snapping of tile to work
	private void GenerateRay()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, layer))
		{	
			GameObject go = hit.transform.gameObject;
			PositionObj(go.transform.position); 
		}
		   
	}

	private void PositionObj(Vector3 position)
	{
		int x = Mathf.RoundToInt(position.x);
		int z = Mathf.RoundToInt(position.z);
		previewPrefab.transform.position = position + new Vector3(0f, 0f, 0f);
	}

}
