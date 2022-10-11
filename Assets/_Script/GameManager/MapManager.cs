using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    public Transform[] spawnPosition;
    public TextMeshProUGUI TotalScore;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.SetSpawnPosition(spawnPosition);
        TotalScore.gameObject.SetActive(false);
    }

    public void OnCompleteMap()
    {
        TotalScore.gameObject.SetActive(true);
        TotalScore.text = checkpoint.Instance.CalTotalScore();
    }

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
}
