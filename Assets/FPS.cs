using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FPS : MonoBehaviour
{
    public Text thisFPS;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        thisFPS.text = (1.0/Time.deltaTime).ToString();
    }
}
