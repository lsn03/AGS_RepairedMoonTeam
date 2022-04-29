using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource lobbyTheme;

    private void Start()
    {
        lobbyTheme.Play();
        Debug.Log( lobbyTheme.clip.length);
    }

}
