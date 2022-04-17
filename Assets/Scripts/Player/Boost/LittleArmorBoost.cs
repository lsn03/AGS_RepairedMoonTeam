using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleArmorBoost : Boost
{
    [SerializeField] float reloadTime;
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
        Activation( reloadTime );
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
