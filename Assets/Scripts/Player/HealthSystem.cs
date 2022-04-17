using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class HealthSystem : MonoBehaviourPunCallbacks, IDamage, IAddHp, IAddArmor,IDamageBooster
{
    const float maxHP = 100f;
    float currentHP ;
    const float maxArmor = 100f;
    float currentArmor = 0f;

    [SerializeField] Image healthBarImage;
    [SerializeField] Image armorBarImage;
    [SerializeField] GameObject ui;

    [SerializeField] Text currentHpText;
    [SerializeField] Text currentArmorText;

    PlayerManager playerManager;
    PhotonView photonView;

    float damageBooster = 1f;
    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerManager = PhotonView.Find( ( int )photonView.InstantiationData[0] ).GetComponent<PlayerManager>();
        if ( !photonView.IsMine )
        {
            Destroy( ui );
        }
        currentHpText.text = currentHP.ToString();
        currentArmorText.text = currentArmor.ToString();
    }
    int cntPlayer;
    private void Update()
    {
        if ( !photonView.IsMine ) return;
        
        cntPlayer =  PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public void SetPointDamageBooster(float point )
    {
        photonView.RPC( "RPC_SetPointDamageBooster", RpcTarget.All, point ); 
    }

    [PunRPC]
    void RPC_SetPointDamageBooster(float point )
    {
        if ( !photonView.IsMine )
        {
            return;
        }
        damageBooster = point;
    }
    public void AddArmor( float _littleArmor )
    {
        photonView.RPC( "RPC_AddArmor", RpcTarget.All, _littleArmor );
    }
    [PunRPC]
    void RPC_AddArmor( float _littleArmor )
    {
        if ( !photonView.IsMine )
        {
            return;
        }
        _littleArmor /= cntPlayer;
        
        currentArmor = System.Math.Min( (currentArmor + _littleArmor), maxArmor );
       
        currentArmorText.text = currentArmor.ToString();
        
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
    void RPC_AddHp( float hp )
    {
        hp /= cntPlayer;
        if ( !photonView.IsMine )
        {
            return;
        }
        currentHP = System.Math.Min( (currentHP + hp), maxHP );
        currentHpText.text = currentHP.ToString();
        healthBarImage.fillAmount = currentHP / maxHP;
        Debug.Log( "RPC added " + hp + "\n currentHp = " + currentHP );
    }

    private float delta;


    [PunRPC]
    void RPC_TakeDamage( float damage )
    {
        if ( !photonView.IsMine )
        {
            return;
        }
        damage /= cntPlayer;
        damage *= damageBooster;
        if ( currentArmor > 0 )
        {
            delta = currentArmor - damage;
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

        currentArmorText.text = currentArmor.ToString();
        currentHpText.text = currentHP.ToString();

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
}
