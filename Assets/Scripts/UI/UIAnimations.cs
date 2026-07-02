using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] private float pulseScale = 1.05f;
    [SerializeField] private float pulseSpeed = 1.2f;

    [Header("Rotation")]
    [SerializeField] private float rotationAngle = 1.5f;

    private Vector3 initialScale;
    private Quaternion initialRotation;

    private void Awake()
    {
        initialScale = transform.localScale;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        float pulse = (Mathf.Sin(Time.unscaledTime * pulseSpeed * Mathf.PI * 2f) + 1f) * 0.5f;
        transform.localScale = Vector3.Lerp(initialScale, initialScale * pulseScale, pulse);

        float angle = Mathf.Sin(Time.unscaledTime * pulseSpeed * Mathf.PI * 2f) * rotationAngle;
        transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, angle);
    }
}
