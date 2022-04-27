using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip Chainsaw_attack, Chainsaw_idle2,
                            Character_death,
                            Health_pack,
                            Laser_loop,
                            Metal_run,
                            Repair_Armor,
                            Rifle_shot,
                            Rocket_explosion,
                            Rocket_launch;
    static AudioSource audioSource;
    void Start()
    {
        Chainsaw_attack = Resources.Load<AudioClip>( "Chainsaw_attack" );
        Chainsaw_idle2 = Resources.Load<AudioClip>( "Chainsaw_idle2" );
        Character_death = Resources.Load<AudioClip>( "Character_death" );
        Health_pack = Resources.Load<AudioClip>( "Health_pack" );
        Laser_loop = Resources.Load<AudioClip>( "Laser_loop" );
        Metal_run = Resources.Load<AudioClip>( "Metal_run" );
        Repair_Armor = Resources.Load<AudioClip>( "Repair_Armor" );
        Metal_run = Resources.Load<AudioClip>( "Metal_run" );
        Rifle_shot = Resources.Load<AudioClip>( "Rifle_shot" );
        Rocket_explosion = Resources.Load<AudioClip>( "Rocket_explosion" );
        Rocket_launch = Resources.Load<AudioClip>( "Rocket_launch" );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void PlaySound(string clip)
    {
        switch ( clip )
        {
            case "Chainsaw_attack":
                audioSource.PlayOneShot( Chainsaw_attack );
                break;
        }
    }
}
