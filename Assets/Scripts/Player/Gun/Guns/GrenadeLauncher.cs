using Photon.Pun;
using Photon.Realtime;
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
    public float speed;
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

        photonView.RPC( nameof( RPC_Shoot ), RpcTarget.All, gameObject.transform.right * speed, PhotonNetwork.LocalPlayer );
        bulletsLeft--;
        yield return new WaitForSeconds(shotDelay);
        photonView.RPC( nameof( RPC_Shoot ), RpcTarget.All, gameObject.transform.right * speed, PhotonNetwork.LocalPlayer );
        bulletsLeft--;
        yield return new WaitForSeconds(shotDelay);
        photonView.RPC( nameof( RPC_Shoot ), RpcTarget.All, gameObject.transform.right * speed, PhotonNetwork.LocalPlayer );
        bulletsLeft--;
    }
    [PunRPC]
    public void RPC_Shoot( Vector3 vel, Player sender )
    {
        GameObject bul = Instantiate( bullet, bulletSpawn.position, bulletSpawn.transform.rotation );
        Rigidbody2D _rigidbody2D = bul.GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = vel;
        bul.GetComponent<BulletGrenade>().SetSender( sender.NickName.Split( '\t' )[0] );
    }
}
