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
    [SerializeField] ScoreBoard endGameScoreBoard;
    [SerializeField] GameManager gameManager;
    [SerializeField] ScoreBoard scoreBoard;
    // GameManager gameManager;
    bool master = false;
    void Awake()
    {
        if ( PhotonNetwork.IsMasterClient )
        {
            currentTimer = maxTimeInMinutes * 60 + maxTimeInSec;
            //Hashtable ht  = new Hashtable() { { "Time", currentTimer } };
            Hastable ht = new Hastable();

            ht.Add( "time", currentTimer );
            PhotonNetwork.CurrentRoom.SetCustomProperties( ht );
            master = true;
            SetCurrentTime( currentTimer );
        }
        else if ( PhotonNetwork.PlayerList.Length > 1 ) 
        {
            Debug.Log( "elseInAwake" );
            SetCurrentTime( currentTimer );

        }
        //Debug.Log( PhotonNetwork.CurrentRoom );
    }
    private void Start()
    {
        if ( !master ) 
        {

            //SetCurrentTime( currentTimer );
        }
        else
        {
            Debug.Log( "elseInStart" );
        }
    }
    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        if ( PhotonNetwork.IsMasterClient )
        {
            SetCurrentTime( currentTimer );
        }
       
    }
    private void SetCurrentTime(float _currTimer) {
        Debug.Log( RpcTarget.All );
        photonView.RPC( "RPC_SetTimeForAllPlayer", RpcTarget.All, _currTimer );
    }
   
    [PunRPC]
    public void RPC_SetTimeForAllPlayer(float _currTimer, PhotonMessageInfo info )
    {
        if ( !PhotonNetwork.IsMasterClient )
        {
            //Debug.Log( PhotonNetwork.LocalPlayer );
            Debug.Log( $"sender is {info.Sender}" );
            currentTimer = _currTimer;
            Debug.Log( currentTimer );
        }
        else
        {

        }
    }
    public override void OnRoomPropertiesUpdate( Hastable propertiesThatChanged )
    {
        object abk;
        if( propertiesThatChanged.ContainsKey( "time" ) )
        {
            propertiesThatChanged.TryGetValue( "time", out abk );
            if(currentTimer< ( float )abk && currentTimer==0 )
                currentTimer = ( float )abk;
        }

    }
    bool flag = false;
    bool isEnd = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!master  && currentTimer<=0)
        {
        //    SetCurrentTime( currentTimer );
        //    flag = true;
            //Debug.Log("time in !master\t"+ currentTimer );
            if ( isEnd )
            {
                endGameCanvas.alpha = 1;
            }
        }
        else
        {
            //Debug.Log("time in else update\t"+ currentTimer );
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
                isEnd = true;
                EndGame();

            }
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
        endGameScoreBoard.IsEndGame();
        scoreBoard.IsEndGame();
        gameManager.IsEndGame();
        PhotonNetwork.AutomaticallySyncScene = false;
       
    }

    
}
