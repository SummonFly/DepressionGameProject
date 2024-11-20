using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Animator))]
public class Solas: MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _minDistance = 0.5f;
    [SerializeField] private float _speed = 1f;

    private Animator m_animator;
    private bool _facingRight;

    public void InstantTeleport()
    {
        var position = transform.position;
        position.x = _target.position.x;
        position.y = _target.position.y;
        transform.position = position;
    }

    private void Start()
    {
        _facingRight = transform.localScale.x > 0f;
        m_animator = GetComponent<Animator>();
    }


    private void Facing()
    {
        var scale = transform.localScale;
        if(_facingRight)
        {
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }else
        {
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }
    }


    private void Update()
    {
        FollowTarget();
        Facing();
    }

    private void FollowTarget()
    {
        var targetPosition = new Vector3(_target.position.x, transform.position.y, transform.position.z);
        var distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget <= _minDistance)
        {
            m_animator.SetBool("Walk", false);
            return;
        }

        var position = transform.position;
        var direction = (targetPosition - transform.position).normalized;
        if(direction.x > 0)
        {
            _facingRight = true;
        }else if(direction.x < 0)
        {
            _facingRight = false;
        }
        position += (targetPosition - transform.position).normalized * _speed * Time.deltaTime;
        m_animator.SetBool("Walk", true);

        transform.position = position;
    }
    

}
