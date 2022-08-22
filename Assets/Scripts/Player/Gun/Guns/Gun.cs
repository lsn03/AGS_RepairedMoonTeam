using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use();

    public GameObject hitEffect;
    public int maxBullets;
    public float timeBetweenShoots;
    protected float timeBeforeShoots;
    public int bulletsLeft;
    public int bulletsToPickUpFirstTime;
    public void SetAddBullet(int addBullet)
    {
        if (bulletsLeft == 0)
        {
            bulletsLeft = bulletsToPickUpFirstTime;
        }
        else if (bulletsLeft > 0)
        {
            bulletsLeft = System.Math.Min(bulletsLeft + addBullet, maxBullets);
        }
    }

}
