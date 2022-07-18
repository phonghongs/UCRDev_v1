using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using System;

public class Checkpoints{
    private int numOfCP;
    private int[,] arrayCheckpointsInit;
    public Checkpoints CP;
    public int getNumOfCP(){
        return arrayCheckpointsInit.Length/2;
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
        arrayCheckpointsInit = this.getrealNumOfCheckpoints();
        int len = arrayCheckpointsInit.Length / 2;
        for(int i = 0; i < len; i += 1){
            for(int j = 0; j < 2; j += 1){
                Debug.Log(arrayCheckpointsInit[i, j]);
            }
        }
    }
    public bool CheckpointsInvalid(){                               //Debug
        arrayCheckpointsInit = getrealNumOfCheckpoints();
        int arrayLen = arrayCheckpointsInit.Length/2;
        bool mycheck = true; 
        for(int i = 0; i < arrayLen; i += 1){
            if(arrayCheckpointsInit[i,1] > 1){
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
        arrayCheckpointsInit = getrealNumOfCheckpoints();
        GameObject[] CP_s = GameObject.FindGameObjectsWithTag("CHECKPOINT");
        if(CP_s == null){ return;}
        int arrayLen = arrayCheckpointsInit.Length/2;
        bool mycheck = true; 
        for(int i = 0; i < arrayLen; i += 1){
            if(arrayCheckpointsInit[i,1] > 1){
                mycheck = false;
                break;
            }
        }
        if(mycheck){return;}

        for(int i = 0; i < arrayLen; i += 1){
            while(arrayCheckpointsInit[i, 1] > 1){
                if(i == 0){
                    float a = DistanceGameO2GameO(CP_s[0], CP_s[2]);
                    float b = DistanceGameO2GameO(CP_s[1], CP_s[2]);
                    if(b<a){
                        CP_s[1].transform.name = (2).ToString();
                    }else{
                        CP_s[0].transform.name = (2).ToString();
                    }
                }else{
                    float a = DistanceGameO2GameO(CP_s[i], CP_s[i-1]);
                    float b = DistanceGameO2GameO(CP_s[i+1], CP_s[i-1]);
                    if(i == (arrayLen - 1)){
                        if(b < a){
                            CP_s[i+1].transform.name = (i+2).ToString();
                        }else{
                            CP_s[i].transform.name = (i+2).ToString();
                        }
                        continue;
                    }
                    if(b > a){
                        CP_s[i+1].transform.name = (i+2).ToString();
                    }else{
                        CP_s[i].transform.name = (i+2).ToString();
                    }
                }
                arrayCheckpointsInit[i, 1] -= 1;
                arrayCheckpointsInit[i+1, 1] += 1;
            }
        }
    }

    public float DistanceGameO2GameO(GameObject A, GameObject B){
        return Vector3.Distance(A.transform.position, B.transform.position);
    }
    
}
public class Vehicles{
    private int numOfVehicles;
    private GameObject[] Vehicle;
    private int[] CheckpointsPass;
    /*
    Vehicle    Car[0]         Car[1]       Car[2]      ...     Car[n]
    Passsed     2               4           1          ...      5
    */
    public void InnitVehicle(int numVe, int[] checkPass, GameObject[] Veh){
        numOfVehicles = numVe;
        CheckpointsPass = checkPass;
        Vehicle = Veh;
    }
    public int[] InitCheckpointsPass(int num){
        int[] T = new int[num];
        return T;
    }
    public void ClearCheckpointsPass(){
        CheckpointsPass = new int[numOfVehicles];
    }
    public int getnumOfVehicles(){
        return numOfVehicles;
    }
    public GameObject[] getVehicles(){
        return Vehicle;
    }
    public GameObject getVehicle(int index){
        return Vehicle[index];
    }
    public int[] getListCheckpointsPass(){
        return CheckpointsPass;
    }
    public int getCheckpointsPass(int index){
        return CheckpointsPass[index];
    }
    public void VehiclePassChecponit(int indexVehicle){
        CheckpointsPass[indexVehicle] += 1;
    }


}
public class CheckpointManager : MonoBehaviour{
    private int numOfCheckpoint;
    private int numOfVehicle;

    private void Awake() {
        Checkpoints A = new Checkpoints();
        A.FixNameCheckpoints();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            if(this.tag == "CHECKPOINT"){

            }else{

            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            if(this.tag == "CHECKPOINT"){
                
            }else {

            }
        }
    }
}
