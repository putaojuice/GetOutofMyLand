using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


// This is stub for the card system, remove once card system is completed
public class TestBuild : MonoBehaviour
{   
   [SerializeField] private GameObject tilePreviewPrefab;
    public Button spawnButton;

    void Start() 
    {
        Button btn = spawnButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskOnClick()
    {
        StartCoroutine(SpawnTile());
    }

    IEnumerator SpawnTile()
    {     
            GridController gridController = GameObject.Find("GameManager").GetComponent<GridController>();
            gridController.StartBuild(tilePreviewPrefab);
            yield return new WaitForSeconds(1f);

    }

}
