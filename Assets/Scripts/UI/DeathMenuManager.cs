using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuManager : MonoBehaviour
{
    public GameObject deathPanel;
    public static DeathMenuManager Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void OpenDeathMenu()
    {
        deathPanel.SetActive( true );
    }
    public void CloseDeathMenu()
    {
        deathPanel.SetActive( false );
    }
}
