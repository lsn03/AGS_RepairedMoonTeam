using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhoWasKilledItem : MonoBehaviour
{
    [SerializeField] float timeBeforeDelete;
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        Destroy( gameObject, timeBeforeDelete );
    }

    public void Setup(string killed)
    {
        text.text = $"you killed {killed}";
    }

}
