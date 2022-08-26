using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnobject : MonoBehaviour
{

    public Transform SpawnPos;
    public GameObject Cube;
    public float TimeSpawn;

    void Start()
    {
        StartCoroutine(SpawnCD());
    }

    // Update is called once per frame
    void Repeat()
    {
        StartCoroutine(SpawnCD());
    }
    IEnumerator SpawnCD()
    {
        yield return new WaitForSeconds(TimeSpawn);
        Instantiate(Cube, SpawnPos.position, Quaternion.identity);
        Repeat();
    }

}
