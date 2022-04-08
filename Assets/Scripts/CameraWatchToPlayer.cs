using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWatchToPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 playerVector;
    [SerializeField] private int speed;
    private void Update()
    {
        playerVector = player.position;
        playerVector.z = -10;
        transform.position = Vector3.Lerp( transform.position, playerVector, speed * Time.deltaTime );
    }
}
