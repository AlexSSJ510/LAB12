using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation; // Necesario para AR
using UnityEngine.XR.ARSubsystems; // Necesario para subsistemas AR
using UnityEngine.InputSystem;

public class ARPlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject _objectToPlacePrefab;
    [SerializeField] private ARRaycastManager _raycastManager;

    private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private GameObject _spawnedObject;

    // Usamos el input táctil simple del Input System
    private void Update()
    {
        // Detectar toque en pantalla (Touchscreen)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // Lanzar rayo AR contra los planos detectados
            if (_raycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = _hits[0].pose;

                if (_spawnedObject == null)
                {
                    _spawnedObject = Instantiate(_objectToPlacePrefab, hitPose.position, hitPose.rotation);
                    // Asegurar que mire hacia la cámara (opcional)
                    Vector3 lookPos = Camera.main.transform.position;
                    lookPos.y = _spawnedObject.transform.position.y;
                    _spawnedObject.transform.LookAt(lookPos);
                    _spawnedObject.transform.Rotate(0, 180, 0); // Corregir rotación si es necesario
                }
                else
                {
                    _spawnedObject.transform.position = hitPose.position;
                }
            }
        }
    }
}