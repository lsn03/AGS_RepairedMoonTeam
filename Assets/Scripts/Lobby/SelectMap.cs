using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectMap : MonoBehaviour
{
    [SerializeField] TMP_Text name;
    [SerializeField] TMP_Text number;
    [SerializeField] Image image;
    public void SelectButton()
    {
        Launcher.Instance.SetMap( int.Parse( number.text), name.text );
        ChosenMapByPlayer.Instance.SetNameAndImage( name.text, image );
    }
  
}
