using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    private List<GameObject> obj = new List<GameObject>();
	private List<GridBase> ground = new List<GridBase>();
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

		if (other.gameObject.tag == "GridFloor")
		{	
			obj.Add(other.gameObject);
		}

		if (other.CompareTag("GridBase"))
		{
			GridBase gridBase = other.GetComponent<GridBase>();
			ground.Add(gridBase);
			gridBase.SetSelectionColor();
		}

		UpdateBuildStatus();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "GridFloor")
		{	
			obj.Remove(other.gameObject);
		}

		if (other.CompareTag("GridBase"))
		{
			GridBase gridBase = other.GetComponent<GridBase>();
			ground.Remove(gridBase);
			gridBase.SetSelectionColor();
		}

		UpdateBuildStatus();
	}

	private void UpdateBuildStatus()
	{	

		if (obj.Count == 0 && ground.Count == tileSize)
		{
			
			UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
			UnityEngine.AI.NavMeshAgent agent = gameObject.transform.Find("agent").gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
			GameObject playerBase = GameObject.FindGameObjectWithTag("Endpoint");
			agent.CalculatePath(playerBase.transform.position, path);
			if (path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
			{
				tileMat.SetColor("_Color", buildableColor);
				buildable = true;
				return;
			}
			
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
			ground[i].SetSelectionColor();
		}


		Instantiate(buildPrefab, transform.position, transform.rotation);

		for (int i = 0; i < ground.Count; i++)
		{	
			Destroy(ground[i].gameObject);
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
