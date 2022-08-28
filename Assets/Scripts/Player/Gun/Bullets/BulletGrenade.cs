using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGrenade : MonoBehaviour
{
    public ItemInfo itemInfo;
    public float speed;
    public float fallSpeed;
    public float destroyTime;

    public GameObject hitEffect;
    private PhotonView photonView;

    public Rigidbody2D _rigidbody2D;

    [Range(0.1f, 5f), SerializeField] float splashRange;


    private void Start()
    {
        Invoke("DestroyBullet", destroyTime);
        photonView = GetComponent<PhotonView>();
        _rigidbody2D.velocity = transform.right * speed;
    }

    private void Update()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y - fallSpeed * Time.deltaTime);
    }

    void DestroyBullet()
    {
        if (photonView.IsMine)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);

            foreach (var _hitCollider in hitColliders)
            {
                PlayerController Player = _hitCollider.GetComponent<PlayerController>();
                if (Player != null)
                {
                    Debug.Log("enemy damaged");
                    try
                    {
                        if (photonView.IsMine)
                        {
                            var closestPoint = _hitCollider.ClosestPoint(transform.position);
                            var distance = Vector3.Distance(closestPoint, transform.position);
                            var damagePercent = Mathf.InverseLerp(0, splashRange, distance);
                            Player.gameObject.GetComponent<IDamage>()?.TakeDamage(((GunInfo)itemInfo).damage, photonView.Owner.NickName.Split('\t')[0]);
                        }
                    }
                    catch (Exception ex)
                    {
                        return;
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



