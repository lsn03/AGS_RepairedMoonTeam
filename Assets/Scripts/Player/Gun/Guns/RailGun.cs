using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RailGun : Gun
{
    [SerializeField] Camera cam;
    public Transform bulletSpawn;

    public LineRenderer lineRenderer;
    public PhotonView photonView;

    public TextMeshProUGUI text;
    [SerializeField] AudioSource shootingSound;

    [Range(0, 0.5f), SerializeField] float waitForSeconds;
    public override void Use()
    {

    }

    void Start()
    {
        shootingSound = GetComponent<AudioSource>();
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (timeBeforeShoots <= 0)
        {
            if (Input.GetMouseButtonDown(0) && itemGameObject.active && bulletsLeft > 0)
            {
                timeBeforeShoots = timeBetweenShoots;
                shootingSound.Play();
                photonView.RPC("Shoot", RpcTarget.All);

            }
        }
        else
        {
            timeBeforeShoots -= Time.deltaTime;
        }

        if (itemGameObject.active)
        {
            text.gameObject.SetActive(true);
            text.SetText(bulletsLeft + " / " + maxBullets);
        }
    }

    [PunRPC]
    IEnumerator Shoot()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(bulletSpawn.position, bulletSpawn.right);
        
        bulletsLeft--;
        if (hitInfo)
        {
            Instantiate(hitEffect, hitInfo.point, Quaternion.identity);
            Debug.Log(hitInfo.transform.name);
            if (photonView.IsMine)
            {
                hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage(((GunInfo)itemInfo).damage, photonView.Owner.NickName.Split('\t')[0]);
            }
            lineRenderer.SetPosition(0, bulletSpawn.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, bulletSpawn.position);
            lineRenderer.SetPosition(1, bulletSpawn.position + bulletSpawn.right * 50);
        }


        timeBeforeShoots = timeBetweenShoots;
        if (lineRenderer != null)
            lineRenderer.enabled = true;
        yield return new WaitForSeconds(waitForSeconds);
        if (lineRenderer != null)
            lineRenderer.enabled = false;

    }


    public void AddBullet(int addBullet)
    {
        SetAddBullet(addBullet);
    }



}