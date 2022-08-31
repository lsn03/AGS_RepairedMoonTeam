using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoWasKilledManager : MonoBehaviour
{
    public static WhoWasKilledManager Instance;
    [SerializeField] GameObject whoWasKilledItemPrefab;
    [SerializeField] Transform container;
    void Start()
    {
        Instance = this;
    }

    public void SetupKill(string killed)
    {
        WhoWasKilledItem item = Instantiate(whoWasKilledItemPrefab,container).GetComponent<WhoWasKilledItem>();
        item.Setup( killed );
    }
    
}
