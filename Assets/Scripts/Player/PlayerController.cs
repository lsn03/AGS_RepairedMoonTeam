using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Range(0,10f),SerializeField] float jumpForce;
    [Range(0,10f),SerializeField] float speed;
    private float movement;

    [SerializeField] public bool isGrounded;
    [SerializeField] public Transform GroundCheck;
    public float GroundCheckRadius;
    public LayerMask Ground;

    [SerializeField] private Text TextName;

    [SerializeField] public Transform Spawn;
    public GameObject Gun;
    private PhotonView photonView;

    Rigidbody2D  _rigidbody2D;

    private float health = 100f;

    public bool facingRight = true;
    Vector3[] SpawnPoint;
    private void Awake()
    {
        SpawnPoint = new Vector3[Spawn.childCount];
        for ( int j = 0; j < Spawn.childCount; j++ )
            SpawnPoint[j] = Spawn.GetChild( j ).transform.position;
    }

    public void Start()
    {
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        TextName.text = photonView.Owner.NickName;
        if ( photonView.Owner.IsLocal )
        {
            Camera.main.GetComponent<CameraWatchToPlayer>().player = gameObject.transform;
        }
    }

    
    void Update()
    {
        if ( !photonView.IsMine ) return;
        Run();
        JumpUp();
        JumpDown();
        CheckingGround();
        Flip();
        Die();
    }

    void Run()
    {
        movement = Input.GetAxisRaw( "Horizontal" );
        _rigidbody2D.velocity = new Vector2( movement * speed, _rigidbody2D.velocity.y );
    }

    void JumpUp()
    {
        if ( isGrounded && ( Input.GetKey( KeyCode.Space ) || Input.GetKey( KeyCode.W ) ) )
        {
            _rigidbody2D.velocity = new Vector2( _rigidbody2D.velocity.x, jumpForce );
        }
    }
    void JumpDown()
    {
        if ( ( Input.GetKey( KeyCode.S ) ) )
        {
            _rigidbody2D.velocity = new Vector2( _rigidbody2D.velocity.x, -jumpForce );
        }
    }
    void CheckingGround()
    {
        isGrounded = Physics2D.OverlapCircle( GroundCheck.position, GroundCheckRadius, Ground );
    }

    public void TakeDamage( float damage )
    {
        health -= damage;
        Debug.Log( health );
        Die();
    }
    
    public void Die()
    {
        if ( health <= 0f || transform.position.y < -20f )
        {

            
            Vector2 _ = SpawnPoint[Random.Range(0,Spawn.childCount)];

            transform.position = new Vector2(_.x, _.y);
            health = 100f;
            Debug.Log( "Die" );
        }
    }
    public void Flip()
    {
        if ( movement < 0f && facingRight )
        {
            Spin();
        }
        else if ( movement > 0f && !facingRight )
        {
            Spin();
        }
    }

    public void Spin()

    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        transform.Rotate (0f,180f,0f);
        Gun.transform.position = new Vector3( Gun.transform.position .x, Gun.transform.position .y, Gun.transform.position.z*-1);
        TextName.GetComponent<RectTransform>().transform.Rotate( 0f, 180f, 0f );
    }
}
