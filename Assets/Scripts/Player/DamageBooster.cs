using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBooster : Boost
{
    [SerializeField] public float reloadTime;

    // Start is called before the first frame update
    void Start()
    {
        lostTime = reloadTime;
        collider = gameObject.GetComponent<BoxCollider2D>();
    }
    public override void Use()
    {
        
    }
    // Update is called once per frame
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
            health.gameObject.GetComponent<IDamageBooster>()?.SetPointDamageBooster( ( ( BoostInfo )itemInfo ).DamageBooster );
            itemGameObject.SetActive( false );

            collider.enabled = false;
        }

    }
}
