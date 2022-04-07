using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Weapon gun;
    public float damage;
    public void Start()
    {
        damage = gun.damage;
        damage = 75f;
    }
}
