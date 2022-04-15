using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleHpBoost : Boost
{
    [SerializeField] public float reloadTime;
    private float lostTime;
    private BoxCollider2D collider;

    public override void Use()
    {
        //Debug.Log( "Take little heart" );
    }
    

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
        HealthSystem health =  collision.GetComponent<HealthSystem>();
        if ( health != null )
        {
            Debug.Log( "OntriggerEnter" );
            health.gameObject.GetComponent<IAddHp>()?.AddHp( ( ( BoostInfo )itemInfo ).addHp );
            itemGameObject.SetActive( false );

            collider.enabled = false;
        }

    }

        
    
}
