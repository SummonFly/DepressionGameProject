using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class LocationTransition : MonoBehaviour
{
    [SerializeField] public Transform Target;
    [SerializeField] public CameraFollow TargetCamera;

    [SerializeField] public UnityEvent OnTransition;
    [SerializeField] public UnityEvent OnStartTransition;
    [SerializeField] public UnityEvent OnEndTransition;

    [SerializeField] private List<TransitionConfig> _transitionPoints;

    private Animator _animator;
    private TransitionConfig _currentDistinationPoint = null;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }


    public void BeforeTransition()
    {
        OnStartTransition?.Invoke();
    }
    public void AfterTransition() 
    {
        OnEndTransition?.Invoke();
    }

    public void WhileTransition()
    {
        if (_currentDistinationPoint != null)
        {
            var position = _currentDistinationPoint.DistinationPosition.position;
            position.z = Target.position.z;
            Target.position = position;

            TargetCamera.SetCameraBounce(_currentDistinationPoint.TargetCameraBounds);
        }
        OnTransition?.Invoke();

    }

    public void TeleportTo(int positionId)
    {
        if (positionId > _transitionPoints.Count || positionId < 0) return;
        _animator.SetTrigger("StartTransition");
        _currentDistinationPoint = _transitionPoints[positionId];
    }
}

[Serializable]
public class TransitionConfig
{
    [SerializeField] public SpriteRenderer TargetCameraBounds;
    [SerializeField] public Transform DistinationPosition;
}