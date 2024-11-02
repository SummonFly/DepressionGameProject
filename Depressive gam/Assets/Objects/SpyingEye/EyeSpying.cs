using UnityEngine;

public class EyeSpying : MonoBehaviour
{
    [SerializeField] private Transform _pupilTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _mouseSpyingOnly = true;
    [SerializeField] private float _pupilMoveDistance = 1;

    private void Start()
    {
        if(_target == null)
            _target = transform;
    }

    private void Update()
    {
        var target = _mouseSpyingOnly ? _camera.ScreenToWorldPoint(Input.mousePosition) : _target.position;
        Vector3 offset = (target - transform.position);
        var translate = transform.position + Vector3.ClampMagnitude(offset, _pupilMoveDistance);
        _pupilTransform.position = new Vector3(translate.x, translate.y, _pupilTransform.position.z);
    }

}
