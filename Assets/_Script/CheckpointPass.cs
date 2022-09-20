using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPass : MonoBehaviour{
    // public void Start(){
    //     foreach(GameObject gameobjecttagPlayer in GameObject.FindGameObjectsWithTag("topCar")){
    //         gameobjecttagPlayer.AddComponent<UIPointManager>();
    //     }
    // }
    public int numCheckpointPassed = -1;
    public int playerIndex = 0;
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "CHECKPOINT"){
            if(numCheckpointPassed != -1){
                numCheckpointPassed += 1;
            }else{
                if(other.gameObject.transform.name == "START"){
                    numCheckpointPassed = 0;
                }
            }            
        }
    }
}
