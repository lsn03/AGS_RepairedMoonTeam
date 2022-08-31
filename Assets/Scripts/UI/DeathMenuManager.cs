using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeathMenuManager : MonoBehaviour
{
    public GameObject deathPanel;
    public GameObject chooseTeam;
    public static DeathMenuManager Instance;
    [SerializeField] TMP_Text text;
    public string team {  get; private set; }
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void OpenDeathMenu()
    {
        deathPanel.SetActive( true );
        text.text = $"<color=red><b><size=150%>suicide</color></b></size>";
    }
    public void OpenDeathMenu(string playerNick = "default")
    {
        deathPanel.SetActive( true );
        text.text = $"player <color=red><b><size=150%>{playerNick}</color></b></size> killed you";
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
