using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerCanvasControl : MonoBehaviour
{
    public TextMeshProUGUI port;
    public TextMeshProUGUI checkpoint;
    public TextMeshProUGUI score;
    // Start is called before the first frame update
    
    public void SetPlayerUI(string _port, string _checkpoint, string _score)
    {
        port.text = _port;
        checkpoint.text = _checkpoint;
        score.text = _score;
    }
}
