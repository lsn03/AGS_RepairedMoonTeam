using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Photon.Pun;

public class SoundManager : MonoBehaviourPunCallbacks
{
    [SerializeField] AudioSource[] gamesound;
    //[Range(0,1f),SerializeField] float volumeMusic = 0.5f;
    int index;
    public AudioMixer audioMixerMusic;
    private const string MUSIC_VOLUME = "musicVolume";
    private PhotonView photonView;
    private void Awake()
    {
        //audioMixerMusic.SetFloat( MUSIC_VOLUME, PlayerPrefs.GetFloat( MUSIC_VOLUME ) );
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if ( !photonView.IsMine ) return;
        audioMixerMusic.SetFloat( MUSIC_VOLUME, PlayerPrefs.GetFloat( MUSIC_VOLUME ) );
        index = Random.Range( 0, gamesound.Length );
      //  gamesound[index].volume = volumeMusic;
        gamesound[index].Play();
    }
    void CheckOnPlay()
    {
        if ( !gamesound[index].isPlaying )
        {
            index = Random.Range( 0, gamesound.Length );
        //    gamesound[index].volume = volumeMusic;
            gamesound[index].Play();
           // Debug.Log( "play" );
        }
       

    }

    private void Update()
    {
        if ( !photonView.IsMine ) return;
        //Debug.Log( index +" " + gamesound[index].clip.name );
        // audioMixerMusic.SetFloat( "volume", PlayerPrefs.GetFloat( "musicVolume" ) );
        // Debug.Log("music volume "+ PlayerPrefs.GetFloat( "musicVolume" ) );
        CheckOnPlay();

        
    }

}
