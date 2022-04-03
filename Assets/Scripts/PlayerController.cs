using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0,10f),SerializeField] float jumpForce;
    [Range(0,10f),SerializeField] float speed;

    [SerializeField]public bool isGrounded;
   [SerializeField]  public Transform GroundCheck;


    public LayerMask Ground;

    Rigidbody2D  _rigidbody2D;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public float GroundCheckRadius;
    void Start()
    {
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
    }

    private float movement;
    void Update()
    {

        Run();
        Jump();
        CheckingGround();
        
    }
    void Run()
    {
        movement = Input.GetAxisRaw( "Horizontal" );
        _rigidbody2D.velocity = new Vector2( movement * speed, _rigidbody2D.velocity.y );
    }

    void Jump()
    {
        if ( isGrounded && Input.GetKey( KeyCode.Space ) )
        {
            _rigidbody2D.velocity = new Vector2( _rigidbody2D.velocity.x, jumpForce );
        }
    }
    void CheckingGround()
    {
        isGrounded = Physics2D.OverlapCircle( GroundCheck.position, GroundCheckRadius, Ground );
    }
}
