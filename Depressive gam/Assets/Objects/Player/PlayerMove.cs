using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterController2D: MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private SpriteRenderer _playerSprite;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _croachSpeed;

    [SerializeField] private float _gravity;
    [SerializeField] private float _constGravity;
    [SerializeField] private float _jumpHight;
    [SerializeField] private float _jumpCooldown;

    private Rigidbody2D m_body;
    private CapsuleCollider2D m_collider;

    private bool _isGrounded;
    private bool _facingRight;
    private float _verticalVellosity;
    private bool _isRun = false;
    private Vector2 _moveDirection = Vector2.zero;

    void Start()
    {
        m_body = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CapsuleCollider2D>();
        _facingRight = transform.localScale.x > 0f;
    }

    public void OnMove(InputAction.CallbackContext context) => _moveDirection = context.ReadValue<Vector2>();
    public void OnRun(InputAction.CallbackContext context) => _isRun = context.ReadValue<float>() == 0? false:true;

    private void Move()
    {
        var speed = _isRun?_runSpeed: _walkSpeed;
        var direction = new Vector2(_moveDirection.x * speed, _verticalVellosity);
        m_body.AddForce(direction, ForceMode2D.Force);
        
        if(m_body.velocity.x > 0 && !_facingRight)
        {
            _facingRight=true;
        }else if(m_body.velocity.x < 0 && _facingRight)
        {
            _facingRight=false;
        }
    }

    private void Facing()
    {
        if(_facingRight)
        {
            _playerSprite.flipX = false;
        }else
        {
            _playerSprite.flipX = true;
        }
    }

    void Update()
    {
        Move();
        Facing();
    }

}
