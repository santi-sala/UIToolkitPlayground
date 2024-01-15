using UnityEngine;

public class Cube : MonoBehaviour
{
    private float _targetScale = 1;
    private Vector3 _scaleVelocity;
    private Quaternion _targetRotation;

    private void OnEnable()
    {
        MainMenuScript.ScaleChanged += OnScaleChanged;
        MainMenuScript.SpinClicked += OnSpinClicked;
    }

    private void OnSpinClicked()
    {
        _targetRotation = transform.rotation * Quaternion.Euler(Random.insideUnitSphere * 360);
    }

    private void OnDisable()
    {
        MainMenuScript.ScaleChanged -= OnScaleChanged;
        MainMenuScript.SpinClicked -= OnSpinClicked;
    }

    private void OnScaleChanged(float newScale)
    {
        _targetScale = newScale;
    }
    private void Update()
    {
        transform.localScale = Vector3.SmoothDamp(transform.localScale, _targetScale * Vector3.one, ref _scaleVelocity, 0.15f);

        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * 5);
    }
}
