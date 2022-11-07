using System.Collections;

using System.Collections.Generic;

using UnityEngine;




public class SpellController : MonoBehaviour

{

    [SerializeField] private LayerMask layer;
	[SerializeField] private Camera cam;
    private DeckController DeckController;
	private GameObject previewPrefab;
    private SpellBase spellBase;
    private bool isCasting = false;

    // Start is called before the first frame update
    void Start()
    {
        DeckController = gameObject.GetComponent<DeckController>();
    }

    // Update is called once per frame
    void Update()

    {
        CastLogic();
    }

    public void CastLogic()

	{

		if (Input.GetMouseButton(0) && isCasting)
		{
			Cast();
		}

		if (Input.GetMouseButton(1) && isCasting)

		{	
			StopCast();
        }

		// Calls method GenerateRay in order for player to place the tile

		if (isCasting)
		{	
			GenerateRay();
		}
	}

	public void StartCasting(GameObject obj)

	{
		previewPrefab = Instantiate(obj, Vector3.zero, Quaternion.identity);
        spellBase = previewPrefab.GetComponent<SpellBase>();
		isCasting = true;
	}

    public void StopCast()

	{	
		if (previewPrefab != null) {
			Destroy(previewPrefab);
		}

		previewPrefab = null;
        spellBase = null;
		isCasting = false;
		DeckController.StopPlayCard();

    }

    private void Cast()

	{
		DeckController.CompleteCard();
		// update navmesh data in run time
        spellBase.CastSpell();
		StopCast();

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
        int y = Mathf.RoundToInt(position.y);
		int z = Mathf.RoundToInt(position.z);
        previewPrefab.transform.position = new Vector3(x,y,z);

	}

}