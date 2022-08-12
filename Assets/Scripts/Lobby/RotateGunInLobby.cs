using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGunInLobby : MonoBehaviour
{
    public GameObject[] gunItems;
    float startPos = 0f;
    float maxPos = 5f, minPos = -5f;
    int popl = 1;
    private void Start()
    {
        var index = Random.Range(0, gunItems.Length);
        gunItems[index].SetActive(true);
        for (int i = 0; i < gunItems.Length; i++)
        {
            if (i != index)
            {
                gunItems[i].SetActive(false);
            }
        }
    }
    private void Update()
    {

        if (startPos > maxPos)
        {

            startPos -= Time.deltaTime;
        }
        if (startPos < minPos)
        {
            startPos += Time.deltaTime;
        }
        transform.Rotate(0f, 0f, startPos);
    }

}
