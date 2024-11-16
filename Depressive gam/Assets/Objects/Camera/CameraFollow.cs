using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _minPursuitDistance = 0.5f;
    [SerializeField, Range(0.5f, 2f)] private float _pursuitSpeed = 1f;

    [SerializeField] private SpriteRenderer _rectBounce;

    private Camera _camera;
    float _rectMinX, _rectMinY, _rectMaxX, _rectMaxY;

    private void Start()
    {
        _camera = GetComponent<Camera>();

        SetCameraBounce(_rectBounce);
    }

    public void SetCameraBounce(SpriteRenderer rectBounce)
    {
        if (rectBounce == null) return;
        var targetPosition = new Vector3(_target.position.x, transform.position.y, transform.position.z);
        transform.position = targetPosition;

        _rectBounce = rectBounce;
        _rectMinX = _rectBounce.transform.position.x - _rectBounce.bounds.size.x / 2f;
        _rectMinY = _rectBounce.transform.position.y - _rectBounce.bounds.size.y / 2f;
        _rectMaxX = _rectBounce.transform.position.x + _rectBounce.bounds.size.x / 2f;
        _rectMaxY = _rectBounce.transform.position.y + _rectBounce.bounds.size.y / 2f;
        transform.position = ClampInBounce(targetPosition);
    }

    private Vector3 ClampInBounce(Vector3 position)
    {
        float width = _camera.orthographicSize * _camera.aspect;

        float minX = _rectMinX + width;
        float maxX = _rectMaxX - width;

        return new Vector3(Mathf.Clamp(position.x, minX, maxX), transform.position.y, transform.position.z);
    }

    private void FollowTarget()
    {
        var targetPosition = new Vector3(_target.position.x, transform.position.y, transform.position.z);
        var distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget <= _minPursuitDistance) return;

        Vector3 position = Vector3.Slerp(transform.position, targetPosition, _pursuitSpeed * Time.deltaTime);
        transform.position = ClampInBounce(position);
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }
}
