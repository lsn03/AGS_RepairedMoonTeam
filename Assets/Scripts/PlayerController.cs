using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [Range(0,10f),SerializeField] float jumpForce;
    [Range(0,10f),SerializeField] float speed;

    [SerializeField] public bool isGrounded;
    [SerializeField] public Transform GroundCheck;

    private PhotonView photonView;

    public LayerMask Ground;

    Rigidbody2D  _rigidbody2D;


    public float GroundCheckRadius;
    void Start()
    {
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

    private float movement;
    void Update()
    {
        if ( !photonView.IsMine ) return;
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
