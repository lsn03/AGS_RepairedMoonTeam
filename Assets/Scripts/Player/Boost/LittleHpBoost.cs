using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleHpBoost : Boost
{
    [SerializeField] public float reloadTime;
    private float lostTime;
    

    public override void Use()
    {
        //Debug.Log( "Take little heart" );
    }
    private BoxCollider2D collider;

    private void Start()
    {
        lostTime = reloadTime;
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!itemGameObject.active)
        {
            if ( lostTime > 0 )
            {
                lostTime -= Time.deltaTime;
               // Debug.Log( lostTime );
            }
            else
            {
                lostTime = reloadTime;
                itemGameObject.SetActive( true );
                collider.enabled = true;
            }
        }
        
    }



    private void OnTriggerEnter2D( Collider2D collision )
    {
        PlayerController player =  collision.GetComponent<PlayerController>();
        if ( player != null )
        {
            Debug.Log( "OntriggerEnter" );
            player.gameObject.GetComponent<IAddHp>()?.AddHp( ( ( BoostInfo )itemInfo ).addHp );
            itemGameObject.SetActive( false );

            collider.enabled = false;
        }

    }

        
    
}
