using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject graphic;
    [SerializeField] public bool red = false;
    [SerializeField] public bool blue = false;
    
    private void Awake()
    {
        graphic.SetActive( false );
    }
}
