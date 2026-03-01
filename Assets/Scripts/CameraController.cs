using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float rotationSpeed = 5f;
    public float zoomSpeed = 2f;
    public float minDistance = 5f;
    public float maxDistance = 20f;

    private float _currentX = 0f;

    private float _currentY = 45f;

    public float waterLevel = 0f; // Altura del agua
    private bool _isUnderwater = false;

    private void LateUpdate()
    {
        if (!target) return;

        if (Input.GetMouseButton(1)) // Right button
        {
            _currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            _currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            _currentY = Mathf.Clamp(_currentY, -80f, 80f);
        }

        //Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        //Calculate orbital position
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance); // -distance = atrás
        transform.position = target.position + offset;

        transform.LookAt(target);
        CheckUnderwater();
    }

    private void CheckUnderwater()
    {
        bool underwater = transform.position.y < waterLevel;

        if (underwater && !_isUnderwater)
        {
            RenderSettings.fogDensity = 0.05f;
            RenderSettings.fogColor = new Color(0.1f, 0.3f, 0.4f);
            _isUnderwater = true;
        }
        else if (!underwater && _isUnderwater)
        {
            RenderSettings.fogDensity = 0.005f;
            RenderSettings.fogColor = new Color(0.53f, 0.81f, 0.92f);
            _isUnderwater = false;
        }
    }
}