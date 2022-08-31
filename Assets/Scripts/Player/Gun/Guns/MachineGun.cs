using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MachineGun : Gun
{
    public Transform bulletSpawn;

    public LineRenderer lineRenderer;
    public PhotonView photonView;

    public TextMeshProUGUI text;

    AudioSource shootingSound;
    [SerializeField, Range(0f, 0.5f)] public float spread;
    float x;
    float y;
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
                x = Random.Range(-spread, spread);
                y = Random.Range(-spread, spread);

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
        }
    }


    [PunRPC]
    IEnumerator ShootAuto()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(bulletSpawn.position, bulletSpawn.right + new Vector3(x, y, 0));
        bulletsLeft--;
        if (hitInfo)
        {
            hitEffectController hit = Instantiate(hitEffect, hitInfo.point, Quaternion.identity).GetComponent<hitEffectController>();
            Debug.Log(hitInfo.transform.name);
            if (photonView.IsMine)
            {
                //Debug.Log( "Popal" );
                if( hitInfo.collider.gameObject.GetComponent<IDamage>() != null )
                {
                    hit.ShowDamage( ( ( GunInfo )itemInfo ).damage );
                }
                else
                {
                    hit.ShowDamage( );
                }
                hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage(((GunInfo)itemInfo).damage, photonView.Owner.NickName.Split('\t')[0], "MachineGun"  );
                
            }
            lineRenderer.SetPosition(0, bulletSpawn.position);
            lineRenderer.SetPosition(1, hitInfo.point);

        }
        else
        {
            lineRenderer.SetPosition(0, bulletSpawn.position);
            lineRenderer.SetPosition(1, bulletSpawn.position + (bulletSpawn.right + new Vector3(x, y, 0)) * 50);
        }
        if (lineRenderer != null)
            lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }
}
