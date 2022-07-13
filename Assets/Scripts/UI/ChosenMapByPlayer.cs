using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChosenMapByPlayer : MonoBehaviour
{
    [SerializeField] TMP_Text name;
    [SerializeField] Image image;
    public static ChosenMapByPlayer Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void SetNameAndImage(string text,Image _image)
    {
        name.text = text;
        image.sprite = _image.sprite;
    }
}
