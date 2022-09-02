using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Targets
{
    public Transform left;
    public Transform right;
    public GameObject gun;
}

public class TargetController : MonoBehaviour
{
    [SerializeField] Targets[] targetsInGun;
    [SerializeField] Transform leftTarget,rightTarget;
    //[SerializeField] GameObject[] gunsObject;
    private void Update()
    {
       foreach(var target in targetsInGun )
        {
            if ( target.gun.active )
            {
                leftTarget.position = target.left.transform.position;
                rightTarget.position = target.right.transform.position;
            }
        }
       
    }
}
