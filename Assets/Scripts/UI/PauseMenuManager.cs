using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Hastable = ExitGames.Client.Photon.Hashtable;


public class PauseMenuManager : MonoBehaviourPunCallbacks
{
    public GameObject pausePannel;
    public GameObject settingPanel;
    public GameObject gameObjectWithButtons;
    public AudioMixer audioMixerMusic,audioMixerEffect,audioMixerShoot;
    public Slider musicSlider;
    public Slider effectSlider;
    public Slider shootSlider;

    private const string 
        MUSIC_VOLUME = "musicVolume",
        EFFECT_VOLUME = "effectVolume",
        SHOOT_VOLUME = "shootVolume";

    private void Awake()
    {
        musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat( MUSIC_VOLUME );
        effectSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat( EFFECT_VOLUME );
        shootSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat( SHOOT_VOLUME );

        audioMixerMusic.SetFloat( MUSIC_VOLUME, PlayerPrefs.GetFloat( MUSIC_VOLUME ) );
        audioMixerEffect.SetFloat( EFFECT_VOLUME, PlayerPrefs.GetFloat( EFFECT_VOLUME ) );
        audioMixerShoot.SetFloat( SHOOT_VOLUME, PlayerPrefs.GetFloat( SHOOT_VOLUME ) );

       // Debug.Log( PlayerPrefs.GetFloat( "musicVolume" ));
    }
    void Start()
    {
        if(pausePannel)
        {
            pausePannel.SetActive(false);
        }
   
        
    }
    private bool isOpenMainMenu = false,isOpenSettingMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpenMainMenu = !isOpenMainMenu;
        }
        
        if(isOpenMainMenu || isOpenSettingMenu )
        {
            SetPause();
            //Time.timeScale = 0; 
        }

        if ( isOpenMainMenu && !isOpenSettingMenu )
        {
            pausePannel.SetActive( true );
            gameObjectWithButtons.SetActive( true );
        }else if(!isOpenSettingMenu)
        
        {
            ResumeButtonClick();
        }
    }
    public void OpenSettings()
    {
        gameObjectWithButtons.SetActive( false );
        settingPanel.SetActive( true );
        isOpenSettingMenu = true;

    }
    public void ResumeButtonClick()
    {
        pausePannel.SetActive(false);
        settingPanel.SetActive( false );
        gameObjectWithButtons.SetActive( true );
        RemovePause();
        isOpenSettingMenu = isOpenMainMenu = false;
    }
   
    public void SetPause()
    {
        if ( PhotonNetwork.LocalPlayer.IsLocal )
        {
            Hastable ht = new Hastable();
            ht.Add( "pause", true );
            PhotonNetwork.LocalPlayer.SetCustomProperties( ht );
        }
    }
    public void RemovePause()
    {
        if ( PhotonNetwork.LocalPlayer.IsLocal )
        {
            Hastable ht = PhotonNetwork.LocalPlayer.CustomProperties;
            
            ht.Remove( "pause");
            ht.Add( "pause", false );
            PhotonNetwork.LocalPlayer.SetCustomProperties( ht );
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetVolumeInMusic(float volume)
    {
        //Debug.Log( $"{volume} music and value in slider {musicSlider.GetComponent<Slider>().value} " );
        audioMixerMusic.SetFloat( MUSIC_VOLUME, volume );
        PlayerPrefs.SetFloat( MUSIC_VOLUME, volume );
    }


    public void SetVolumeInEffect( float volume )
    {
        audioMixerEffect.SetFloat( EFFECT_VOLUME, volume );
        PlayerPrefs.SetFloat( EFFECT_VOLUME, volume );
    }
    public void SetVolumeInShoot( float volume )
    {
        audioMixerShoot.SetFloat( SHOOT_VOLUME, volume );
        PlayerPrefs.SetFloat( SHOOT_VOLUME, volume );
    }

}
