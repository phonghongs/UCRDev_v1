using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coins : MonoBehaviour
{
    List<GameObject> passedCoin;

    private void Start()
    {
        passedCoin = new List<GameObject>();
    }
    private void Update() {
        if (Input.GetKey("r")){
            if (this.gameObject == PlayerManager.Instance.getPlayer(PlayerManager.Instance.targetPlayer).obj)
            {
                OutLine(PlayerManager.Instance.targetPlayer);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < PlayerManager.Instance.getNumPlayer(); i++)
        {
            if (this.gameObject == PlayerManager.Instance.getPlayer(i).obj)
            {
                if (other.CompareTag("Coin"))
                {
                    if (passedCoin.Count > 0 && passedCoin.Contains(other.gameObject))
                        break;
                    passedCoin.Add(other.gameObject);
                    CoinsManager.Instance.coinCounting[i]++;
                    //Destroy(other.gameObject);
                }

                if (other.CompareTag("CHECKPOINT"))
                {
                    checkpoint.Instance.OnTriggerCheckpoint(other, this.gameObject);
                }

                if (other.CompareTag("Outline"))
                {
                    OutLine(i);
                }

                if (other.CompareTag("CHECKOUT"))
                {
                    PlayerManager.Instance.ResetPlayer(this.gameObject);
                }
                break;
            }
        }
    }

    event Action a;

    private void OutLine(int index){
        checkpoint.Instance.finished[index] = true;
        PlayerManager.Instance.ResetPlayer(this.gameObject);
        MapManager.Instance.OnCompleteMap(index);
    }
}
