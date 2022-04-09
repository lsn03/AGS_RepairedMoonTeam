using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public float offset;
    public GameObject Ammo;
    public Transform shotDir;
    public GameObject Gun;
    public float startTime;
    private float timeShot;
    [SerializeField] float minAxis;
    [SerializeField] float maxAxis;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if ( !photonView.IsMine ) return;
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        float rotateZ = Mathf.Atan2(difference.y,difference.x)*Mathf.Rad2Deg;
        if ( ( rotateZ + offset ) > maxAxis )
        {
            Gun.transform.rotation = Quaternion.Euler( 0f, 0f, maxAxis );
        }
        else if( ( rotateZ + offset ) < minAxis )
        {
            Gun.transform.rotation = Quaternion.Euler( 0f, 0f, minAxis );
        }
        else
        {
            Gun.transform.rotation = Quaternion.Euler( 0f, 0f, rotateZ + offset );
        }
        

        if ( timeShot <= 0 )
        {
            if ( Input.GetMouseButton( 0 ) )
            {
                PhotonNetwork.Instantiate( Ammo.name, shotDir.position, Gun.transform.rotation );
                timeShot = startTime;
            }
        }
        else
        {
            timeShot -= Time.deltaTime;
        }

        
    }
}
