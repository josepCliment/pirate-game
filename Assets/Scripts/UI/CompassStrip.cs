using UnityEngine;

namespace UI
{
    public class CompassStrip : MonoBehaviour
    {
        [SerializeField] private Transform boat;
        [SerializeField] private RectTransform strip;
        [SerializeField] private float stripWidth = 1200f;

        private void Update()
        {
            float yaw = boat.eulerAngles.y;
            float t = yaw / 360f;
            float x = Mathf.Lerp(0f, -stripWidth, t);
            strip.anchoredPosition = new Vector2(x, 0f);
        }
    }
}