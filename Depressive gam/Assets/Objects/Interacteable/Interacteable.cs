using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Interacteable : MonoBehaviour
{
    [SerializeField] private UnityEvent _onShow;
    [SerializeField] private UnityEvent _onHide;
    [SerializeField] private UnityEvent _onInteract;
    [SerializeField] private GameObject _previewTarget;

    [SerializeField] private float _showDelay = 1f;

    private float _currentTime = 0f;
    private bool _isShowed = false;
    private bool _collision;

    public void Interact(InputAction.CallbackContext context)
    {
        if(!_collision || !_isShowed) return;
        var intaract = context.ReadValue<float>() == 0f? false:true;
        if (intaract)
        {
            _onInteract?.Invoke();
        }
    }

    public void ActivatePreviewTarget()
    {
        if (_previewTarget == null) return;
        _previewTarget.SetActive(true);
    }

    public void DeactivatePreviewTarget()
    {
        if(_previewTarget == null) return;
        _previewTarget.SetActive(false);
    }

    private void Update()
    {
        if(_collision)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0 && !_isShowed)
            {
                _onShow?.Invoke();
                _isShowed = true;
                _currentTime = -1f;
            }
        }
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out CharacterController2D controller2D))
        {
            _collision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterController2D controller2D))
        {
            _currentTime = _showDelay;
            _onHide?.Invoke();
            _isShowed = false;
            _collision = false;
        }
    }
}
