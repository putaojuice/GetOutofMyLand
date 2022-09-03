using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

// This is stub for the card system, remove once card system is completed
public class TestTurretBuild : MonoBehaviour
{   
   [SerializeField] private GameObject tilePreviewPrefab;
   
    void Start()
    {

    }
    
    public void OnMouseDown()
    {
        TurretController turretController = GameObject.Find("Platform").GetComponent<TurretController>();
        turretController.StartBuild(tilePreviewPrefab);
    }

}
