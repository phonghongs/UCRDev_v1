using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraManager : MonoBehaviour
{
    public Button changeVehicles;
    public CarFollower camController;
    public GameObject initializeCamera;
    private int playerIndex = 0;
    // public TMP_Text portShow;
    public static CameraManager Instance { get; private set; }

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
    void Start(){
        camController.carTransform = initializeCamera.GetComponent<Transform>();
        // changeVehicles.onClick.AddListener(TaskOnClick);
    }

    void Update(){
        // portShow.text = PlayerManager.Instance.getPlayer(0).controller.carSpeed.ToString();
    }

    public void TaskOnClick(){
        Debug.Log("Vao ne");
        playerIndex += 1;
        if (playerIndex >= PlayerManager.Instance.getNumPlayer()){
            playerIndex = 0;
        }
        PlayerManager.Instance.SetControllerAvtivate(playerIndex);
        camController.carTransform = PlayerManager.Instance.getPlayer(playerIndex).obj.GetComponent<Transform>();
    }
}
