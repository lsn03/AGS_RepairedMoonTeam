using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenuManager : MonoBehaviour
{
    public GameObject deathPanel;
    public GameObject chooseTeam;
    public static DeathMenuManager Instance;
    public string team {  get; private set; }
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

    public void OpenChooseTeamMenu()
    {
        chooseTeam.SetActive( true );
    }
    public void CloseChooseTeamMenu()
    {
        chooseTeam.SetActive( false );
        Debug.Log( "close" );
    }

    public void ChooseRedTeam()
    {
        team = "red";
    }
    public void ChooseBlueTeam()
    {
        team = "blue";
    }
}
