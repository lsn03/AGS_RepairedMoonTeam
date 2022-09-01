using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class HealthSystem : MonoBehaviourPunCallbacks, IDamage, IAddHp, IAddArmor, IDamageBooster
{
    const float maxHP = 100f;
    float currentHP;
    const float maxArmor = 50f;
    float currentArmor = 0f;

    [SerializeField] Image healthBarImage;
    [SerializeField] Image armorBarImage;
    [SerializeField] GameObject ui;

    [SerializeField] Text currentHpText;
    [SerializeField] Text currentArmorText;

    [SerializeField] AudioSource armorSound;
    [SerializeField] AudioSource hpSound;
    [SerializeField] AudioSource deathSound;

    PlayerManager playerManager;
    PhotonView photonView;

    float damageBooster = 1f;
    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
        if (!photonView.IsMine)
        {
            Destroy(ui);
        }
        currentHpText.text = currentHP.ToString();
        currentArmorText.text = currentArmor.ToString();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
    }

    public void SetPointDamageBooster(float point)
    {
        photonView.RPC("RPC_SetPointDamageBooster", RpcTarget.All, point);
    }

    [PunRPC]
    void RPC_SetPointDamageBooster(float point)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        damageBooster = point;
    }
    public void AddArmor(float _littleArmor)
    {
        photonView.RPC("RPC_AddArmor", RpcTarget.All, _littleArmor);
    }
    [PunRPC]
    void RPC_AddArmor(float _littleArmor)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //_littleArmor /= cntPlayer;
        armorSound.Play();
        currentArmor = System.Math.Min((currentArmor + _littleArmor), maxArmor);

        currentArmorText.text = currentArmor.ToString();

        armorBarImage.fillAmount = currentArmor / maxArmor;
        Debug.Log("Current Armor is: " + currentArmor);
    }
    public void AddHp(float hp)
    {
        Debug.Log("Addhp added external hp");
        photonView.RPC("RPC_AddHp", RpcTarget.All, hp);
    }
    public void TakeDamage( float damage, string name, string weapon  )
    {
        Debug.Log("took damage" + damage);

        photonView.RPC("RPC_TakeDamage", photonView.Owner, damage, name ,weapon);
    }

    [PunRPC]
    void RPC_AddHp(float hp)
    {
        //hp /= cntPlayer;
        if (!photonView.IsMine)
        {
            return;
        }
        hpSound.Play();
        currentHP = System.Math.Min((currentHP + hp), maxHP);
        currentHpText.text = currentHP.ToString();
        healthBarImage.fillAmount = currentHP / maxHP;
        Debug.Log("RPC added " + hp + "\n currentHp = " + currentHP);
    }

    private float delta;

    [PunRPC]

    void RPC_TakeDamage(float damage, string autor, string weapon, PhotonMessageInfo info )
    {
        if (!photonView.IsMine) return;
        if (autor == photonView.Owner.NickName.Split('\t')[0] || info.Sender == PhotonNetwork.LocalPlayer)
        {
            return;
        }
        //damage /= cntPlayer;
        damage *= damageBooster;
        if (currentArmor > 0)
        {
            delta = currentArmor - damage;
            if (delta > 0)
            {
                currentArmor = delta;
            }
            else
            {
                currentArmor = 0;
                currentHP += delta;
            }

        }
        else
        {
            currentHP -= damage;
        }

        currentArmorText.text = currentArmor.ToString();
        currentHpText.text = currentHP.ToString();
        Debug.Log( $"sender is: \t{info.Sender}, currentHp = {currentHP}" );
        armorBarImage.fillAmount = currentArmor / maxArmor;
        healthBarImage.fillAmount = currentHP / maxHP;
        if (currentHP <= 0)
        {
            Debug.Log($"\t\tsender is: \t{info.Sender}");
            PlayerManager.Find(info.Sender).GetKill(PhotonNetwork.LocalPlayer);
            Die( info.Sender, weapon );

        }
        Debug.Log("Took damage " + damage);
    }
    public void Die( Player player, string weapon   )
    {
        deathSound.Play();
        playerManager.Die(player,weapon);
    }
    public void Die(  )
    {
        deathSound.Play();
        playerManager.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "spikes" && photonView.IsMine)
        {
            Die();
        }
    }
}