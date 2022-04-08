using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Range(0,10f),SerializeField] float jumpForce;
    [Range(0,10f),SerializeField] float speed;

    [SerializeField] public bool isGrounded;
    [SerializeField] public Transform GroundCheck;
    [SerializeField] private Text TextName;

    private PhotonView photonView;

    public LayerMask Ground;

    Rigidbody2D  _rigidbody2D;


    public float GroundCheckRadius;
    void Start()
    {
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        TextName.text = photonView.Owner.NickName;
        if (photonView.Owner.IsLocal)
            Camera.main.GetComponent<CameraWatchToPlayer>().player = gameObject.transform;
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
