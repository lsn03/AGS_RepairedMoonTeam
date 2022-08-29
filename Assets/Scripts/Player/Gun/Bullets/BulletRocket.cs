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
    [SerializeField] PhotonView photonView;

    public float pushForce;

    public Rigidbody2D _rigidbody2D;

    [Range(0.1f, 5f), SerializeField] float splashRange;

    private void Update()
    {

        photonView = GetComponent<PhotonView>();


    }
    private void Start()
    {
        Invoke("DestroyBullet", destroyTime);
        
        _rigidbody2D.velocity = transform.right * speed;
    }

    void DestroyBullet()
    {
        try
        {
            if ( photonView.IsMine )
            {
                Instantiate( hitEffect, transform.position, Quaternion.identity );
                PhotonNetwork.Destroy( gameObject );

            }
        }
        catch(Exception ex )
        {
            Debug.Log( ex.Message );
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);

            foreach (var _hitCollider in hitColliders)
            {
                PlayerController Player = _hitCollider.GetComponent<PlayerController>();
                DestroyingPlatform platform = _hitCollider.GetComponent<DestroyingPlatform>();
                if (Player != null || platform!=null)
                {
                    //Debug.Log("enemy damaged");
                    try
                    {
                        if (photonView.IsMine)
                        {
                            //Debug.Log("transformPosition" + transform.position);
                            var closestPoint = _hitCollider.ClosestPoint(transform.position);
                            //Debug.Log("closestPoint" + closestPoint);
                            var distance = Vector2.Distance(closestPoint, transform.position);
                            //Debug.Log("distance" + distance);
                            var damagePercent = Mathf.InverseLerp(splashRange, 0, distance);
                            //Debug.Log("damagePercent"+ damagePercent);

                            Vector2 forseVector = transform.position;
                            forseVector -= closestPoint;
                            forseVector.Normalize();
                            //Debug.Log("forseVector" + forseVector);

                            Player?.gameObject.GetComponent<IDamage>()?.TakeDamage(((GunInfo)itemInfo).damage * damagePercent, photonView.Owner.NickName.Split('\t')[0]);
                            Player?.gameObject.transform.GetComponent<Rigidbody2D>().AddForce((-1) * pushForce * damagePercent * forseVector);

                            platform?.gameObject.GetComponent<IDamage>()?.TakeDamage( ( ( GunInfo )itemInfo ).damage, photonView.Owner.NickName.Split( '\t' )[0] );


                            //Debug.Log("final forse" + ((-1) * pushForce * damagePercent * forseVector));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log( ex.Message );
                    }

                }
            }
            DestroyBullet();
        }
    }
    // ����������
    // �������
    // ���������

}



