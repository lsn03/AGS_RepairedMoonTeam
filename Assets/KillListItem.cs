using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class GunData
{
    public string name;
    public Sprite image;
}
public class KillListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text killer;
    [SerializeField] Image image;
    [SerializeField] TMP_Text killed;

    
    [SerializeField] float timeBeforeDelete = 4f;
    [SerializeField] GunData[] guns;
   // public Sprite[] sprites;
    //[SerializeField] public SpriteRenderer sprite;

    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        try
        {
            Destroy( gameObject, timeBeforeDelete );
        }
        catch (System.Exception ex )
        {
            Debug.Log( ex.Message );
        }
        
        
    }

    public void Setup(string killer,string nameOfGun,string killed )
    {
        RPC_Setup( killer, nameOfGun, killed );


    }

   
    public void RPC_Setup( string killer, string nameOfGun, string killed )
    {
        this.killer.text = killer;
        LoadImage( nameOfGun );
        this.killed.text = killed;
    }
    public void DestroyObject()
    {
        try
        {
            Destroy( gameObject );
        }catch(System.Exception ex )
        {
            Debug.Log( ex.Message );
        }
        
    }
    public void LoadImage(string nameOfGun)
    {
        foreach ( GunData gun in guns )
        {
            if ( gun.name == nameOfGun )
            {
                image.sprite = gun.image;
            }
        }
    }

    private void FixedUpdate()
    {
        if ( timeBeforeDelete > 0 )
        {

        }
    }

}
