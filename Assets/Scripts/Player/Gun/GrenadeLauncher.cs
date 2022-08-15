using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrenadeLauncher : Gun
{
    public float offset;
    public GameObject bullet;
    public Transform bulletSpawn;
    public GameObject Gun;

    private PhotonView photonView;

    public TextMeshProUGUI text;

    [SerializeField, Range(0f, 15f)] public float bulletOffset;

    PlayerController player;
    [SerializeField] AudioSource sound;

    // Start is called before the first frame update
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

        PhotonNetwork.Instantiate(bullet.name, new Vector3(bulletSpawn.position.x, bulletSpawn.position.y + bulletOffset), bulletSpawn.transform.rotation);
        PhotonNetwork.Instantiate(bullet.name, bulletSpawn.position, bulletSpawn.transform.rotation);
        PhotonNetwork.Instantiate(bullet.name, new Vector3(bulletSpawn.position.x, bulletSpawn.position.y - bulletOffset), bulletSpawn.transform.rotation);
    }

    public void AddBullet(int addBullet)
    {
        SetAddBullet(addBullet);
    }
}
