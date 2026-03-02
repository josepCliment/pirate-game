using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BoatHUD : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private BoatMovement boat;

        [Header("Compass")] [SerializeField] private RectTransform compassNeedle;

        [Header("Speedometer")] [SerializeField]
        private RectTransform speedNeedle;

        [SerializeField] private float needleMinAngle = 130f;
        [SerializeField] private float needleMaxAngle = -130f;

        [Header("HP")] [SerializeField] private Slider hpSlider;
        [SerializeField] private Image hpFill;
        [SerializeField] private Color hpColorHigh = new Color(0.2f, 0.8f, 0.2f);
        [SerializeField] private Color hpColorLow = new Color(0.8f, 0.2f, 0.1f);

        [Header("HP (temporal hasta ShipInstance)")] [SerializeField]
        private float maxHp = 100f;

        private float _currentHp;

        private void Start()
        {
            _currentHp = maxHp;
            if (hpSlider)
            {
                hpSlider.minValue = 0f;
                hpSlider.maxValue = maxHp;
                hpSlider.value = _currentHp;
            }
        }

        private void Update()
        {
            UpdateCompass();
            UpdateSpeedometer();
            UpdateHp();
        }

        private void UpdateCompass()
        {
        }

        private void UpdateSpeedometer()
        {
        }

        private void UpdateHp()
        {
            if (hpSlider) return;

            hpSlider.value = _currentHp;
            if (hpFill)
            {
                float t = _currentHp / maxHp;
                hpFill.color = Color.Lerp(hpColorLow, hpColorHigh, t);
            }
        }

        public void TakeDamage(float damage)
        {
            _currentHp -= Mathf.Clamp(_currentHp - damage, 0f, maxHp);
        }

        [ContextMenu("Test Damage -10")]
        private void TestDamage() => TakeDamage(10f);
    }
}