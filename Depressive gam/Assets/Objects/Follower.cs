using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _minPursuitDistance = 0.5f;
    [SerializeField, Range(0.5f, 10f)] private float _pursuitSpeed = 1f;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void FollowTarget()
    {
        var targetPosition = new Vector3(_target.position.x, transform.position.y, transform.position.z);
        var distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget <= _minPursuitDistance) return;

        Vector3 position = Vector3.Slerp(transform.position, targetPosition, _pursuitSpeed * Time.deltaTime);
        position.y = transform.position.y;
        position.z = transform.position.z;
        if(position.x - transform.position.x > 0)
        {
            _renderer.flipX = false;
        }else
        {
            _renderer.flipX = true ;
        }
        transform.position = position;
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }
}
