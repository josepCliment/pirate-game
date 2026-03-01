using System;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public float acceleration = 5f;
    public float rotationSpeed = 2f;
    public float maxSpeed = 7f;

    public float dragCoefficient = 0.05f;
    public float minDriftSpeed = 0.2f;

    public float bobbingSpeed = 1f;
    public float bobbingAmount = 0.2f;
    public float rollSpeed = 2f;
    public float rollAmount = 5f;

    private float _currentSpeed;
    private float _initialY;
    private float _currentRollX = 0f;
    private float _currentRollZ = 0f;
    private Rigidbody _rigidbody;
    private bool _isColliding = false;

    private void Start()
    {
        _initialY = transform.position.y;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveBoat();
    }

    void OnCollisionEnter(Collision collision)
    {
        _isColliding = true;
    }

    void OnCollisionExit(Collision collision)
    {
        _isColliding = false;
    }

    private void MoveBoat()
    {
        float h = Input.GetAxis("Horizontal"); // -1 (izq) a 1 (der) ROTATE
        float v = Input.GetAxis("Vertical"); // -1 (atrás) a 1 (adelante) MOVE

        //Rotate the boat
        transform.Rotate(0, h * rotationSpeed * Time.fixedDeltaTime, 0);

        if (v != 0)
        {
            _currentSpeed += v * acceleration * Time.fixedDeltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, -maxSpeed * .3f, maxSpeed);
        }

        float dragForce = dragCoefficient * _currentSpeed * _currentSpeed * Mathf.Sign(_currentSpeed);
        _currentSpeed -= dragForce * Time.fixedDeltaTime;

        if (Mathf.Abs(_currentSpeed) < minDriftSpeed && v == 0)
        {
            _currentSpeed = minDriftSpeed * Mathf.Sign(_currentSpeed);
            if (_currentSpeed == 0) _currentSpeed = minDriftSpeed;
        }


        CheckCollisions();
        ApplyBobbing(h);
    }

    private float CalculateBobbing()
    {
        float wave1 = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        float wave2 = Mathf.Sin(Time.time * bobbingSpeed * 0.7f) * bobbingAmount * 0.5f;
        float wave3 = Mathf.Sin(Time.time * bobbingSpeed * 1.3f) * bobbingAmount * 0.3f;

        float speedFactor = 1f - (Mathf.Abs(_currentSpeed) / maxSpeed) * 0.5f;
        return (wave1 + wave2 + wave3) * speedFactor;
    }

    private void ApplyBobbing(float h)
    {
        float waveRollX = Mathf.Sin(Time.time * bobbingSpeed * 0.5f) * 1.5f;
        float waveRollZ = Mathf.Sin(Time.time * bobbingSpeed * 0.8f + 1.0f) * 1.5f;

        float targetRollZ = -h * rollAmount + waveRollZ;
        float targetRollX = waveRollX;

        targetRollX = Mathf.Clamp(targetRollX, -3f, 3f); // Máximo 3 grados
        targetRollZ = Mathf.Clamp(targetRollZ, -rollAmount, rollAmount);

        _currentRollX = Mathf.Lerp(_currentRollX, targetRollX, rollSpeed * Time.fixedDeltaTime);
        _currentRollZ = Mathf.Lerp(_currentRollZ, targetRollZ, rollSpeed * Time.fixedDeltaTime);

        float currentYRotation = transform.localEulerAngles.y;
        transform.localRotation = Quaternion.Euler(_currentRollX, currentYRotation, _currentRollZ);
    }

    private void CheckCollisions()
    {
        float bobbing = CalculateBobbing();

        if (!_isColliding || _currentSpeed < 0)
        {
            Vector3 horizontalMovement = transform.forward * (_currentSpeed * Time.fixedDeltaTime);
            Vector3 newPosition = transform.position + horizontalMovement;
            newPosition.y = _initialY + bobbing;

            _rigidbody.MovePosition(newPosition);
        }
        else
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, 5f * Time.fixedDeltaTime);
            Vector3 pos = transform.position;
            pos.y = _initialY + bobbing;
            _rigidbody.MovePosition(pos);
        }
    }
}