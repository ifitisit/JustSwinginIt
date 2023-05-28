using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Autorun : MonoBehaviour
{
    public bool IsGrounded { get { return _isGrounded; } }
    public float CurrentSpeed { get { return _rigidBody.velocity.x; } }

    [Header("Base Movement")]
    [SerializeField]
    protected float _baseMaxSpeed = 15f;
    [SerializeField]
    protected float _baseMoveForce = 100f;
    [SerializeField]
    protected float _stopSlideSpeed = 0f;

    [Header("Ground Check")]
    [SerializeField]
    protected Transform _groundCheckTransform;
    [SerializeField]
    protected float _groundCheckRadius = 0.25f;
    [SerializeField]
    protected LayerMask _groundLayerMask;

    protected Rigidbody2D _rigidBody;
    protected bool _isGrounded = true;
    protected float _currentMaxSpeed;
    protected float _currentMoveForce;

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

        _rigidBody.velocity = new Vector2(_baseMaxSpeed, 0f);
    }

    protected void FixedUpdate()
    {
        GroundCheck();
        Move();
    }

    protected virtual void Move()
    {
        if (_rigidBody.velocity.x == -_stopSlideSpeed)
            _rigidBody.velocity = new Vector2(0f, _rigidBody.velocity.y);

        if (_isGrounded)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0);
        }
    }

    protected virtual void GroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheckTransform.position, _groundCheckRadius, _groundLayerMask);
    }
}
