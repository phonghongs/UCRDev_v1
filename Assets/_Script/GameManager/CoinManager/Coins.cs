using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            CoinsManager.Instance.coinCounting ++;
            Destroy(other.gameObject);
        }

        // if (other.CompareTag("CHECKPOINT"))
        // {
        //     checkpoint.Instance.OnTriggerCheckpoint(other);
        // }

        if (other.CompareTag("Outline")){
            PlayerManager.Instance.ResetPlayer(this.gameObject);
        }
    }
}
