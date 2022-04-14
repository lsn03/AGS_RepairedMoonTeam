using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks,IDamage,IAddHp,IAddArmor
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

    float maxHP = 100f;
    float currentHP ;
    float maxArmor = 100f;
    float currentArmor = 0f;

    [SerializeField] Image healthBarImage;
    [SerializeField] Image armorBarImage;
    [SerializeField] GameObject ui;

    PlayerManager playerManager;
    private void Awake()
    {
        currentHP = maxHP;
    }

    public void Start()
    {
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int) photonView.InstantiationData[0]).GetComponent<PlayerManager>();

        TextName.text = photonView.Owner.NickName;
        if ( photonView.Owner.IsLocal )
        {
            Camera.main.GetComponent<CameraWatchToPlayer>().player = gameObject.transform;
        }
        if (! photonView.IsMine )
        {
            Destroy( ui );
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
        Flip();
        
        SwitchGunByButton();
        SwitchGunByScrollWheel();

        if ( Input.GetMouseButtonDown( 0 ) )
        {
            items[itemIndex].Use();
        }

        if ( transform.position.y < -15f )
        {
            Die();
        }
        if ( currentHP > maxHP )
        {
            currentHP -= Time.deltaTime;
        }
        if ( currentArmor > maxArmor )
        {
            currentArmor -= Time.deltaTime;
        }
        Debug.Log( currentHP + "\t" + currentArmor );
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

    public void AddArmor(float _littleArmor )
    {
        photonView.RPC( "RPC_AddArmor", RpcTarget.All, _littleArmor );
    }
    [PunRPC]
    void RPC_AddArmor(float _littleArmor)
    {
        if ( !photonView.IsMine )
        {
            return;
        }
        currentArmor = System.Math.Min( currentArmor + _littleArmor, maxArmor );
       
        armorBarImage.fillAmount = currentArmor / maxArmor;
        Debug.Log( "Current Armor is: " + currentArmor );
    }
    public void AddHp( float hp )
    {
        Debug.Log( "Addhp added external hp" );
        photonView.RPC( "RPC_AddHp", RpcTarget.All, hp );
    }
    public void TakeDamage( float damage )
    {
        Debug.Log( "took damage" + damage );

        photonView.RPC( "RPC_TakeDamage", RpcTarget.All, damage );
    }

    [PunRPC]
    void RPC_AddHp(float hp )
    {
        if ( !photonView.IsMine )
        {
            return;
        }
        currentHP = System.Math.Min( currentHP + hp, maxHP );
        //currentHP += hp;
        healthBarImage.fillAmount = currentHP / maxHP;
        Debug.Log( "RPC added " + hp + "\n currentHp = " +currentHP );
    }

    private float delta;
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if ( !photonView.IsMine )
        {
            return;
        }

        if ( currentArmor > 0 )
        {
            delta = currentArmor-damage;
            if ( delta > 0 )
            {
                currentArmor = delta;
            }
            else
            {
                currentArmor = 0;
                currentHP += delta;
            }
            
        }
        else
        {
            currentHP -= damage;
        }


        armorBarImage.fillAmount = currentArmor / maxArmor;
        healthBarImage.fillAmount = currentHP / maxHP;
        if ( currentHP <= 0 )
        {
            Die();
        }
        Debug.Log( "Took damage " + damage );
    }

    public void Die()
    {

        playerManager.Die();
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

        transform.Rotate( 0f, 180f, 0f );
        Gun.transform.position = new Vector3( Gun.transform.position.x, Gun.transform.position.y, Gun.transform.position.z * -1 );
        TextName.GetComponent<RectTransform>().transform.Rotate( 0f, 180f, 0f );
        ui.GetComponent<RectTransform>().transform.Rotate( 0f, 180f, 0f );
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

    //public override void OnPlayerPropertiesUpdate( Player targetPlayer, Hashtable changedProps )
    //{
    //    if(!photonView.IsMine && targetPlayer == photonView.Owner )
    //    {
            
    //        EquipIem( ( int )changedProps["itemIndex"] );
    //    }
    //}

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



}
