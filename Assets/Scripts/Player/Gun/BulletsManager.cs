using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsManager : Item
{
    protected float lostTime;
    protected BoxCollider2D collider;
    public int bulletToAdd;
    [SerializeField] public float reloadTime;

    // Start is called before the first frame update
    void Start()
    {
        lostTime = reloadTime;
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Activation(reloadTime);
    }
    public override void Use()
    {

    }
    protected void Activation(float reloadTime)
    {
        if (!itemGameObject.active)
        {
            if (lostTime > 0)
            {
                lostTime -= Time.deltaTime;
                // Debug.Log( lostTime );
            }
            else
            {
                lostTime = reloadTime;
                itemGameObject.SetActive(true);
                collider.enabled = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            System.Type t = typeof(MachineGun);

            Debug.Log("getLazerBullets");
            Gun weapon = GetWeaponObject(player.gameObject);

            weapon.SetAddBullet(bulletToAdd);

            itemGameObject.SetActive(false);
            collider.enabled = false;
        }
    }

    Gun GetWeaponObject(GameObject go)
    {
        switch (itemInfo.itemName)
        {
            case "MachineGun":
                return go.GetComponentInChildren<MachineGun>();
            case "Shotgun":
                return go.GetComponentInChildren<Shotgun>();
            case "GrenadeLauncher":
                return go.GetComponentInChildren<GrenadeLauncher>();
            case "RocketLauncher":
                return go.GetComponentInChildren<RocketLauncher>();
            case "LaserGun":
                return go.GetComponentInChildren<LaserGun>();
            case "PlasmaRifle":
                return go.GetComponentInChildren<PlasmaRifle>();
            case "RailGun":
                return go.GetComponentInChildren<RailGun>();
            default:
                return go.GetComponentInChildren<Chainsaw>();
        }
        
    }

}
