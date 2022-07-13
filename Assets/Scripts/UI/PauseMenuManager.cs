using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviourPunCallbacks
{
    public GameObject pausePannel;
    public GameObject settingPanel;
    public GameObject gameObjectWithButtons;
    public AudioMixer audioMixerMusic,audioMixerEffect,audioMixerShoot;
    public Slider musicSlider;
    public Slider effectSlider;
    public Slider shootSlider;
    private const string MUSIC_VOLUME = "musicVolume",EFFECT_VOLUME = "effectVolume",SHOOT_VOLUME = "shootVolume";

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

        musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat( MUSIC_VOLUME );
        effectSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat( EFFECT_VOLUME );
        shootSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat( SHOOT_VOLUME );

        audioMixerMusic.SetFloat( MUSIC_VOLUME, PlayerPrefs.GetFloat( MUSIC_VOLUME ) );
        audioMixerEffect.SetFloat( EFFECT_VOLUME, PlayerPrefs.GetFloat( EFFECT_VOLUME ) );
        audioMixerShoot.SetFloat( SHOOT_VOLUME, PlayerPrefs.GetFloat( SHOOT_VOLUME ) );
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!pausePannel.active)
        {
            pausePannel.SetActive(true);
            gameObjectWithButtons.SetActive( true );
        }
        if ( pausePannel.active || settingPanel.active )
        {
            Time.timeScale = 0;
        }
    }
    public void OpenSettings()
    {
        gameObjectWithButtons.SetActive( false );
        settingPanel.SetActive( true );
    }
    public void ResumeButtonClick()
    {
        pausePannel.SetActive(false);
        settingPanel.SetActive( false );
        gameObjectWithButtons.SetActive( true );
        Time.timeScale = 1;
    }

    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetVolumeInMusic(float volume)
    {
        Debug.Log( $"{volume} music and value in slider {musicSlider.GetComponent<Slider>().value} " );
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
