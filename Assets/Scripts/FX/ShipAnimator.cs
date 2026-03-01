using System;
using UnityEngine;

namespace FX
{
    public class ShipAnimator : MonoBehaviour
    {
        [SerializeField] private Transform wheel;

        [SerializeField] private float wheelRotationSpeed = 90f;
        [SerializeField] private float wheelMaxAngle = 270f;
        [SerializeField] private float wheelReturnSpeed = 270f;

        private float _turnInput;

        private float _currentWheelAngle = 0f;

        void Update()
        {
            _turnInput = Input.GetAxis("Horizontal");

            RotateWheel();
        }

        private void RotateWheel()
        {
            if (Mathf.Abs(_turnInput) > 0.01f)
            {
                float delta = -_turnInput * wheelRotationSpeed * Time.deltaTime;
                float newAngle = Mathf.Clamp(_currentWheelAngle + delta, -wheelMaxAngle, wheelMaxAngle);

                delta = newAngle - _currentWheelAngle;
                _currentWheelAngle = newAngle;

                wheel.Rotate(Vector3.right, delta);
            }
            else
            {
                float returnDelta = -Mathf.Sign(_currentWheelAngle) * wheelReturnSpeed * Time.deltaTime;
                if (Math.Abs(returnDelta) > Math.Abs(_currentWheelAngle))
                {
                    returnDelta = -_currentWheelAngle;
                }

                _currentWheelAngle += returnDelta;
                wheel.Rotate(Vector3.right, returnDelta);
            }
        }
    }
}