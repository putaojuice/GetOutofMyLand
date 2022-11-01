using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using TMPro;

public class TurretController : MonoBehaviour
{
	[SerializeField] private LayerMask layer;
	[SerializeField] private NavMeshSurface surf;
	[SerializeField] private GameObject playerBase;
	[SerializeField] Camera cam;
	[SerializeField] GameObject statsPanel;

	private GameObject previewPrefab;
	private TurretBase turretBase;
	private DeckController DeckController;
	private Turret currentTurret;
	private bool isBuilding = false;

	// Start is called before the first frame update
	void Start()
    {	
		
		// BuildNavMesh on start up
		surf.BuildNavMesh();
		DeckController = gameObject.GetComponent<DeckController>();
	}

    private void Update()
	{   
		if (isBuilding) {
			BuildLogic();
		} else {
			if (Input.GetMouseButton(0)) {
				SelectingTurret();
			}
		}
		
	}

	private void SelectingTurret()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);  
		LayerMask mask = LayerMask.GetMask("Tower");
        RaycastHit hit;  
        if (Physics.Raycast(cam.transform.position, ray.direction, out hit, Mathf.Infinity, mask)) { 
			 if (hit.transform.gameObject.tag == "Tower") {
				Turret selected = hit.transform.gameObject.GetComponent<Turret>();
				if (currentTurret != null && !Object.ReferenceEquals(currentTurret, selected)) {
					currentTurret.highlighted = false;
				}
				currentTurret = selected;
				selected.highlighted = true;
				float damage = currentTurret.GetDamage();
				statsPanel.transform.Find("levelBox").Find("Level").gameObject.GetComponent<TMP_Text>().text = "Level " + currentTurret.towerLevel;
				statsPanel.transform.Find("statBox").Find("Damage").gameObject.GetComponent<TMP_Text>().text = "Damage: " + damage;
				statsPanel.transform.Find("statBox").Find("Range").gameObject.GetComponent<TMP_Text>().text = "Range: " + currentTurret.range;
				statsPanel.transform.Find("statBox").Find("FireRate").gameObject.GetComponent<TMP_Text>().text = "Fire Rate: " + currentTurret.firingRate;
				statsPanel.SetActive(true);
			 } else {
				if (currentTurret != null) {
					currentTurret.highlighted = false;
					currentTurret = null;
				}
				statsPanel.SetActive(false);
			 }
        } else {
			if (currentTurret != null) {
				currentTurret.highlighted = false;
				currentTurret = null;
			}
			
			statsPanel.SetActive(false);
        }
    }

	private void SelectLogic()
	{

	}

	public void BuildLogic()
	{   
		
		if (Input.GetMouseButton(0) && isBuilding && turretBase.GetBuildable())
		{	
			CompleteBuild();
		}

		if (Input.GetMouseButton(0) && isBuilding && turretBase.GetUpgradable())
		{
			CompleteUpgrade();
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
		turretBase.Build();

		DeckController.CompleteCard();
		// update navmesh data in run time
		surf.UpdateNavMesh(surf.navMeshData);
		isBuilding = false;
		StopBuild();
	}

	
	private void CompleteUpgrade()
	{
		turretBase.Upgrade();

		DeckController.CompleteCard();
		isBuilding = false;
		StopBuild();
	}


	// This method casts a ray from player's mouse to the position on the screen in order for positioning and snapping of tile to work
	private void GenerateRay()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		//if (Physics.Raycast(ray, out hit, layer))
		if (Physics.Raycast(cam.transform.position, ray.direction, out hit, Mathf.Infinity, layer))
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
