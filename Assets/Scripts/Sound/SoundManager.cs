using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource[] gamesound;
    [Range(0,1f),SerializeField] float volumeMusic = 0.5f;
    int index;

    private void Start()
    {
        index = Random.Range( 0, gamesound.Length );
        gamesound[index].volume = volumeMusic;
        gamesound[index].Play();
    }
    void CheckOnPlay()
    {
        if ( !gamesound[index].isPlaying )
        {
            index = Random.Range( 0, gamesound.Length );
            gamesound[index].volume = volumeMusic;
            gamesound[index].Play();
            Debug.Log( "play" );
        }
       

    }

    private void Update()
    {
        //Debug.Log( index +" " + gamesound[index].clip.name );

        CheckOnPlay();

        
    }

}
