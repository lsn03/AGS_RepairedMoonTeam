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

    [SerializeField, Range(0.2f, 1f)] public float shotDelay;

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
                StartCoroutine(Shoot());
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
    IEnumerator Shoot()
    {
        
        sound.Play();

        PhotonNetwork.Instantiate(bullet.name, bulletSpawn.position, bulletSpawn.transform.rotation);
        bulletsLeft--;
        yield return new WaitForSeconds(shotDelay);
        PhotonNetwork.Instantiate(bullet.name, bulletSpawn.position, bulletSpawn.transform.rotation);
        bulletsLeft--;
        yield return new WaitForSeconds(shotDelay);
        PhotonNetwork.Instantiate(bullet.name, bulletSpawn.position, bulletSpawn.transform.rotation);
        bulletsLeft--;
    }
}
