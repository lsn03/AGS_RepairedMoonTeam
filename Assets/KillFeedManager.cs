using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KillFeedManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject killListPrefab;
    [SerializeField] Transform container;
    public static KillFeedManager Instance;
    
    int currentCount = 0;
    int maxCount = 4;
    
    void Start()
    {
        
        Instance = this;
    }

    public override void OnRoomPropertiesUpdate( ExitGames.Client.Photon.Hashtable propertiesThatChanged )
    {
        if ( propertiesThatChanged.ContainsKey( "killAnouncer" ) )
        {
            propertiesThatChanged.TryGetValue("killAnouncer",out object obj);
            string[] value = ((string)obj).Split('\t');
            propertiesThatChanged.Remove( "killAnouncer" );
            
            SetUpPlayer(value[0],value[1],value[2]);
        }
    }

    public void SetUpPlayer( string killer, string nameOfGun, string killed )
    {
        currentCount = container.childCount;
        if ( currentCount < maxCount )
        {
            KillListItem list = Instantiate( killListPrefab, container).GetComponent<KillListItem>();


            // KillListItem list = Instantiate(killListPrefab,container).GetComponent<KillListItem>();
            list.Setup( killer, nameOfGun, killed );
            currentCount += 1; 
        }
        else
        {

            foreach ( Transform list in container )
            {
                KillListItem item = list.GetComponent<KillListItem>();
                item.DestroyObject();
                
                currentCount -=1;
                KillListItem list2 = Instantiate( killListPrefab, container).GetComponent<KillListItem>();


                // KillListItem list = Instantiate(killListPrefab,container).GetComponent<KillListItem>();
                list2.Setup( killer, nameOfGun, killed );
                currentCount += 1;
                break;
            }
        }
    }

    
    
    
    
}
