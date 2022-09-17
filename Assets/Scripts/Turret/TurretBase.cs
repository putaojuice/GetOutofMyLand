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
			UpdateBuildStatus();
		} else {
			obj.Add(other.gameObject);
			UpdateBuildStatus();
        }

	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("GridFloor"))
		{	
            GridFloor gridFloor = other.GetComponent<GridFloor>();
            floor.Remove(gridFloor);
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
			buildable = true;
		}
		else
		{	
			buildable = false;
		}
	}

	public void SetColor(bool isBlockedPath) {
		buildable = !isBlockedPath;
		if (buildable) {
			turretMat.SetColor("_Color", buildableColor);
		} else {
			turretMat.SetColor("_Color", unbuildableColor);
		}

	}

	public bool GetBuildable()
	{	
		return floor.Count >= 1 && obj.Count == 0;
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
