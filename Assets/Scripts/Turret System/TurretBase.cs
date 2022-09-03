using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    private List<GameObject> obj = new List<GameObject>();
	private List<GridFloor> floor = new List<GridFloor>();
	[SerializeField] private Material turretMat;
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

		if (other.CompareTag("GridFloor"))
		{	
			GridFloor gridFloor = other.GetComponent<GridFloor>();
            floor.Add(gridFloor);
            gridFloor.SetSelectionColor();
		} else {
            obj.Add(other.gameObject);
        }

		UpdateBuildStatus();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("GridFloor"))
		{	
            GridFloor gridFloor = other.GetComponent<GridFloor>();
            floor.Remove(gridFloor);
            gridFloor.SetSelectionColor();
		} else {
			obj.Remove(other.gameObject);
		}


		UpdateBuildStatus();
	}

	private void UpdateBuildStatus()
	{	
        // Check if turret is on top of floor 
		if (floor.Count >= 1 && obj.Count == 0)
		{	
			turretMat.SetColor("_Color", buildableColor);
			buildable = true;
		}
		else
		{	
			turretMat.SetColor("_Color", unbuildableColor);
			buildable = false;
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

	public void Rotate()
	{
		rotated = !rotated;
	}

	public bool GetRotateState()
	{
		return rotated;
	}
}
