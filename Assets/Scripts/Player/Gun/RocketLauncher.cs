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

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        player = GetComponent<PlayerController>();


    }

    // Update is called once per frame
    //void Update()
    //{
    //    if ( !photonView.IsMine ) return;
    //    Gun.SetActive( true );
    //    Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
    //    float rotateZ = Mathf.Atan2(difference.y,difference.x)*Mathf.Rad2Deg;

    //    Gun.transform.rotation = Quaternion.Euler( 0f, 0f, rotateZ + offset );


    //}

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

        else
        {
            text.gameObject.SetActive(false);
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

    public void AddBullet(int addBullet)
    {
        SetAddBullet(addBullet);
    }
}
