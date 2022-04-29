using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider slider;
    public int colorIndex = 0;
    
    void Start()
    {
        
        slider = gameObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener( delegate { ValueChangeCheck(); } );
    }
    public void ValueChangeCheck()
    {
       Launcher.Instance.ChangePlayerColor( colorIndex, slider.value );
    }

}
