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

    
    [SerializeField] float timeBeforeDelete;
    [SerializeField] GunData[] guns;

    [Range(0,10f),SerializeField] float koef;
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
        this.killer.text = killer;
        LoadImage( nameOfGun );
        this.killed.text = killed;
    }
    public void Setup( string killed )
    {
        this.killer.text = "";
        LoadImage( "suicide" );
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
    private float alpha = 1f;
    private void FixedUpdate()
    {
        if ( timeBeforeDelete > 0 || alpha>0)
        {
            float delta = Time.fixedDeltaTime;

            Debug.Log( $"alpha {alpha} timeBeforeDel{timeBeforeDelete} delta {delta}" );
            timeBeforeDelete -= delta;
            alpha -= (delta / timeBeforeDelete/koef);
            image.color = new Color( image.color.r, image.color.g, image.color.b, alpha );
            killer.alpha = alpha;
            killed.alpha = alpha;
        }
        else
        {
            DestroyObject();
        }
    }

}
