using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingPlatform : Boost, IDamage
{
    PhotonView photonView;
    [SerializeField] private float reloadTime;
    public float maxHp = 100f,currentHp;

    public void TakeDamage( float damage, string name, string weapon = null)
    {
        Debug.Log( "took damage" + damage );
        photonView.RPC( nameof( RPC_TakeDamage ), RpcTarget.All, damage, name );
    }
    [PunRPC]
    public void RPC_TakeDamage( float damage, string name )
    {

        currentHp -= damage;

        if ( currentHp <= 0 )
        {
            DieObject();
        }
    }

    private void DieObject()
    {
        itemGameObject.SetActive( false );
        collider.enabled = false;
        currentHp = maxHp;
       
    }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        currentHp = maxHp;
        lostTime = reloadTime;
        collider = gameObject.GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log( currentHp );
        Activation( reloadTime );
    }

    public override void Use()
    {
      
    }
}
