using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletsManager : Item
{
    public abstract override void Use();
    protected float lostTime;
    protected BoxCollider2D collider;
    public int bulletToAdd;
    protected void Activation( float reloadTime )
    {
        if ( !itemGameObject.active )
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

}
