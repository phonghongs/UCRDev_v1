using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class checkpoint : MonoBehaviour
{
    [Header("Points")]
    public GameObject[] start;
    public GameObject[] end;
    public GameObject[] checkpoints;

    public List<List<GameObject>> passedCheckpoint;

    [Header("Settings")]
    public float laps = 1;

    [Header("Information")]
    public float[] currentCheckpoint;
    private float[] currentLap;
    private bool[] started;
    public bool[] finished;
    public float maxTimePerLap;

    public float[] currentLapTime;
    private float[] bestLapTime;
    private float[] bestLap;

    public static checkpoint Instance { get; private set; }

    private GUIStyle guistyle;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        guistyle = new GUIStyle();
        guistyle.fontSize = 14;
        guistyle.fontStyle = FontStyle.Bold;
        guistyle.font = Font.CreateDynamicFontFromOSFont("Liberation Sans", 14);
        guistyle.normal.textColor = Color.white;

        currentCheckpoint = new float[] { 0.0f, 0.0f };
        currentLap = new float[] { 1.0f, 1.0f };

        started = new bool[] { false, false };
        finished = new bool[] { false, false };

        currentLapTime = new float[] { 0.0f, 0.0f };
        bestLapTime = new float[] { 0.0f, 0.0f };
        bestLap = new float[] { 0.0f, 0.0f };

        passedCheckpoint = new List<List<GameObject>>();
        passedCheckpoint.Add(new List<GameObject>());
        passedCheckpoint.Add(new List<GameObject>());
    }

    private void Update()
    {
        for (int i = 0; i < started.Length; i++)
        {
            if (started[i] && !finished[i])
            {
                currentLapTime[i] += Time.deltaTime;
            }

            if (started[i])
            {
                bestLapTime = currentLapTime;
            }
        }
    }

    public void CheckpointPerPlayer(GameObject thisCheckpoint, int playerindex)
    {
        // Started the race
        if (thisCheckpoint == start[playerindex] && !started[playerindex])
        {
            print("Started");
            started[playerindex] = true;
        }
        // Ended the lap or race
        else if (thisCheckpoint == end[playerindex] && started[playerindex])
        {
            // If all the laps are finished, end the race
            if (currentLap[playerindex] == laps)
            {
                if (currentCheckpoint[playerindex] == checkpoints.Length)
                {
                    if (currentLapTime[playerindex] < bestLapTime[playerindex])
                    {
                        bestLap[playerindex] = currentLap[playerindex];
                    }

                    finished[playerindex] = true;
                    print($"Finished: {playerindex}");
                }
                else
                {
                    print("Did not go through all checkpoints");
                }
            }
            // If all laps are not finished, start a new lap
            else if (currentLap[playerindex] < laps)
            {
                if (currentCheckpoint[playerindex] == checkpoints.Length)
                {
                    if (currentLapTime[playerindex] < bestLapTime[playerindex])
                    {
                        bestLap[playerindex] = currentLap[playerindex];
                        bestLapTime[playerindex] = currentLapTime[playerindex]; // Because the update function has already run this frame, we need to add this line or it won't work
                    }

                    currentLap[playerindex]++;
                    currentCheckpoint[playerindex] = 0;
                    currentLapTime[playerindex] = 0;
                    print($"Started lap {currentLap[playerindex]}");
                }
                else
                {
                    print("Did not go through all checkpoints");
                }
            }
        }

        if (finished[playerindex])
            return;
        if (passedCheckpoint[playerindex].Count > 0 && passedCheckpoint[playerindex].Contains(thisCheckpoint))
        {
            return;
        }

        currentCheckpoint[playerindex]++;
        passedCheckpoint[playerindex].Add(thisCheckpoint);

        // Loop through the checkpoints to compare and check which one the player touched
        //for (int i = 0; i < checkpoints.Length; i++)
        //{
        //    if (finished[playerindex])
        //        return;

        //    print($"Correct Checkpoint: {Mathf.FloorToInt(currentLapTime[playerindex] / 60)}:{currentLapTime[playerindex] % 60:00.000}");
        //    currentCheckpoint[playerindex]++;
        //    print(currentCheckpoint[playerindex]);
        //}
    }

    public void OnTriggerCheckpoint(Collider other, GameObject playerobject)
    {
        GameObject thisCheckpoint = other.gameObject;

        for (int i = 0; i < PlayerManager.Instance.getNumPlayer(); i++)
        {
            if (playerobject == PlayerManager.Instance.getPlayer(i).obj)
            {
                CheckpointPerPlayer(thisCheckpoint, i);
                break;
            }
        }
    }


    //private void OnGUI()
    //{
    //    // Current time
    //    string formattedCurrentLapTime = $"Current: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000} - (Lap {currentLap})";
    //    GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentLapTime, guistyle);

    //    // Best time
    //    string formattedBestLapTime = $"Best: {Mathf.FloorToInt(bestLapTime / 60)}:{bestLapTime % 60:00.000} - (Lap {bestLap})";
    //    GUI.Label(new Rect(250, 10, 250, 100), (started) ? formattedBestLapTime : "0:00.000", guistyle);

    //    // Current checkpoint
    //    string formattedcheckpoint = $"Current: {Mathf.FloorToInt(currentCheckpoint)}";
    //    GUI.Label(new Rect(450, 10, 250, 100),formattedcheckpoint, guistyle);

    //    // Checkpoint left
    //    string formattedcheckpointleft = $"Checkpoint left : {(checkpoints.Length) - Mathf.FloorToInt(currentCheckpoint)}";
    //    GUI.Label(new Rect(50, 30, 250, 100), formattedcheckpointleft, guistyle);

    //    string formattedCoin = $"Coins earned : {(CoinsManager.Instance.coinCounting)} - {(CoinsManager.Instance.numCoins)}";
    //    GUI.Label(new Rect(50, 50, 250, 100), formattedCoin, guistyle);

    //    string totalScore = $"Total Score : {CalTotalScore()}";
    //    GUI.Label(new Rect(50, 70, 250, 100), totalScore, guistyle);
    //}

    public string CalTotalScore(int indexPlayer)
    {
        float scoreCoins = ((float)CoinsManager.Instance.coinCounting[indexPlayer]/(float)CoinsManager.Instance.numCoins) * 10;
        float scoreTimes = ((maxTimePerLap - bestLapTime[indexPlayer]) / maxTimePerLap) * 30;
        float scoreCheckpoints = ((currentCheckpoint[indexPlayer]) / checkpoints.Length) * 60;

        if (scoreTimes < 0)
            scoreTimes = 0;

        return $"{scoreCoins + scoreTimes + scoreCheckpoints}";
    }
}
