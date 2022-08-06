using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using System.Linq;
using TMPro;
using Hastable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviourPunCallbacks
{
    [SerializeField] float maxTimeInMinutes;
    [SerializeField] float maxTimeInSec;
    [SerializeField] TMP_Text time;
    [SerializeField] CanvasGroup endGameCanvas;
    float currentTimer;
    [SerializeField] ScoreBoard scoreBoard;
    [SerializeField] GameManager gameManager;
   // GameManager gameManager;
    bool master = false;
    void Awake()
    {
        if ( PhotonNetwork.IsMasterClient )
        {
            currentTimer = maxTimeInMinutes*60+maxTimeInSec;
            //Hashtable ht  = new Hashtable() { { "Time", currentTimer } };
            Hastable ht = new Hastable();

            ht.Add( "time", currentTimer );
            PhotonNetwork.CurrentRoom.SetCustomProperties( ht );
            master = true;

        }
        else
        {
            //currentTimer = ( float )PhotonNetwork.CurrentRoom.CustomProperties["time"];
        }
        //Debug.Log( PhotonNetwork.CurrentRoom );
    }
    private void Start()
    {
        if ( !master && PhotonNetwork.PlayerList.Length == 1 ) 
        {
            // Debug.Log( PhotonNetwork.CurrentRoom.CustomProperties );
            currentTimer = ( float )PhotonNetwork.CurrentRoom.CustomProperties["time"];
        }
    }
    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        if(PhotonNetwork.LocalPlayer == newPlayer )
        {
            
            
        }
       

        //Debug.Log( $"OnPlayerEneteredRoom {currentTimer}" );
        //Debug.Log( PhotonNetwork.CurrentRoom.CustomProperties );
    }
    public override void OnRoomPropertiesUpdate( Hastable propertiesThatChanged )
    {
        object abk;
        if ( propertiesThatChanged.TryGetValue( "time", out abk ) )
        {
            currentTimer = (float)abk;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        currentTimer -= Time.fixedDeltaTime;
        Hastable ht = new Hastable();
        if ( PhotonNetwork.IsConnected == false )
             ht = PhotonNetwork.CurrentRoom.CustomProperties;
        ht.Remove( "time" );
        ht.Add( "time", currentTimer );

        if ( PhotonNetwork.IsConnected == false )
            PhotonNetwork.CurrentRoom.SetCustomProperties( ht );


        string min = ((int) (currentTimer / 60)).ToString("00");
        time.text = $"{min}:{( currentTimer % 60 ).ToString( "00" )}";
        //Debug.Log( min +"\t"+currentTimer );
        if ( currentTimer <= 0 )
        {
            EndGame();
            
        }
    }
    void UpdateTimer()
    {
        
        
    }
    private void EndGame()
    {
        if ( PhotonNetwork.IsMasterClient )
        {
            PhotonNetwork.DestroyAll();
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        time.gameObject.SetActive( false );
        endGameCanvas.alpha = 1;
        scoreBoard.IsEndGame();
        gameManager.IsEndGame();
        PhotonNetwork.AutomaticallySyncScene = false;
       
    }

    
}
