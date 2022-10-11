using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coins : MonoBehaviour
{
    private void Update() {
        if (Input.GetKey("r")){
            OutLine();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            CoinsManager.Instance.coinCounting ++;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("CHECKPOINT"))
        {
            checkpoint.Instance.OnTriggerCheckpoint(other);
        }

        if (other.CompareTag("Outline")){
            OutLine();
        }

        if (other.CompareTag("CHECKOUT")){
            PlayerManager.Instance.ResetPlayer(this.gameObject);
        }
    }

    event Action a;

    private void OutLine(){
        checkpoint.Instance.finished = true;
        PlayerManager.Instance.ResetPlayer(this.gameObject);
        MapManager.Instance.OnCompleteMap();
    }
}
