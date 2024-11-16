using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class SimplePuzzlePiece : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool IsDistination => _isDistination;

    [SerializeField] public UnityEvent OnStateUpdate;

    [SerializeField] private float _substitutionDistance;

    private bool _isDistination = false;
    private bool _state = false;

    public void OnDrag(PointerEventData eventData)
    {
        var pos = Vector3.Distance(transform.localPosition, Vector3.zero);
        if(pos <= _substitutionDistance)
        {
            transform.localPosition = Vector3.zero;
            _state = true;
        }else
        {
            transform.position = eventData.position;
            _state = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(_state != _isDistination)
        {
            _isDistination = _state;
            OnStateUpdate?.Invoke();
        }
    }
}
