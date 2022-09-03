using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

// This is stub for the card system, remove once card system is completed
public class TestBuild : MonoBehaviour
{   
   [SerializeField] private GameObject tilePreviewPrefab;
   
    void Start()
    {

    }
    
    public void OnMouseDown()
    {
        GridController gridController = GameObject.Find("Platform").GetComponent<GridController>();
        gridController.StartBuild(tilePreviewPrefab);
    }

}
