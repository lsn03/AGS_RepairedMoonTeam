using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System;
#endif

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    [SerializeField] GameObject player;
    [SerializeField] GameObject redSlider;
    [SerializeField] GameObject greenSlider;
    [SerializeField] GameObject blueSlider;
    private ColorPlayer colorPlayer;
    private float[] colors = {0,0,0};

    public void ExitGame()
    {
        Application.Quit();
    }

    [SerializeField] private Text userNameText;
    [SerializeField] private InputField UserNameinputField;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log( "Connecting to Master" );
        PhotonNetwork.ConnectUsingSettings();
        UseColorSetting();
    }

    void UseColorSetting()
    {
        colorPlayer = player.GetComponent<ColorPlayer>();
        Color startColor = colorPlayer.GetColor();
       
        colors[0] = float.TryParse( PlayerPrefs.GetString( "color_r" ), out startColor.r )? float.Parse( PlayerPrefs.GetString( "color_r" )): 1;
        colors[1] = float.TryParse( PlayerPrefs.GetString( "color_g" ), out startColor.g ) ? float.Parse( PlayerPrefs.GetString( "color_g" ) ) : 1;
        colors[2] = float.TryParse( PlayerPrefs.GetString( "color_b" ), out startColor.b ) ? float.Parse( PlayerPrefs.GetString( "color_b" ) ) : 1;
        //colors[1] = float.Parse( PlayerPrefs.GetString( "color_g" ) );
        //colors[2] = float.Parse( PlayerPrefs.GetString( "color_b" ) );
        SaveColor();



        Debug.Log( colors[0]+" "+ colors[1] +" "+ colors[2] );

        redSlider.GetComponent<Slider>().value = float.Parse( PlayerPrefs.GetString( "color_r" ) );
        greenSlider.GetComponent<Slider>().value = float.Parse( PlayerPrefs.GetString( "color_g" ) );
        blueSlider.GetComponent<Slider>().value = float.Parse( PlayerPrefs.GetString( "color_b" ) );

        colorPlayer.SetColor( new Color( float.Parse( PlayerPrefs.GetString( "color_r" ) ), float.Parse( PlayerPrefs.GetString( "color_g" ) ), float.Parse( PlayerPrefs.GetString( "color_b" ) ) ) );

    }
    //private void Update()
    //{
    //    Debug.Log( colors[0]+"\t"+ colors[1]+"\t" + colors[2] );
    //}
    public void ChangePlayerColor(int rgbIndex,float colorFloat )
    {
        colors[rgbIndex] = colorFloat;
        colorPlayer.SetColor( new Color( colors[0], colors[1], colors[2] ) );
    }

    private void SaveColor()
    {


        PlayerPrefs.SetString( "color_r", colors[0].ToString() );
        PlayerPrefs.SetString( "color_g", colors[1].ToString() );
        PlayerPrefs.SetString( "color_b", colors[2].ToString() );

        PhotonNetwork.NickName = PlayerPrefs.GetString( "name" ) + "\t"+ PlayerPrefs.GetString( "color_r" ) +"\t" + PlayerPrefs.GetString( "color_g" ) +"\t" + PlayerPrefs.GetString( "color_b" );
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log( "Connect to Master" );
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu( "title" );
        Debug.Log( "Joined to lobby" );
        UserNameinputField.text = PlayerPrefs.GetString( "name" );
        PhotonNetwork.NickName = userNameText.text;
    }

    public void CreateRoom()
    {
        SaveName();
        SaveColor();
        if ( string.IsNullOrEmpty( roomNameInputField.text ) )
        {
            PhotonNetwork.CreateRoom( "Room "+UnityEngine.Random.Range(0,9999) );
            MenuManager.Instance.OpenMenu( "loading" );
        }
        else
        {
            PhotonNetwork.CreateRoom( roomNameInputField.text );
            MenuManager.Instance.OpenMenu( "loading" );
        }

    }
    private void SaveName()
    {
        if( string.IsNullOrEmpty( userNameText.text ) )
        {
            PlayerPrefs.SetString( "name", "Player"+ UnityEngine.Random.Range( 0, 9999 ) );
        }
        else
        {
            PlayerPrefs.SetString( "name", userNameText.text );
        }
        
        PhotonNetwork.NickName = PlayerPrefs.GetString( "name" );
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu( "room" );
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in PlayerListContent )
        {
            Destroy( child.gameObject );
        }

        for ( int i = 0; i < players.Length; i++ )
        {
            Instantiate( PlayerListItemPrefab, PlayerListContent ).GetComponent<PlayerListItem>().SetUp( players[i] );
        }
        startGameButton.SetActive( PhotonNetwork.IsMasterClient );
    }

    public override void OnMasterClientSwitched( Player newMasterClient )
    {
        startGameButton.SetActive( PhotonNetwork.IsMasterClient );
    }

    public override void OnCreateRoomFailed( short returnCode, string message )
    {
        errorText.text = "Room Creation Falied: "+message;
        MenuManager.Instance.OpenMenu( "error" );
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu( "loading" );
    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu( "title" );
    }

    public override void OnRoomListUpdate( List<RoomInfo> roomList )
    {
        foreach (Transform trans in roomListContent )
        {
            Destroy( trans.gameObject );
        }
        for(int i = 0; i< roomList.Count;i++ )
        {
            if ( roomList[i].RemovedFromList )
                continue;
            Instantiate( roomListItemPrefab, roomListContent ).GetComponent<RoonListItem>().SetUp( roomList[i] );
        }
    }
    public void JoinRoom(RoomInfo info )
    {
        SaveName();
        SaveColor();
        PhotonNetwork.JoinRoom( info.Name );
        MenuManager.Instance.OpenMenu( "loading" );


    }
    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        Instantiate( PlayerListItemPrefab, PlayerListContent ).GetComponent<PlayerListItem>().SetUp(newPlayer );

    }

    public void StartGame()
    {
        MenuManager.Instance.OpenMenu( "loading" );
        PhotonNetwork.LoadLevel( 1 );
    }

}
