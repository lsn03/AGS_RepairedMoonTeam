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
    [Range(0, 10f), SerializeField] float jumpForce;
    [Range(0, 150f), SerializeField] float runSpeed;
    [Range(0, 20f), SerializeField] float fallSpeed;
    private float movement;

    [Range(0, 10f), SerializeField] float maxRunSpeed;
    [Range(0, 25f), SerializeField] float maxFallSpeed;

    [SerializeField] public bool isGrounded;
    [SerializeField] public Transform GroundCheck;

    public LayerMask Ground;

    public float teleportTimer;

    [SerializeField] private Text TextName;

    public GameObject Gun;
    private PhotonView photonView;

    Rigidbody2D _rigidbody2D;

    public bool facingRight = true;


    [SerializeField] Item[] items;
    int itemIndex;

    int previousItemIndex = -1;

    [Range(0, 2.5f), SerializeField] float wheelWeaponChangeTime;
    public float wheelWeaponChangeTimer;


    [SerializeField] GameObject ui;

    [SerializeField] GameObject Custom;

    PlayerManager playerManager;
    Animator _animator;

    [SerializeField] AudioSource runSound;

    string[] arrayNick;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
        _animator = GetComponent<Animator>();

        var text = photonView.Owner.NickName;
        arrayNick = text.Split('\t');
        TextName.text = arrayNick[0];
        Custom.GetComponent<SpriteRenderer>().color = new Color(float.Parse(arrayNick[1]), float.Parse(arrayNick[2]), float.Parse(arrayNick[3]));

        if (photonView.Owner.IsLocal)
        {
            Camera.main.GetComponent<CameraWatchToPlayer>().player = gameObject.transform;
        }

        EquipItem(0);
        wheelWeaponChangeTimer = 0;
    }


    void Update()
    {
        if (!photonView.IsMine) return;
        Run();
        JumpUp();
        JumpDown();
        CheckingGround();

        SwitchGunByButton();
        SwitchGunByScrollWheel();

        if (wheelWeaponChangeTimer > 0)
            wheelWeaponChangeTimer -= Time.deltaTime;

        if (teleportTimer > 0)
            teleportTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }

        if (_rigidbody2D.velocity.y < -maxFallSpeed)
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -maxFallSpeed);
    }

    void Run()
    {
        movement = Input.GetAxisRaw("Horizontal");

        int side;
        if (_rigidbody2D.velocity.x >= 0)
            side = 1;
        else
            side = -1;

        if (movement == 0)
        {
            if (isGrounded && _rigidbody2D.velocity.x != 0)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x - runSpeed * Time.deltaTime * side * maxRunSpeed / 1.5f, _rigidbody2D.velocity.y);

                if ((side == 1 && _rigidbody2D.velocity.x < 0)
                    || (side == -1 && _rigidbody2D.velocity.x > 0))
                    _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            }
        }
        else
        {
            if (movement > 0 && _rigidbody2D.velocity.x < maxRunSpeed)
                _rigidbody2D.velocity = new Vector2(Mathf.Min(maxRunSpeed, _rigidbody2D.velocity.x + movement * runSpeed * Time.deltaTime), _rigidbody2D.velocity.y);
            else if (movement < 0 && _rigidbody2D.velocity.x > -maxRunSpeed)
                _rigidbody2D.velocity = new Vector2(Mathf.Max(-maxRunSpeed, _rigidbody2D.velocity.x + movement * runSpeed * Time.deltaTime), _rigidbody2D.velocity.y);
        }

        if (!runSound.isPlaying && Mathf.Abs(movement) >= 0.01f)
        {
            runSound.Play();
        }

        if (_animator)
        {
            _animator.SetBool("isRun", Mathf.Abs(movement) >= 0.01f);
        }
    }

    void JumpUp()
    {
        if (isGrounded && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && _rigidbody2D.velocity.y < jumpForce)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        }
    }
    void JumpDown()
    {
        if ((Input.GetKey(KeyCode.S)))
        {
            if (_rigidbody2D.velocity.y > -maxFallSpeed)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y - fallSpeed * Time.deltaTime);
                if (_rigidbody2D.velocity.y < -maxFallSpeed)
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -maxFallSpeed);
            }

        }
    }
    void CheckingGround()
    {
        ContactFilter2D _ContactFilter = new ContactFilter2D();
        _ContactFilter.SetLayerMask(Ground);
        List<Collider2D> results = new List<Collider2D>();
        isGrounded = Physics2D.OverlapCollider(GroundCheck.GetComponent<EdgeCollider2D>(), _ContactFilter, results) > 0;
    }

    bool EquipItem(int _index)
    {
        if (_index == previousItemIndex) return false;
        if (items[_index].GetComponent<Gun>().bulletsLeft == 0 && _index != 0) return false;

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;
        if (photonView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        return true;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("itemIndex") && !photonView.IsMine && targetPlayer == photonView.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    void SwitchGunByButton()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);

                break;
            }
        }
    }
    void SwitchGunByScrollWheel()
    {
        if (wheelWeaponChangeTimer > 0) return;

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            int newItemIndex;
            if (itemIndex >= items.Length - 1)
            {
                newItemIndex = 0;
            }
            else
            {
                newItemIndex = itemIndex + 1;
            }
            while (!EquipItem(newItemIndex))
            {
                if (newItemIndex >= items.Length - 1)
                {
                    newItemIndex = 0;
                }
                else
                {
                    newItemIndex++;
                }
            }                           
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            int newItemIndex; 
            if (itemIndex <= 0)
            {
                newItemIndex = items.Length - 1;
            }
            else
            {
                newItemIndex = itemIndex - 1;
            }
            while (!EquipItem(newItemIndex))
            {
                if (newItemIndex <= 0)
                {
                    newItemIndex = items.Length - 1;
                }
                else
                {
                    newItemIndex--;
                }
            } 
        }

        wheelWeaponChangeTimer = wheelWeaponChangeTime;
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

