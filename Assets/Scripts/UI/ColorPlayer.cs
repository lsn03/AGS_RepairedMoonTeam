using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlayer : MonoBehaviour
{
    public Color GetColor()
    {
        return GetComponent<SpriteRenderer>().color;
    }
    public void SetColor(Color newColor )
    {
        GetComponent<SpriteRenderer>().color = newColor;
    }
}
