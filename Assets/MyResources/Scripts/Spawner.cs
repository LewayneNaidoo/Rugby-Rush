using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject AIPlayer;
    public GameObject[] spawnPoints;
    public List<GameObject> currentAI;

    private void Start()
    {
        AIPlayer.GetComponentInChildren<RugbyPlayerAIController>().target = GameObject.FindGameObjectWithTag("Player");
    }

    public void SpawnAI(int difficulty, float speed)
    {
        for (int i = 0; i < difficulty; i++)
        {
            AIPlayer.GetComponentInChildren<RugbyPlayerAIController>().movementSpeed = Random.Range(speed - 2, speed + 2);
            currentAI.Add(Instantiate(AIPlayer, spawnPoints[Random.Range(0,spawnPoints.Length)].transform.position, Quaternion.identity));
        }

    }

    public void DestroyAllAI()
    {
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("AI");
        //foreach (GameObject enemy in enemies)
        //    GameObject.Destroy(enemy);

        foreach (GameObject g in currentAI)
        {
            Destroy(g.gameObject);
        }

        currentAI.Clear();

    }

    public void Reposition()
    {
        foreach (GameObject g in currentAI)
        {
            g.transform.position = spawnPoints[0].transform.position;
        }
    }

    public void BenchAllBehindLine(float z)
    {
        foreach (GameObject g in currentAI)
        {
            if (g.transform.position.z < z-3)
            {
                g.GetComponentInChildren<RugbyPlayerAIController>().benched = true;
            }
        }
    }

}
