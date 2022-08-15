using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticRifleBullet : BulletsManager
{
    [SerializeField] public float reloadTime;
    public override void Use()
    {

    }
    private void Update()
    {
        Activation(reloadTime);
    }
    // Start is called before the first frame update
    void Start()
    {
        lostTime = reloadTime;
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {

            Debug.Log("getLazerBullets");
            player.gameObject.GetComponentInChildren<AutomaticShot>().AddBullet(bulletToAdd);


            itemGameObject.SetActive(false);
            collider.enabled = false;
        }

    }
}
