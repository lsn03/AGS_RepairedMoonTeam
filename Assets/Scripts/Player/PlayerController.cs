using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using System.Text;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [Range(0,10f),SerializeField] float jumpForce;
    [Range(0,10f),SerializeField] float speed;
    private float movement;

    [SerializeField] public bool isGrounded;
    [SerializeField] public Transform GroundCheck;
    public float GroundCheckRadius;
    public LayerMask Ground;

    [SerializeField] private Text TextName;

    public GameObject Gun;
    private PhotonView photonView;

    Rigidbody2D  _rigidbody2D;

    public bool facingRight = true;
    

    [SerializeField] Item[] items;
     int itemIndex;
 
    int previousItemIndex=-1;

    [SerializeField] GameObject ui;

    [SerializeField] GameObject Custom;

    PlayerManager playerManager;
    Animator _animator;

    [SerializeField] AudioSource runSound;

    string [] arrayNick;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void Start()
    {
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        playerManager = PhotonView.Find((int) photonView.InstantiationData[0]).GetComponent<PlayerManager>();
        _animator = GetComponent<Animator>();
        
        var text = photonView.Owner.NickName;
        arrayNick = text.Split('\t');
        TextName.text = arrayNick[0];
        Custom.GetComponent<SpriteRenderer>().color = new Color( float.Parse( arrayNick[1]), float.Parse( arrayNick[2] ), float.Parse( arrayNick[3] ) );
        

        if ( photonView.Owner.IsLocal )
        {
            Camera.main.GetComponent<CameraWatchToPlayer>().player = gameObject.transform;
        }
        

        EquipIem( 0 );
        
    }


    void Update()
    {
        if ( !photonView.IsMine ) return;
        Run();
        JumpUp();
        JumpDown();
        CheckingGround();
        //Flip();
        
        SwitchGunByButton();
        SwitchGunByScrollWheel();

        if ( Input.GetMouseButtonDown( 0 ) )
        {
            items[itemIndex].Use();
        }

        
        
    }

    void Run()
    {
        movement = Input.GetAxisRaw( "Horizontal" );

        
        int side;
        if (_rigidbody2D.velocity.x >= 0)
            side = 1;
        else
            side = -1;

        if (movement == 0)
        {
            if (isGrounded && _rigidbody2D.velocity.x != 0)
            {
                

                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x - speed * Time.deltaTime * side, _rigidbody2D.velocity.y);

                if ((side == 1 && _rigidbody2D.velocity.x < 0)
                    || (side == -1 && _rigidbody2D.velocity.x > 0))
                    _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);

            }
        }
        else
        {
            float _VelocityX;
            if (movement > 0)
                _VelocityX = Mathf.Min(movement * speed, _rigidbody2D.velocity.x + movement * speed * Time.deltaTime * 7.5f);
            else
                _VelocityX = Mathf.Max(movement * speed, _rigidbody2D.velocity.x + movement * speed * Time.deltaTime * 7.5f);

            Debug.Log(_VelocityX);


            _rigidbody2D.velocity = new Vector2(_VelocityX, _rigidbody2D.velocity.y);
        }
        
        if(!runSound.isPlaying && Mathf.Abs( movement ) >= 0.01f  )
        {
            runSound.Play();
        }

        if ( _animator )
        {
            _animator.SetBool( "isRun", Mathf.Abs( movement ) >= 0.01f );
        }
        
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

    void EquipIem( int _index )
    {
        if ( _index == previousItemIndex )
        {
            return;
        }

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive( true );
        
        if ( previousItemIndex != -1 )
        {
            items[previousItemIndex].itemGameObject.SetActive( false );
        }

        previousItemIndex = itemIndex;
        if ( photonView.IsMine )
        {
            Hashtable hash = new Hashtable();
            hash.Add( "itemIndex", itemIndex );
            PhotonNetwork.LocalPlayer.SetCustomProperties( hash );
        }
        
    }

    public override void OnPlayerPropertiesUpdate( Player targetPlayer, Hashtable changedProps )
    {
        if (changedProps.ContainsKey("itemIndex")&& !photonView.IsMine && targetPlayer == photonView.Owner )
        {

            EquipIem( ( int )changedProps["itemIndex"] );
        }
    }

    void SwitchGunByButton()
    {
        for ( int i = 0; i < items.Length; i++ )
        {
            if ( Input.GetKeyDown( ( i + 1 ).ToString() ) )
            {
                EquipIem( i );
               
                break;
            }
        }
    }
    void SwitchGunByScrollWheel()
    {
        if ( Input.GetAxisRaw( "Mouse ScrollWheel" ) > 0f )
        {
            if ( itemIndex >= items.Length - 1 )
            {
                EquipIem( 0 );
               
            }
            else
            {
                EquipIem( itemIndex + 1 );
              
            }

        }
        else if ( Input.GetAxisRaw( "Mouse ScrollWheel" ) < 0f )
        {
            if ( itemIndex <= 0 )
            {
                EquipIem( items.Length - 1 );
              
            }
            else
            {
                EquipIem( itemIndex - 1 );
               
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("lift"))
        {
            this.transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("lift"))
        {
            this.transform.parent = null;
        }
    }

}

