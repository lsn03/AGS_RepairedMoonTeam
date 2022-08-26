using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RocketLauncher : Gun
{
    public float offset;
    public GameObject bullet;
    public Transform bulletSpawn;
    public GameObject Gun;

    private PhotonView photonView;

    public TextMeshProUGUI text;

    PlayerController player;
    [SerializeField] AudioSource sound;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (timeBeforeShoots <= 0)
        {
            if (Input.GetMouseButtonDown(0) && itemGameObject.active && bulletsLeft > 0)
            {
                Shoot();
                timeBeforeShoots = timeBetweenShoots;
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

    public override void Use()
    {

    }
    public void Shoot()
    {
        bulletsLeft--;
        sound.Play();
        PhotonNetwork.Instantiate(bullet.name, bulletSpawn.position, bulletSpawn.transform.rotation);
    }
}
