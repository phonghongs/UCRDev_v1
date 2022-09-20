using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointOut : MonoBehaviour{
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "CHECKOUT"){
            if(other.gameObject.transform.name == "DONE"){
                Debug.Log("Done");
            }
            if(other.gameObject.transform.name == "OUT"){
                Debug.Log("OUT");
            }
            if(other.gameObject.transform.name == "Right"){
                Debug.Log("DIE");
            }
            if(other.gameObject.transform.name == "Left"){
                Debug.Log("DIE");
            }
        }
    }
}
