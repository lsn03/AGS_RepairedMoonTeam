using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsManager : Item
{
    public float reloadTimer;
    protected BoxCollider2D collider;
    public int bulletToAdd;
    [SerializeField] public float reloadTime;

    // Start is called before the first frame update
    void Start()
    {
        reloadTimer = reloadTime;
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!itemGameObject.activeSelf)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
            }
            else
            {
                reloadTimer = reloadTime;
                itemGameObject.SetActive(true);
                collider.enabled = true;
            }
        }
    }
    public override void Use()
    {

    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        PlayerController player = _collision.GetComponent<PlayerController>();
        if (player != null)
        {
            Gun weapon = GetWeaponObject(player.gameObject);

            weapon.AddBullet(bulletToAdd);

            itemGameObject.SetActive(false);
            collider.enabled = false;
        }
    }

    Gun GetWeaponObject(GameObject _gameObject)
    {
        switch (itemInfo.itemName)
        {
            case "MachineGun":
                return _gameObject.GetComponentInChildren<MachineGun>();
            case "Shotgun":
                return _gameObject.GetComponentInChildren<Shotgun>();
            case "GrenadeLauncher":
                return _gameObject.GetComponentInChildren<GrenadeLauncher>();
            case "RocketLauncher":
                return _gameObject.GetComponentInChildren<RocketLauncher>();
            case "LaserGun":
                return _gameObject.GetComponentInChildren<LaserGun>();
            case "PlasmaRifle":
                return _gameObject.GetComponentInChildren<PlasmaRifle>();
            case "RailGun":
                return _gameObject.GetComponentInChildren<RailGun>();
            default:
                return _gameObject.GetComponentInChildren<Chainsaw>();
        }        
    }
}
