using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaserGun : Gun
{
    public Transform bulletSpawn;

    public LineRenderer lineRenderer;
    public PhotonView photonView;

    public TextMeshProUGUI text;

    AudioSource shootingSound;
    public override void Use()
    {

    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        shootingSound = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        if (!photonView.IsMine) return;
        if (timeBeforeShoots <= 0)
        {
            if (Input.GetMouseButton(0) && itemGameObject.active && bulletsLeft > 0)
            {
                timeBeforeShoots = timeBetweenShoots;
                
                if (!shootingSound.isPlaying)
                    shootingSound.Play();

                photonView.RPC("ShootAuto", RpcTarget.All);
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
            if (!Input.GetMouseButton(0))
            {
                if (lineRenderer != null)
                lineRenderer.enabled = false;
            }
        }
    }


    [PunRPC]
    IEnumerator ShootAuto()
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
        if (lineRenderer != null)
            lineRenderer.enabled = true;
        yield return new WaitForSeconds(0);        
    }
}
