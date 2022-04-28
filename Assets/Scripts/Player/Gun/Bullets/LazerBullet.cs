using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBullet : BulletsManager
{
    [SerializeField] public float reloadTime;
    PhotonView photonView;
    public override void Use()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        lostTime = reloadTime;
        collider = gameObject.GetComponent<BoxCollider2D>();
        photonView = GetComponent<PhotonView>();
    }
    private void Update()
    {
        Activation( reloadTime );

    }
    private void OnTriggerEnter2D( Collider2D collision )
    {
        PlayerController player =  collision.GetComponent<PlayerController>();
        if ( player != null )
        {
            
                Debug.Log( "getLazerBullets" );
                player.gameObject.GetComponentInChildren<SingleShot>().AddBullet( bulletToAdd );
                //player.gameObject.GetComponent<IAddHp>()?.AddHp( ( ( BoostInfo )itemInfo ).addHp );
            
            itemGameObject.SetActive( false );
            collider.enabled = false;
        }

    }
}
