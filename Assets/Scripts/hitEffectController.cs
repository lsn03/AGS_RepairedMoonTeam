using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitEffectController : MonoBehaviour
{

    public float destroyTime;
    private void Start()
    {
        Invoke( "DestroyHitEffect", destroyTime );
    }
    
    void DestroyHitEffect()
    {
        Destroy( gameObject );
    }
    
}
