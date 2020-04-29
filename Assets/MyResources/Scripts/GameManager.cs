using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Score Elements")]
    public Text score;
    public Text best;
    public Text tries;
    public Text downs;

    public int scoreVal;
    public int bestVal;
    public int triesVal;
    public int downsVal;

    [Header("Reset")]
    public GameObject player;
    public Vector3 startPos;
    public Spawner spawner;
    public bool AllowedToTackle = true;

    [Header("AI")]
    public int aiState;
    public float newDownsLine;
    private int Difficulty = 1;

    void Start()
    {
        scoreVal = 0;
        triesVal = 0;
        downsVal = 0;

        bestVal = PlayerPrefs.GetInt("BestScore");

        SetAllUIText();

        spawner.SpawnAI(Difficulty, player.GetComponentInChildren<RugbyPlayerController>().movementSpeed);

        player.GetComponentInChildren<RugbyPlayerController>().state = 1;
        aiState = 1;
    }

    private void SetAllUIText()
    {
        score.text = "Score: " + scoreVal.ToString();
        best.text = "Best Score: " + bestVal.ToString();
        tries.text = "Tries: " + triesVal.ToString();
        downs.text = "Down: " + downsVal.ToString();
    }

    private void Update()
    {
        
    }

    public void IncreaseScore(int points)
    {
        scoreVal += points;
        if (scoreVal > bestVal)
        {
            bestVal = scoreVal;
            PlayerPrefs.SetInt("BestScore", bestVal);
        }
        SetAllUIText();
    }

    public void IncreaseTries()
    {
        triesVal++;
        IncreaseScore(3);
        downsVal = 0;
        SetAllUIText();
        NewRound();
    }

    public void NewRound()
    {
        Difficulty++;
        player.transform.position = startPos;
        spawner.DestroyAllAI();
        spawner.SpawnAI(Difficulty,player.GetComponentInChildren<RugbyPlayerController>().movementSpeed);
    }

    public void IncreaseDowns()
    {
        downsVal++;
        SetAllUIText();
        if (downsVal > 4) { GameOver(); }
    }

    public void NewDown()
    {
        //spawner.Reposition();
        IncreaseDowns();
        StartCoroutine(AllowMovement());
        AllowedToTackle = true;
    }

    IEnumerator AllowMovement()
    {
        // tackle animation
        yield return new WaitForSecondsRealtime(2);

        newDownsLine = player.transform.position.z;
        spawner.BenchAllBehindLine(newDownsLine);

        aiState = 2;
        player.GetComponentInChildren<RugbyPlayerController>().AnimReset();
        

        // get up and stretch animation
        yield return new WaitForSecondsRealtime(3);

        player.GetComponentInChildren<RugbyPlayerController>().state = 1;
        aiState = 1;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

}
