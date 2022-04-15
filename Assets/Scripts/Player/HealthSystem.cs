using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class HealthSystem : MonoBehaviourPunCallbacks, IDamage, IAddHp, IAddArmor
{
    const float maxHP = 100f;
    float currentHP ;
    const float maxArmor = 100f;
    float currentArmor = 0f;

    [SerializeField] Image healthBarImage;
    [SerializeField] Image armorBarImage;
    [SerializeField] GameObject ui;

    PlayerManager playerManager;
    PhotonView photonView;
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
    }
    private void Update()
    {
        if ( transform.position.y < -15f )
        {
            Die();
        }
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
    void RPC_AddHp( float hp )
    {
        if ( !photonView.IsMine )
        {
            return;
        }
        currentHP = System.Math.Min( currentHP + hp, maxHP );

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
