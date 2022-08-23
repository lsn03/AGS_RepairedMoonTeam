using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    SpawnPoint[] spawnPoints;
    SpawnPoint[] blueSpawnPoints;
    SpawnPoint[] redSpawnPoints;
    SpawnPoint[] deathMatchSpawnPoint;
    int countRed=0,countBlue=0, deathMatchCount = 0;
    private void Awake()
    {
        Instance = this;
       
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        CountRedAndBlueSpawnPoints();
        

    }
    public Transform GetDeathMatchSpawnpoint()
    {
        return deathMatchSpawnPoint[Random.Range( 0, deathMatchSpawnPoint.Length)].transform;
    }
    public Transform GetTeamDeathMatchRedSpawnpoint()
    {
        return redSpawnPoints[Random.Range( 0, redSpawnPoints.Length )].transform;
    }
    public Transform GetTeamDeathMatchBlueSpawnpoint()
    {
        return blueSpawnPoints[Random.Range( 0, blueSpawnPoints.Length )].transform;
    }

    private void CountRedAndBlueSpawnPoints()
    {
        foreach(SpawnPoint item in spawnPoints )
        {
            if ( item.red )
            {
                countRed++;
            }
            else
            if ( item.blue )
            {
                countBlue++;
            }
            else
            {
                deathMatchCount++;
            }
        }
        blueSpawnPoints = new SpawnPoint[countBlue];
        redSpawnPoints = new SpawnPoint[countRed];
        deathMatchSpawnPoint = new SpawnPoint[deathMatchCount];
        int tempRed=0,tempBlue=0,tempNone = 0;
        for (int i = 0; i< spawnPoints.Length;i++ )
        {
            if ( spawnPoints[i].red )
            {
                redSpawnPoints[tempRed] = spawnPoints[i];
                tempRed++;
            }
            else
            if ( spawnPoints[i].blue )
            {
                blueSpawnPoints[tempBlue] = spawnPoints[i];
                tempBlue++;
            }
            else
            {
                deathMatchSpawnPoint[tempNone] = spawnPoints[i];
                tempNone++;
            }
        }



    }
}
