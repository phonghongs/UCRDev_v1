using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CheckpointManager : MonoBehaviour{
    public CheckpointPass CPP;
    public CheckpointOut CPO;
    private void Awake() {
    }
    void Start(){
        int PlayerIndex = 1;
        foreach(GameObject gameobjecttagPlayer in GameObject.FindGameObjectsWithTag("topCar")){
            gameobjecttagPlayer.AddComponent<CheckpointPass>();
            gameobjecttagPlayer.GetComponent<CheckpointPass>().playerIndex = PlayerIndex;
            PlayerIndex += 1;
           
        }
        foreach(GameObject gameobjecttagPlayer in GameObject.FindGameObjectsWithTag("midCar")){
            gameobjecttagPlayer.AddComponent<CheckpointOut>();
        }
    }
}
