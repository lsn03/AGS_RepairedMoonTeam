using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePannel;

    // Start is called before the first frame update
    void Start()
    {
        if(pausePannel)
        {
            pausePannel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePannel.SetActive(true);
        }
    }

    public void ResumeButtonClick()
    {
        pausePannel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
