using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinemachineControler : MonoBehaviour
{
    public GameObject cine1;
    public GameObject cine2;

    public Camera camSeg;
    public Camera camCine;
    // Start is called before the first frame update
    public Camera cam1;
    public Camera cam2;

    private void Start() {
        cine1.SetActive(false);
        cine2.SetActive(false);
        camSeg.gameObject.SetActive(false);
        camCine.gameObject.SetActive(false);
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log ("displays connected: " + Display.displays.Length);
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            camSeg.gameObject.SetActive(true);
            camCine.gameObject.SetActive(false);
            cam1.gameObject.SetActive(false);
            cam2.gameObject.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            camSeg.gameObject.SetActive(false);
            camCine.gameObject.SetActive(true);
            cam1.gameObject.SetActive(false);
            cam2.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)){
            camCine.gameObject.SetActive(true);
            camSeg.gameObject.SetActive(false);
            cam1.gameObject.SetActive(false);
            cam2.gameObject.SetActive(false);
            cine1.SetActive(true);
            cine2.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)){
            camCine.gameObject.SetActive(true);
            camSeg.gameObject.SetActive(false);
            cam1.gameObject.SetActive(false);
            cam2.gameObject.SetActive(false);
            cine1.SetActive(false);
            cine2.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5)){
            camSeg.gameObject.SetActive(false);
            camCine.gameObject.SetActive(true);
            cam1.gameObject.SetActive(true);
            cam2.gameObject.SetActive(false);
        }
    }
}
