using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class Player{
    public string Name;
    public int CheckpointPassed;
    public int TimeSec;
    public int Coin;
    public int HourPassed;
    public int MunitesPassed;
    public int SecondsPassed;
    public int PerSecondsPassed;
}
public class ExportData : MonoBehaviour
{
    // public string filename = "";
    // public UIPointManager TimeUI;  

    void Start(){
        // filename = Application.dataPath + "/data.xlsx";
    }
    // void Update(){
    //     if(Input.GetKeyDown(KeyCode.Space)){
    //         WriteXLSX();
    //     }
    // }
    // public void WriteXLSX(){
    //     TextWrite tw = new StreamWriter(filename, flase);
    //     tw.WriteLine("Name, Checkpoint, TimeSec, Coin, H, M, S, P");
    //     tw.Close();

    // }
    // public void RecordEvent(){
    //     TextWrite tw = new StreamWriter(filename, flase);
    //     tw.WriteLine("Name, Checkpoint, TimeSec, Coin, H, M, S, P");
    //     tw.Close();
    // }
}
