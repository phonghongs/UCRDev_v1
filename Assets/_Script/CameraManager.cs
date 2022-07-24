using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Button changeVehicles;
    public CarFollower camController;
    public GameObject initializeCamera;
    private int playerIndex = 0;

    void Start(){
        camController.carTransform = initializeCamera.GetComponent<Transform>();
        changeVehicles.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick(){
        playerIndex += 1;
        if (playerIndex >= PlayerManager.Instance.getNumPlayer()){
            playerIndex = 0;
        }
        PlayerManager.Instance.SetControllerAvtivate(playerIndex);
        camController.carTransform = PlayerManager.Instance.getPlayer(playerIndex).obj.GetComponent<Transform>();
    }
}
