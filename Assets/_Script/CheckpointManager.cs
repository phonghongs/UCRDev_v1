using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// using static SocketManager;
public class Checkpoints{
    private int numOfCP;
    private int[,] arrayCheckpointsInit;
    public int getNumOfCP(){
        return numOfCP;
    }
    public int [,] getArrayCheckpointsInit(){
        return getrealNumOfCheckpoints();
    }
    public int? getNumOfCheckpoints(){
        GameObject[] CP_s = GameObject.FindGameObjectsWithTag("CHECKPOINT");
        if(CP_s == null){
            Debug.Log("Please add tag <CHECKPOINT> to checkpoint active");
            return null;
        }
        return CP_s.Length;
    }
    public int[,] getrealNumOfCheckpoints(){
        GameObject[] CP_s = GameObject.FindGameObjectsWithTag("CHECKPOINT");
        if(CP_s == null){
            Debug.Log("Please add tag <CHECKPOINT> to checkpoint active");
            return null;
        }
        int len = CP_s.Length;
        int[,] checkTrueCheckpoint = new int[len, 2];
        for(int i = 0; i < len; i += 1){
            checkTrueCheckpoint[i,0] = i + 1;
        }
        for(int i = 0; i < len ; i += 1){
            foreach(GameObject cp in CP_s){ 
                if(cp.transform.name == (i+1).ToString()){
                    checkTrueCheckpoint[i, 1] += 1;
                }
            }
        }
        /*
        Array checkpoint A [,]
                        ________0_______________1_______________...___________len________
                    0   |       1       |       2       |       ...     |   len - 1     |
                    1   |   size_cp1    |   size_cp2    |   size_cp1    |   size_cp1    |

        len_checkpoint = len x2
        */
        return checkTrueCheckpoint;
    }
    public void CheckInitCheckpoints(){                             //Debug
        int[,] arrayCheckpoints = this.getrealNumOfCheckpoints();
        int len = arrayCheckpoints.Length / 2;
        for(int i = 0; i < len; i += 1){
            for(int j = 0; j < 2; j += 1){
                Debug.Log(arrayCheckpoints[i, j]);
            }
        }
    }
    public bool CheckpointsInvalid(){                               //Debug
        int[,] arrayCheckpoints = getrealNumOfCheckpoints();
        int arrayLen = arrayCheckpoints.Length/2;
        bool mycheck = true; 
        for(int i = 0; i < arrayLen; i += 1){
            if(arrayCheckpoints[i,1] > 1){
                mycheck = false;
                break;
                
            }
        }
        if(mycheck){
            Debug.Log("Checkpoints valid");
        }else{
            Debug.Log("Checkpoints inlavid");
        }
        return mycheck;
    }
    public void FixNameCheckpoints(){
        int[,] arrayCheckpoints = getrealNumOfCheckpoints();
        GameObject[] CP_s = GameObject.FindGameObjectsWithTag("CHECKPOINT");
        if(CP_s == null){ return;}
        int arrayLen = arrayCheckpoints.Length/2;
        bool mycheck = true; 
        for(int i = 0; i < arrayLen; i += 1){
            if(arrayCheckpoints[i,1] > 1){
                mycheck = false;
                break;
                
            }
        }
        if(mycheck){return;}
        // Vehicles Players = new Vehicles{
        // }
        for(int i = 0; i < arrayLen; i += 1){
            if(arrayCheckpoints[i, 1] > 1){
                if(i == 0){
                    // float a = DistanceGameO2GameO(CP_s[i], Players.getPlayers()[]);
                    // float b = DistanceGameO2GameO(CP_s[i+1], Players.getPlayers()[]);
                    // if(b > a){
                    //     CP_s[i+1].transform.name = (i+2).ToString();
                    // }
                }
            }
            // float a =  DistanceGameO2GameO(CP_s[i]);
        }
    }
    public float DistanceGameO2GameO(GameObject A, GameObject B){
        return Vector3.Distance(A.transform.position, B.transform.position);
    }
    private void OnDestroy() {
    }
}
public class Vehicles{
    private SocketManager allManagerPlayer;
    private int numOfVehicles;
    public void setSocket(SocketManager A){
        allManagerPlayer  = A;
    }
    public void setNumPlayers(int A){
        numOfVehicles = A;
    }
    public PlayerShape[] getPlayers(){
        numOfVehicles = allManagerPlayer.getNumPlayer();
        return allManagerPlayer.players;
    }
    public int getNumOfVehicles(){
        return allManagerPlayer.getNumPlayer();
    }
}
public class CheckpointManager : MonoBehaviour
{
    private int numOfCheckpoint;
    private int numOfVehicle;
    private GameObject CheckPoints;
    private SocketManager allManagerPlayer;
    // Start is called before the first frame update
    void Start()
    {   
        Checkpoints A = new Checkpoints();
        A.FixNameCheckpoints();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
