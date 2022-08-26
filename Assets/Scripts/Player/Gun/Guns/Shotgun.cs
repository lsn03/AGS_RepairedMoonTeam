using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Shotgun : Gun
{
    public Transform bulletSpawn;

    public LineRenderer[] lineRenderer;
    public PhotonView photonView;

    public TextMeshProUGUI text;
    [SerializeField] AudioSource shootingSound;

    [Range(0, 0.5f), SerializeField] float waitForSeconds;
    [SerializeField] int bulletAmount;

    [SerializeField, Range(0f, 0.5f)] public float spread;
    float x;
    float y;

    public override void Use()
    {

    }
    private void Awake()
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
        bulletsLeft--;

        for (int i = 0; i < bulletAmount; i++)
        {
            x = Random.Range(-spread, spread);
            y = Random.Range(-spread, spread);
            
            RaycastHit2D hitInfo = Physics2D.Raycast(bulletSpawn.position, bulletSpawn.right + new Vector3(x, y, 0));

            if (hitInfo)
            {
                Instantiate(hitEffect, hitInfo.point, Quaternion.identity);
                Debug.Log(hitInfo.transform.name);
                if (photonView.IsMine)
                {
                    hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage(((GunInfo)itemInfo).damage, photonView.Owner.NickName.Split('\t')[0]);
                }
                lineRenderer[i].SetPosition(0, bulletSpawn.position);
                lineRenderer[i].SetPosition(1, hitInfo.point);
            }
            else
            {
                lineRenderer[i].SetPosition(0, bulletSpawn.position);
                lineRenderer[i].SetPosition(1, bulletSpawn.position + (bulletSpawn.right + new Vector3(x, y, 0)) * 50);
            }

            if (lineRenderer != null)
                lineRenderer[i].enabled = true;
        }

        yield return new WaitForSeconds(waitForSeconds);

        for (int i = 0; i < bulletAmount; i++)
        {
            if (lineRenderer != null)
                lineRenderer[i].enabled = false;
        }

        timeBeforeShoots = timeBetweenShoots;
    }
}
