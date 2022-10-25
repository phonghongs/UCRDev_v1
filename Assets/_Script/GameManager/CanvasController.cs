using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private playerCanvasControl player1canvas;
    [SerializeField] private playerCanvasControl player2canvas;
    [SerializeField] private GameObject UIScene;
    // Start is called before the first frame update
    void Start()
    {
        FomatPlayer1("", "", "");
        FomatPlayer2("", "", "");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIScene.SetActive(false);
            CameraManager.Instance.TaskOnClick();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIScene.SetActive(true);
        }
    }

    public void FomatPlayer1(string _port, string _checkpoint, string _score)
    {
        player1canvas.SetPlayerUI(
            $"PORT: {_port}",
            $"CHECKPOINT: {_checkpoint}",
            $"TOTAL SCORE: {_score}"
            );
    }
    public void FomatPlayer2(string _port, string _checkpoint, string _score)
    {
        player2canvas.SetPlayerUI(
            $"{_port} : PORT",
            $"{_checkpoint} : CHECKPOINT",
            $"{_score} : TOTAL SCORE"
            );
    }
}
