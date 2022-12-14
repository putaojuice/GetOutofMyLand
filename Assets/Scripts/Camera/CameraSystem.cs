using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{   
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    [Header("Camera Attributes")]
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float dragSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomDistanceMax;
    [SerializeField] private float zoomDistanceMin;
    [SerializeField] private int scrollSize;
    [SerializeField] private bool enableEdgeScrolling;
    [SerializeField] private bool enableMouseDrag;
    
    private bool dragMovementActive;
    private Vector2 lastMousePosition;
    private float targetFieldOfView = 80;
    private float deltaTime;

    public static CameraSystem instance;

    private void Awake()
    {
        if (instance == null) {
         instance = this;
         DontDestroyOnLoad(gameObject);
        } else {
         Object.Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        deltaTime = 1.0f / 4.0f;
         translateSpeed = 1.5f;
        rotateSpeed = 1.5f;
        dragSpeed = 0.5f;
        zoomSpeed = 4.0f;
        zoomDistanceMax = 80.0f;
        zoomDistanceMin = 10.0f;
        scrollSize = 2;

    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            HandleCameraTranslation();
            HandleCameraRotation();
            HandleCameraZoom();

       
        }
    }

    // Handle all camera translation
    private void HandleCameraTranslation() {
        Vector3 translateDir = new Vector3(0, 0, 0);
        // Define translate input & edge scroll conditions
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            translateDir.z = 1f;
        } 

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            translateDir.z = -1f;
        } 

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            translateDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            translateDir.x = 1f;
        }

        // Compute translate direction & update translate position
        Vector3 moveDir = transform.forward * translateDir.z + transform.right * translateDir.x;
        transform.position += moveDir * translateSpeed * deltaTime;
        
        if (enableEdgeScrolling) {
            HandleEdgeScrolling();
        }

        if (enableMouseDrag) {
            HandleMouseDrag();
        }

    }

    // Handle camera translation from edge scrolling
    private void HandleEdgeScrolling() {
        Vector3 translateDir = new Vector3(0, 0, 0);
        if (Input.mousePosition.y > Screen.height - scrollSize) {
            translateDir.z = 1f;
        }

        if (Input.mousePosition.y < scrollSize) {
            translateDir.z = -1f;
        }

        if (Input.mousePosition.x < scrollSize) {
            translateDir.x = -1f;
        }

        if (Input.mousePosition.x > Screen.width - scrollSize) {
            translateDir.x = 1f;
        }
        
        Vector3 moveDir = transform.forward * translateDir.z + transform.right * translateDir.x;
        transform.position += moveDir * translateSpeed * deltaTime;
    }

    // Handle camera translation from Mouse dragging
    private void HandleMouseDrag() {
        Vector3 translateDir = new Vector3(0, 0, 0);
        // Mouse Drag control
        // 0 for left-mouse, 1 for right-mouse
        if (Input.GetMouseButtonDown(0)) {
            dragMovementActive = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) {
            dragMovementActive = false;
        }

        if (dragMovementActive) {
            Vector2 mouseMove = (Vector2) Input.mousePosition - lastMousePosition;
            translateDir.x = mouseMove.x * dragSpeed;
            translateDir.z = mouseMove.y * dragSpeed;
            lastMousePosition = Input.mousePosition;
        }

        Vector3 moveDir = transform.forward * translateDir.z + transform.right * translateDir.x;
        transform.position += moveDir * translateSpeed * deltaTime;
    }

    private void HandleCameraZoom() {
        if (Input.mouseScrollDelta.y > 0) {
            targetFieldOfView -= zoomSpeed;
        }

        if (Input.mouseScrollDelta.y < 0) {
            targetFieldOfView += zoomSpeed;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, zoomDistanceMin, zoomDistanceMax);

        // smoothing
        virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, targetFieldOfView, deltaTime * zoomSpeed); 
    }

    // Handle camera rotation
    private void HandleCameraRotation() {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir = 1f;
        if (Input.GetKey(KeyCode.E)) rotateDir = -1f;
        // Update camera rotation
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * deltaTime, 0);
    }
}
