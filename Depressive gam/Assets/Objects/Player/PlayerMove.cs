using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent (typeof(Animator))]
public class CharacterController2D: MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _croachSpeed;
    [SerializeField] private float _jumpFoce;

    private Rigidbody2D m_body;
    private CapsuleCollider2D m_collider;
    private Animator m_animator;

    private bool _isGrounded;
    private bool _facingRight;
    private bool _isRun = false;
    private bool _doJump = false;
    private Vector2 _moveDirection = Vector2.zero;

    void Start()
    {
        m_body = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CapsuleCollider2D>();
        _facingRight = transform.localScale.x > 0f;
        m_animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context) => _moveDirection = context.ReadValue<Vector2>();
    public void OnRun(InputAction.CallbackContext context) => _isRun = context.ReadValue<float>() == 0? false:true;
    public void OnJump(InputAction.CallbackContext context) => _doJump = context.ReadValue<float>() == 0 ? false : true;

    private void Move()
    {
        var speed = _isRun?_runSpeed:_walkSpeed;
        m_animator.SetBool("Run", _isRun);

        var verticalVellosity = m_body.velocity.y;
        if(_doJump)
        {
            verticalVellosity = _jumpFoce;
            _doJump = false;
        }

        m_body.velocity = new Vector2(_moveDirection.x * speed, verticalVellosity);


        if (m_body.velocity.x > 0 && !_facingRight)
        {
            _facingRight=true;
        }else if(m_body.velocity.x < 0 && _facingRight)
        {
            _facingRight=false;
        }

        m_animator.SetBool("Walk", Mathf.Abs(m_body.velocity.x) >= 0.2f && _isGrounded);
    }

    private void Facing()
    {
        var scale = transform.localScale;
        if(_facingRight)
        {
            transform.localScale = new Vector3(1, scale.y, scale.z);
        }else
        {
            transform.localScale = new Vector3(-1, scale.y, scale.z);
        }
    }

    private void UpdateGroundState()
    {
        var colliderBounds = m_collider.bounds;
        float colliderRadius = m_collider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        _isGrounded = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius, _groundLayer).Length > 0;
    }

    void FixedUpdate()
    {
        UpdateGroundState();
        if(_isGrounded)
        {
            Move();
            Facing();
        }
    }

}
