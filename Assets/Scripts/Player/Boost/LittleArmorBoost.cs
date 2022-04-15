using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleArmorBoost : Boost
{
    [SerializeField] float reloadTime;
    private float lostTime;
    private BoxCollider2D collider;
    public override void Use()
    {
        
    }
    void Start()
    {
        lostTime = reloadTime;
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        if ( !itemGameObject.active )
        {
            if ( lostTime > 0 )
            {
                lostTime -= Time.deltaTime;
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
            health.gameObject.GetComponent<IAddArmor>()?.AddArmor( ( ( BoostInfo )itemInfo ).addArmor );
            itemGameObject.SetActive( false );

            collider.enabled = false;
        }

    }

}
