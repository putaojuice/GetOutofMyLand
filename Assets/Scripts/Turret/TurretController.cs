using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;


public class TurretController : MonoBehaviour
{
	[SerializeField] private LayerMask layer;
	[SerializeField] private NavMeshSurface surf;
	// agent attached to spawn point to calculate possibility of blocked path
	[SerializeField] private UnityEngine.AI.NavMeshAgent agent;
	[SerializeField] private GameObject playerBase;

	private GameObject previewPrefab;
	private TurretBase turretBase;
	private DeckController DeckController;
    private Camera cam;
	private bool isBuilding = false;
	private bool isBlockedPath = false;
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

		if (Input.GetMouseButton(0) && isBuilding && turretBase.GetBuildable() && !isBlockedPath)
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

	private void StopBuild()
	{
		Destroy(previewPrefab);
		previewPrefab = null;
		turretBase = null;
		isBuilding = false;
		DeckController.StopPlayCard();
	}

	private void CompleteBuild()
	{
		turretBase.Build();
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
			if (hit.transform.CompareTag("GridFloor"))
            {
                GameObject go = hit.transform.gameObject;
                PositionObj(go.transform.position);
                previewPrefab.GetComponent<TurretBase>().SetColor(true);
                PathCalculation();
				if (isBlockedPath) {
					 previewPrefab.GetComponent<TurretBase>().SetColor(isBlockedPath);
				}

            } else {
                PathCalculation();
                previewPrefab.GetComponent<TurretBase>().SetColor(false);
            }
		}

		   
	}

	private void PositionObj(Vector3 position)
	{
		int x = Mathf.RoundToInt(position.x);
		int z = Mathf.RoundToInt(position.z);
		previewPrefab.transform.position = position + new Vector3(0f, 0.85f, 0f);
	}

	private void PathCalculation()
    {
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
        agent.CalculatePath(playerBase.transform.position, path);
        // Debug.Log(path.status);

        if (path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            isBlockedPath = false;
        }
        else 
        {
            isBlockedPath = true;
        }

    }

}
