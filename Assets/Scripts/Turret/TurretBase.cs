using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{	
	private List<GameObject> detector = new List<GameObject>();
	private List<GameObject> obj = new List<GameObject>();
	private List<GridBase> floor = new List<GridBase>();
	private Turret sameTurret = null; 
	[SerializeField] private TurretType type;
	[SerializeField] private Material turretMat;
	[SerializeField] private Material turretTileMat;
	[SerializeField] private Color buildableColor;
	[SerializeField] private Color unbuildableColor;
	[SerializeField] private GameObject buildPrefab;

	private bool buildable = false;
	private bool upgradable = false;
	private bool rotated = false;

	private void Start()
	{
		UpdateBuildStatus();
	}

	private void OnTriggerEnter(Collider other)
	{	
		if (other.gameObject.tag == "Tower" && other.gameObject.GetComponent<Turret>().GetTurretType() == type) {
			sameTurret = other.gameObject.GetComponent<Turret>();
		} else if (other.gameObject.tag == "Detector" && other.GetType() == typeof(CapsuleCollider)) {
			detector.Add(other.gameObject);
		} else if (other.gameObject.tag == "GridBase")
		{	
			GridBase gridFloor = other.GetComponent<GridBase>();
            floor.Add(gridFloor);
		} else {
			if (other.gameObject.tag != "TowerRangeIndicator" && other.GetType() != typeof(CapsuleCollider)) {
				obj.Add(other.gameObject);
			}	
			
        }

		UpdateBuildStatus();
	}

	private void OnTriggerExit(Collider other)
	{	
		if (other.gameObject.tag == "Tower" && other.gameObject.GetComponent<Turret>().GetTurretType() == type) {
			sameTurret = null;	
		} else if (other.gameObject.tag == "Detector" && other.GetType() == typeof(CapsuleCollider)) {
			detector.Remove(other.gameObject);
		} else if (other.gameObject.tag == "GridBase")
		{	
            GridBase gridFloor = other.GetComponent<GridBase>();
            floor.Remove(gridFloor);
		} else {
			if (other.gameObject.tag != "TowerRangeIndicator" && other.GetType() != typeof(CapsuleCollider)) {
				obj.Remove(other.gameObject);
			}
		}


		UpdateBuildStatus();
	}

	private void UpdateBuildStatus()
	{	
		if (sameTurret != null && sameTurret.GetLevel() < 3) {
			upgradable = true;
			turretMat.SetColor("_Color", buildableColor);
			turretTileMat.SetColor("_Color", buildableColor);
		} else if (floor.Count >= 1 && obj.Count == 0 && detector.Count > 0)
		{	
			buildable = true;
			upgradable = false;
			turretMat.SetColor("_Color", buildableColor);
			turretTileMat.SetColor("_Color", buildableColor);
		}
		else
		{	
			buildable = false;
			upgradable = false;
			turretMat.SetColor("_Color", unbuildableColor);
			turretTileMat.SetColor("_Color", unbuildableColor);
		}
	}

	public bool GetBuildable()
	{	
		return buildable;
	}

	public bool GetUpgradable()
	{
		return upgradable;
	}

	public void Build()
	{
		for (int i = 0; i < floor.Count; i++)
		{
			floor[i].SetSelectionColor();
		}
		Instantiate(buildPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	public void Upgrade()
	{
		sameTurret.UpgradeTower();
		upgradable = false;
		Destroy(gameObject);
	}

	public GameObject BuildAndReturnTurret()
	{
		for (int i = 0; i < floor.Count; i++)
		{
			floor[i].SetSelectionColor();
		}

		Instantiate(buildPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
		return buildPrefab;
	}

	public void Rotate()
	{
		rotated = !rotated;
	}

	public bool GetRotateState()
	{
		return rotated;
	}


}
