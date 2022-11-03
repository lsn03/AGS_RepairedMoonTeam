using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRocket : MonoBehaviour
{
    public ItemInfo itemInfo;
    public float speed;
    public float destroyTime;

    public GameObject hitEffect;
   

    public float pushForce;

    public Rigidbody2D _rigidbody2D;

    [Range(0.1f, 5f), SerializeField] float splashRange;


    private void Start()
    {
        Invoke("DestroyBullet", destroyTime);
        
        _rigidbody2D.velocity = transform.right * speed;
    }
    public void SetSender( string name )
    {
        this.name = name;
    }
    private string name;


    void DestroyBullet()
    {
       Destroy( gameObject );
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);

            foreach (var _hitCollider in hitColliders)
            {
                var obj = _hitCollider.GetComponent<IDamage>();

               
                if (obj!=null)
                {
                    var closestPoint = _hitCollider.ClosestPoint(transform.position);
                      
                    var distance = Vector2.Distance(closestPoint, transform.position);

                    Debug.Log(distance);
                    distance -= 0.7f;

                    if (distance < 0) distance = 0;

                    var damagePercent = Mathf.InverseLerp(splashRange - 0.7f, 0, distance);

                    int finalDamage = Convert.ToInt32 ( Math.Ceiling ((( GunInfo ) itemInfo ).damage * damagePercent ));
                    Vector2 forseVector = transform.position;
                    forseVector -= closestPoint;
                    forseVector.Normalize();


                    obj.TakeDamage(finalDamage, name,nameof(RocketLauncher));
                    PlayerController player = _hitCollider.GetComponent<PlayerController>();
                    if ( player != null ) 
                        player.gameObject.transform.GetComponent<Rigidbody2D>().AddForce((-1) * pushForce * damagePercent * forseVector);
                   
                    hitEffectController hitText = Instantiate(hitEffect, _hitCollider.transform.position, Quaternion.identity).GetComponent<hitEffectController>();
                    //переделать!!!!
                    hitText.ShowDamage(finalDamage);

                    
                }
                hitEffectController hitPaticle = Instantiate(hitEffect, transform.position, Quaternion.identity).GetComponent<hitEffectController>();
            }
            DestroyBullet();
        }
    }
    // ����������
    // �������
    // ���������

}



