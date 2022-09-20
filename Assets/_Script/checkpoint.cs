using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    [Header("Points")]
    public GameObject start;
    public GameObject end;
    public GameObject[] checkpoints;
    

    [Header("Settings")]
    public float laps = 1;

    [Header("Information")]
    private float currentCheckpoint;
    private float currentLap;
    private bool started;
    private bool finished;

    private float currentLapTime;
    private float bestLapTime;
    private float bestLap;

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

        currentCheckpoint = 0;
        currentLap = 1;

        started = false;
        finished = false;

        currentLapTime = 0;
        bestLapTime = 0;
        bestLap = 0;
    }

    private void Update()
    {
        if (started && !finished)
        {
            currentLapTime += Time.deltaTime;
            
            if (bestLap == 0)
            {
                bestLap = 1;
            }
        }

        if (started)
        {
            if (bestLap == currentLap)
            {
                bestLapTime = currentLapTime;
            }
        }
    }

    public void OnTriggerCheckpoint(Collider other)
    {
        GameObject thisCheckpoint = other.gameObject;

        // Started the race
        if (thisCheckpoint == start && !started)
        {
            print("Started");
            started = true;
        }
        // Ended the lap or race
        else if (thisCheckpoint == end && started)
        {
            // If all the laps are finished, end the race
            if (currentLap == laps)
            {
                if (currentCheckpoint == checkpoints.Length)
                {
                    if (currentLapTime < bestLapTime)
                    {
                        bestLap = currentLap;
                    }

                    finished = true;
                    print("Finished");
                }
                else
                {
                    print("Did not go through all checkpoints");
                }
            }
            // If all laps are not finished, start a new lap
            else if (currentLap < laps)
            {
                if (currentCheckpoint == checkpoints.Length)
                {
                    if (currentLapTime < bestLapTime)
                    {
                        bestLap = currentLap;
                        bestLapTime = currentLapTime; // Because the update function has already run this frame, we need to add this line or it won't work
                    }

                    currentLap++;
                    currentCheckpoint = 0;
                    currentLapTime = 0;
                    print($"Started lap {currentLap}");
                }
                else
                {
                    print("Did not go through all checkpoints");
                }
            }
        }

        // Loop through the checkpoints to compare and check which one the player touched
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (finished)
                return;

            // If the checkpoint is correct
            if (thisCheckpoint == checkpoints[i] && i + 1 == currentCheckpoint + 1)
            {
                print($"Correct Checkpoint: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                currentCheckpoint++;
                print(currentCheckpoint);
            }
            // If the checkpoint is incorrect
            else if (thisCheckpoint == checkpoints[i] && i + 1 != currentCheckpoint + 1)
            {
                print($"Incorrect checkpoint");
            }
        }
    }


    private void OnGUI()
    {
        // Current time
        string formattedCurrentLapTime = $"Current: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000} - (Lap {currentLap})";
        GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentLapTime, guistyle);

        // Best time
        string formattedBestLapTime = $"Best: {Mathf.FloorToInt(bestLapTime / 60)}:{bestLapTime % 60:00.000} - (Lap {bestLap})";
        GUI.Label(new Rect(250, 10, 250, 100), (started) ? formattedBestLapTime : "0:00.000", guistyle);

        // Current checkpoint
        string formattedcheckpoint = $"Current: {Mathf.FloorToInt(currentCheckpoint)}";
        GUI.Label(new Rect(450, 10, 250, 100),formattedcheckpoint, guistyle);

        // Checkpoint left
        string formattedcheckpointleft = $"Checkpoint left : {(checkpoints.Length) - Mathf.FloorToInt(currentCheckpoint)}";
        GUI.Label(new Rect(50, 30, 250, 100), formattedcheckpointleft, guistyle);

        string formattedCoin = $"Coins earned : {(CoinsManager.Instance.coinCounting)} - {(CoinsManager.Instance.numCoins)}";
        GUI.Label(new Rect(50, 50, 250, 100), formattedCoin, guistyle);
    }
}