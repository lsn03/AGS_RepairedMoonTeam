using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using Hastable = ExitGames.Client.Photon.Hashtable;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System;
#endif

[System.Serializable]
public class MapData
{
    public string name;
    public int scene;
}

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
    [SerializeField] GameObject chooseMapButton;

    [SerializeField] GameObject player;
    [SerializeField] GameObject redSlider;
    [SerializeField] GameObject greenSlider;
    [SerializeField] GameObject blueSlider;

    [SerializeField] GameObject deathMatchObject;
    [SerializeField] GameObject teamDeathMatchObject;
    [SerializeField] GameObject captureTheFlagObject;

    [SerializeField] GameObject redTeam;
    [SerializeField] GameObject deathMatch;
    [SerializeField] GameObject blueTeam;


    [SerializeField] Transform redTeamPlayerListContent;
    [SerializeField] Transform blueTeamPlayerListContent;

    [SerializeField] GameObject blueButton,redButton;
    private ColorPlayer colorPlayer;
    private float[] colors = {0,0,0};
    Player character;
    public void ExitGame()
    {
        Application.Quit();
    }
    private string gameMode = "deathMatch";
    [SerializeField] private Text userNameText;
    [SerializeField] private InputField UserNameinputField;
    [SerializeField] private int currentNumberMap;
    [SerializeField] private string currentNameMap;
    private void Awake()
    {
        Instance = this;
    }

    public MapData[] map;

    PhotonView photonView;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        Debug.Log( "Connecting to Master" );
        PhotonNetwork.ConnectUsingSettings();
        UseColorSetting();
        // Debug.Log( photonView.Owner);
    }

    void UseColorSetting()
    {
        colorPlayer = player.GetComponent<ColorPlayer>();
        Color startColor = colorPlayer.GetColor();

        colors[0] = float.TryParse( PlayerPrefs.GetString( "color_r" ), out startColor.r ) ? float.Parse( PlayerPrefs.GetString( "color_r" ) ) : 1;
        colors[1] = float.TryParse( PlayerPrefs.GetString( "color_g" ), out startColor.g ) ? float.Parse( PlayerPrefs.GetString( "color_g" ) ) : 1;
        colors[2] = float.TryParse( PlayerPrefs.GetString( "color_b" ), out startColor.b ) ? float.Parse( PlayerPrefs.GetString( "color_b" ) ) : 1;
        //colors[1] = float.Parse( PlayerPrefs.GetString( "color_g" ) );
        //colors[2] = float.Parse( PlayerPrefs.GetString( "color_b" ) );
        SaveColor();



        // Debug.Log( colors[0]+" "+ colors[1] +" "+ colors[2] );

        redSlider.GetComponent<Slider>().value = float.Parse( PlayerPrefs.GetString( "color_r" ) );
        greenSlider.GetComponent<Slider>().value = float.Parse( PlayerPrefs.GetString( "color_g" ) );
        blueSlider.GetComponent<Slider>().value = float.Parse( PlayerPrefs.GetString( "color_b" ) );

        colorPlayer.SetColor( new Color( float.Parse( PlayerPrefs.GetString( "color_r" ) ), float.Parse( PlayerPrefs.GetString( "color_g" ) ), float.Parse( PlayerPrefs.GetString( "color_b" ) ) ) );

    }

    public void ChangePlayerColor( int rgbIndex, float colorFloat )
    {
        colors[rgbIndex] = colorFloat;
        colorPlayer.SetColor( new Color( colors[0], colors[1], colors[2] ) );
    }

    private void SaveColor()
    {


        PlayerPrefs.SetString( "color_r", colors[0].ToString() );
        PlayerPrefs.SetString( "color_g", colors[1].ToString() );
        PlayerPrefs.SetString( "color_b", colors[2].ToString() );

        PhotonNetwork.NickName = PlayerPrefs.GetString( "name" ) + "\t" + PlayerPrefs.GetString( "color_r" ) + "\t" + PlayerPrefs.GetString( "color_g" ) + "\t" + PlayerPrefs.GetString( "color_b" );

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
        RoomOptions roomOptions = new RoomOptions(){MaxPlayers = 8 };
        if ( string.IsNullOrEmpty( roomNameInputField.text ) )
        {

            PhotonNetwork.CreateRoom( "Room " + PhotonNetwork.NickName.Split( "\t" )[0], roomOptions );

            MenuManager.Instance.OpenMenu( "loading" );
        }
        else
        {
            PhotonNetwork.CreateRoom( roomNameInputField.text, roomOptions );

            MenuManager.Instance.OpenMenu( "loading" );
        }

    }
    public override void OnCreatedRoom()
    {
        Hastable ht = new Hastable();
        ht.Add( "gamemode", gameMode );
        PhotonNetwork.CurrentRoom.SetCustomProperties( ht );
        if ( gameMode == "deathMatch" )
        {
            redTeam.SetActive( false );
            deathMatch.SetActive( true );
            blueTeam.SetActive( false );
        }
        else if ( gameMode == "teamDeathMatch" || gameMode == "captureTheFlag" )
        {
            redTeam.SetActive( true );
            deathMatch.SetActive( true );
            blueTeam.SetActive( true );
        }
    }
    private void SaveName()
    {
        if ( string.IsNullOrEmpty( userNameText.text ) )
        {
            PlayerPrefs.SetString( "name", "Player" + UnityEngine.Random.Range( 0, 9999 ) );
        }
        else
        {
            PlayerPrefs.SetString( "name", userNameText.text );
        }

        PhotonNetwork.NickName = PlayerPrefs.GetString( "name" );
    }

    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        Instantiate( PlayerListItemPrefab, PlayerListContent ).GetComponent<PlayerListItem>().SetUp( newPlayer );

        SetGameMode( gameMode );

        SetPlayerListInTeams();
    }
    [PunRPC]
    public void RPC_SetPlayerListInTeams()
    {
        Player[] players = PhotonNetwork.PlayerList;
        string team;
        foreach ( Player player in players )
        {
            player.CustomProperties.TryGetValue( "team", out object obj );
            team = ( string )obj;
            if ( team == "red" )
            {
                if ( !CheckOnEsxist( redTeamPlayerListContent, player ) )
                {
                    foreach(Transform child in PlayerListContent )
                    {
                        if (player == child.GetComponent<PlayerListItem>().GetPlayer() )
                        {
                            Destroy( child.gameObject );
                        }
                    }

                    Instantiate( PlayerListItemPrefab, redTeamPlayerListContent ).GetComponent<PlayerListItem>().SetUp( player );
                }
            } else if ( team == "blue" )
            {

                if ( !CheckOnEsxist( blueTeamPlayerListContent, player ) )
                {
                    foreach ( Transform child in PlayerListContent )
                    {
                        if ( player == child.GetComponent<PlayerListItem>().GetPlayer() )
                        {
                            Destroy( child.gameObject );
                        }
                    }
                    Instantiate( PlayerListItemPrefab, blueTeamPlayerListContent ).GetComponent<PlayerListItem>().SetUp( player );
                }
            }
            Debug.Log( $"{nameof(RPC_SetPlayerListInTeams)} team\t{team}" );
        }
    }
    public void SetPlayerListInTeams()
    {
        photonView.RPC( nameof( RPC_SetPlayerListInTeams ), RpcTarget.All );
    }
    private void ClearListContent()
    {
        foreach ( Transform child in PlayerListContent )
        {
            Destroy( child.gameObject );
        }
        foreach ( Transform child in redTeamPlayerListContent )
        {
            Destroy( child.gameObject );
        }
        foreach ( Transform child in blueTeamPlayerListContent )
        {
            Destroy( child.gameObject );
        }
    }

    private void SetupGameModeInRoom()
    {
        redButton.SetActive( true );
        blueButton.SetActive( true );
        Hastable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        
        object obj;
        ht.TryGetValue( "gamemode", out obj );

        

        if ( ( string )obj == "deathMatch" )
        {
            redTeam.SetActive( false );
            deathMatch.SetActive( true );
            blueTeam.SetActive( false );
        }
        else if ( ( string )obj == "teamDeathMatch" || ( string )obj == "captureTheFlag" )
        {

            redTeam.SetActive( true );
            deathMatch.SetActive( true );
            blueTeam.SetActive( true );
        }
    }
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu( "room" );
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;

        ClearListContent();


        Debug.Log( "OnJoinedRoom" );
        for ( int i = 0; i < players.Length; i++ )
        {
                Instantiate( PlayerListItemPrefab, PlayerListContent ).GetComponent<PlayerListItem>().SetUp( players[i] );
           
                
        }

        SetupGameModeInRoom();
        SetPlayerListInTeams();

        chooseMapButton.SetActive( PhotonNetwork.IsMasterClient );
        startGameButton.SetActive( PhotonNetwork.IsMasterClient );
    }

    public override void OnMasterClientSwitched( Player newMasterClient )
    {
        chooseMapButton.SetActive( PhotonNetwork.IsMasterClient );
        startGameButton.SetActive( PhotonNetwork.IsMasterClient );
    }

    public override void OnCreateRoomFailed( short returnCode, string message )
    {
        errorText.text = "Room Creation Falied: " + message;
        MenuManager.Instance.OpenMenu( "error" );
    }

    public void LeaveRoom()
    {
        
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue( "team", out object obj );
        string team = (string)obj;

        
        PhotonNetwork.LeaveRoom();
        currentTeam = "";
        
        //gameMode = "deathMatch";
        MenuManager.Instance.OpenMenu( "loading" );
    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu( "title" );
        Hastable ht = new Hastable();
        ht.Add( "team", null );
        PhotonNetwork.LocalPlayer.SetCustomProperties( ht );
    }

    public override void OnRoomListUpdate( List<RoomInfo> roomList )
    {
        foreach ( Transform trans in roomListContent )
        {
            Destroy( trans.gameObject );
        }
        for ( int i = 0; i < roomList.Count; i++ )
        {
            if ( roomList[i].RemovedFromList )
                continue;
            Instantiate( roomListItemPrefab, roomListContent ).GetComponent<RoonListItem>().SetUp( roomList[i] );
        }
    }
    public void JoinRoom( RoomInfo info )
    {
        SaveName();
        SaveColor();
        PhotonNetwork.JoinRoom( info.Name );
        MenuManager.Instance.OpenMenu( "loading" );


    }
    public void LeaveSelector()
    {
        MenuManager.Instance.OpenMenu( "room" );
        deathMatchObject.SetActive( false );
        teamDeathMatchObject.SetActive( false );
        captureTheFlagObject.SetActive( false );
    }
   
    public void SetMap( int number, string name )
    {
        this.currentNameMap = name;
        this.currentNumberMap = number;
    }

    public void ChangeMap()
    {
        MenuManager.Instance.OpenMenu( "mapSelector" );
        if ( gameMode == "deathMatch" )
        {
            deathMatchObject.SetActive( true );
            teamDeathMatchObject.SetActive( false );
            captureTheFlagObject.SetActive( false );
        }
        else if ( gameMode == "teamDeathMatch" )
        {
            deathMatchObject.SetActive( false );
            teamDeathMatchObject.SetActive( true );
            captureTheFlagObject.SetActive( false );
        }
        else if ( gameMode == "captureTheFlag" )
        {
            deathMatchObject.SetActive( false );
            teamDeathMatchObject.SetActive( false );
            captureTheFlagObject.SetActive( true );
        }
    }

    public void StartGame()
    {
        MenuManager.Instance.OpenMenu( "loading" );

        if ( currentNumberMap == 0 )
        {
            // PhotonNetwork.LoadLevel( 2 );


            PhotonNetwork.LoadLevel( UnityEngine.Random.Range( 1, map.Length ) );
        }
        else
        {
            PhotonNetwork.LoadLevel( currentNumberMap );
        }

    }

    public void SetGameMode(string _gamemode)
    {
        
        photonView.RPC( nameof(RPC_SetGameMode), RpcTarget.All, _gamemode );
    }
    [PunRPC]
    public void RPC_SetGameMode(string _gamemode )
    {
        Debug.Log( "rpc_setGameMode" + "\t" + _gamemode );
        gameMode = _gamemode;
    }
    public void ChooseGameMode( int value )
    {
        if ( value == 0 )
        {
            gameMode = "deathMatch";
        }
        else if ( value == 1 )
        {
            gameMode = "teamDeathMatch";
        }
        else if ( value == 2 )
        {
            gameMode = "captureTheFlag";
        }

    }
    string currentTeam = "";
    public void ChooseRedTeam()
    {
        Debug.Log( currentTeam );
        Player currentPlayer;
        if ( currentTeam == "" )
        {
            foreach ( Transform child in PlayerListContent )
            {

                currentPlayer = child.GetComponent<PlayerListItem>().GetPlayer();
                if ( currentPlayer == PhotonNetwork.LocalPlayer )
                {

                    Destroy( child.gameObject );
                    Debug.Log( "ChooseRed" );
                    GameObject go =  Instantiate( PlayerListItemPrefab, redTeamPlayerListContent );
                    go.GetComponent<PlayerListItem>().SetUp( currentPlayer );
                    go.GetComponent<PlayerListItem>().SetTeam( "red" );
                    ////UpdateForAllPlayer( redTeamPlayerListContent, blueTeamPlayerListContent, PlayerListContent );
                    //UpdateForAllPlayer( currentPlayer );
                    SetPlayerListInTeams();
                    currentTeam = "red";
                    redButton.SetActive( false );
                    blueButton.SetActive( false );
                    break;
                }
            }
        }
            
    }
    
    public void ChooseBlueTeam()
    {
        Player currentPlayer;
        if ( currentTeam == "" )
        {
            foreach ( Transform child in PlayerListContent )
            {
                currentPlayer = child.GetComponent<PlayerListItem>().GetPlayer();
                if ( PhotonNetwork.LocalPlayer == currentPlayer )
                {
                    Destroy( child.gameObject );
                    // Debug.Log( nameof( ChooseBlueTeam )+ " choose" );
                    GameObject go = Instantiate( PlayerListItemPrefab, blueTeamPlayerListContent );
                    go.GetComponent<PlayerListItem>().SetUp( currentPlayer );
                    go.GetComponent<PlayerListItem>().SetTeam( "blue" );
                    SetPlayerListInTeams();
                    //UpdateForAllPlayer( currentPlayer );
                    currentTeam = "blue";
                    redButton.SetActive( false );
                    blueButton.SetActive( false );

                    break;
                }
            }
        }
            
        
       
        
    }

    public void UpdateForAllPlayer( Player _player )
    {
        photonView.RPC( "RPC_UpdateForAllPlayer", RpcTarget.All, _player );
    }
    [PunRPC]
    private void RPC_UpdateForAllPlayer( Player _player )
    {
       
        object team;
        Player currentPlayer;
        foreach ( Transform child in PlayerListContent )
        {
            currentPlayer = child.GetComponent<PlayerListItem>().GetPlayer();
          
            if ( currentPlayer == _player ) 
            {
              
                Hastable ht = new Hastable();
                ht = currentPlayer.CustomProperties;

                ht.TryGetValue( "team", out team );
                Destroy( child.gameObject );
               
                if ( ( string )team == "blue" )
                {
                    if(!CheckOnEsxist( blueTeamPlayerListContent, currentPlayer ) )
                    {
                        Debug.Log( "rpc_blueTeam" );
                        Instantiate( PlayerListItemPrefab, blueTeamPlayerListContent ).GetComponent<PlayerListItem>().SetUp( currentPlayer );
                    }
                        
                }
                else if ( ( string )team == "red" )
                {
                   if( !CheckOnEsxist( redTeamPlayerListContent,currentPlayer ) )
                    {
                        Instantiate( PlayerListItemPrefab, redTeamPlayerListContent ).GetComponent<PlayerListItem>().SetUp( currentPlayer );

                        Debug.Log( "rpcUpdateRed" );

                    }


                }
               
                return;
            }

        }

    }
    private void Update()
    {
       // PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue( "team", out object obj );

       // Debug.Log( "obj is \t "+obj+"\t"+ string.IsNullOrEmpty((string)obj)  );
    }

    public bool CheckOnEsxist(Transform team,Player _player)
    {
        foreach( Transform child in team )
        {
            PlayerListItem item =child.GetComponent<PlayerListItem>();
            Player currPlayer = item.GetPlayer();
            if(_player == currPlayer )
            {
                return true;
            }
        }
        return false;
    }
}
/*
 * сделать синхранизхацию для новоприбывших людей, чтобы всех не пихало в изначальный playerList
 *
 */