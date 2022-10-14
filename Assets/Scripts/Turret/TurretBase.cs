using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{	
	private List<GameObject> detector = new List<GameObject>();
	private List<GameObject> obj = new List<GameObject>();
	private List<GridBase> floor = new List<GridBase>();
	[SerializeField] private Material turretMat;
	[SerializeField] private Material turretTileMat;
	[SerializeField] private Color buildableColor;
	[SerializeField] private Color unbuildableColor;
	[SerializeField] private GameObject buildPrefab;

	private bool buildable = false;
	private bool rotated = false;

	private void Start()
	{
		UpdateBuildStatus();
	}

	private void OnTriggerEnter(Collider other)
	{	
		if (other.gameObject.tag == "Detector" && other.GetType() == typeof(CapsuleCollider)) {
			detector.Add(other.gameObject);
		} else if (other.gameObject.tag == "GridBase")
		{	
			GridBase gridFloor = other.GetComponent<GridBase>();
            floor.Add(gridFloor);
		} else {
			obj.Add(other.gameObject);
        }

		UpdateBuildStatus();
	}

	private void OnTriggerExit(Collider other)
	{	
		if (other.gameObject.tag == "Detector" && other.GetType() == typeof(CapsuleCollider)) {
			detector.Remove(other.gameObject);
		} else if (other.gameObject.tag == "GridBase")
		{	
            GridBase gridFloor = other.GetComponent<GridBase>();
            floor.Remove(gridFloor);
		} else {
			obj.Remove(other.gameObject);
		}


		UpdateBuildStatus();
	}

	private void UpdateBuildStatus()
	{	
        // Check if turret is on top of floor 
		if (floor.Count >= 1 && obj.Count == 0 && detector.Count > 0)
		{	
			buildable = true;
			turretMat.SetColor("_Color", buildableColor);
			turretTileMat.SetColor("_Color", buildableColor);
		}
		else
		{	
			buildable = false;
			turretMat.SetColor("_Color", unbuildableColor);
			turretTileMat.SetColor("_Color", unbuildableColor);
		}
	}

	public bool GetBuildable()
	{	
		return buildable;
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

	public Vector3 BuildAndReturnPosition()
	{
		for (int i = 0; i < floor.Count; i++)
		{
			floor[i].SetSelectionColor();
		}

		Instantiate(buildPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
		return transform.position;
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
