using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class CharacterController2D: MonoBehaviour
{
    [SerializeField] public bool _canMove = true;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    private Rigidbody2D m_body;
    private CapsuleCollider2D m_collider;
    private Animator m_animator;
    private AudioSource m_audioSource;
    private bool _isGrounded;
    private bool _facingRight;
    private bool _isRun = false;
    private Vector2 _moveDirection = Vector2.zero;

    void Start()
    {
        m_body = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CapsuleCollider2D>();
        m_audioSource = GetComponent<AudioSource>();
        _facingRight = transform.localScale.x > 0f;
        m_animator = GetComponent<Animator>();
    }

    public void CanMove(bool value)
    {
        _canMove = value;
    }

    public void OnMove(InputAction.CallbackContext context) => _moveDirection = context.ReadValue<Vector2>();
    public void OnRun(InputAction.CallbackContext context) => _isRun = context.ReadValue<float>() == 0? false:true;
    
    private void Move()
    {
        var speed = _isRun?_runSpeed:_walkSpeed;
        

        var verticalVellosity = m_body.velocity.y;

        m_body.velocity = new Vector2(_moveDirection.x * speed, verticalVellosity);


        if (m_body.velocity.x > 0 && !_facingRight)
        {
            _facingRight=true;
        }else if(m_body.velocity.x < 0 && _facingRight)
        {
            _facingRight=false;
        }

        var isMove = Mathf.Abs(m_body.velocity.x) >= 0.2f;
        m_animator.SetBool("Run", isMove &&  _isRun);
        m_animator.SetBool("Walk", isMove && _isGrounded);

        if(isMove)
        {
            if(!m_audioSource.isPlaying)
            {
                m_audioSource.Play();
            }
        }
        else
        {
            m_audioSource.Pause();
        }
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
        if(_isGrounded && _canMove)
        {
            Move();
            Facing();
        }
    }

}
