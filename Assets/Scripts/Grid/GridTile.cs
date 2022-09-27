using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{	
	private List<GameObject> detector = new List<GameObject>();
    private List<GameObject> obj = new List<GameObject>();
	private List<GameObject> ground = new List<GameObject>();
	[SerializeField] private Material tileMat;
	[SerializeField] private Color buildableColor;
	[SerializeField] private Color unbuildableColor;
	[SerializeField] private GameObject buildPrefab;
	[SerializeField] private int tileSize;
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
		}

		if (other.gameObject.tag == "GridFloor")
		{	
			obj.Add(other.gameObject);
		}

		if (other.gameObject.tag == "GridBase")
		{
			GridBase gridBase = other.GetComponent<GridBase>();
			ground.Add(other.gameObject);
			gridBase.SetSelectionColor();
		}

		UpdateBuildStatus();
	}

	private void OnTriggerExit(Collider other)
	{	
		if (other.gameObject.tag == "Detector" && other.GetType() == typeof(CapsuleCollider)) {
			detector.Remove(other.gameObject);
		}

		if (other.gameObject.tag == "GridFloor")
		{	
			obj.Remove(other.gameObject);
		}

		if (other.gameObject.tag == "GridBase")
		{
			GridBase gridBase = other.GetComponent<GridBase>();
			ground.Remove(other.gameObject);
			gridBase.SetSelectionColor();
		}

		UpdateBuildStatus();
	}

	private void UpdateBuildStatus()
	{	
		if (obj.Count == 0 && ground.Count == tileSize && detector.Count > 0)
		{	
			tileMat.SetColor("_Color", buildableColor);
			buildable = true;
			return;
		}
			
		tileMat.SetColor("_Color", unbuildableColor);
		buildable = false;
		
	}

	public bool GetBuildable()
	{
		return buildable;
	}

	public void Build()
	{
		for (int i = 0; i < ground.Count; i++)
		{
			ground[i].GetComponent<GridBase>().SetSelectionColor();
		}


		Instantiate(buildPrefab, transform.position, transform.rotation);

		for (int i = 0; i < ground.Count; i++)
		{	
			Destroy(ground[i]);
		}

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
