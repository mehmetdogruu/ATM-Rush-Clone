using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("ATM"))
        {
            for (int i = 0; i < Collactable.instance.collectedMoney.Count-1; i++)
            {
                if (Collactable.instance.collectedMoney[i]==other.gameObject)
                {
                    StartCoroutine(Collactable.instance.ATMKeepMoney(other.gameObject, i));
                    break;
                }

            }
        }
        else if (this.gameObject.CompareTag("Thorn"))
        {
            if (other.gameObject.tag=="Money"||other.gameObject.tag=="Gold"||other.gameObject.tag=="Diamond")
            {
                for (int i = 0; i < Collactable.instance.collectedMoney.Count-1; i++)
                {
                    if (Collactable.instance.collectedMoney[i]==other.gameObject)
                    {
                        Collactable.instance.DestroyMoney(other.gameObject, i, this.gameObject);
                        break;
                    }

                }

            }

        }
        else if (this.gameObject.CompareTag("Card") || this.gameObject.CompareTag("Guillotine"))
        {
            if (other.gameObject.tag == "Money" || other.gameObject.tag == "Gold" || other.gameObject.tag == "Diamond")
            {
                for (int i = 0; i < Collactable.instance.collectedMoney.Count - 1; i++)
                {
                    if (Collactable.instance.collectedMoney[i] == other.gameObject)
                    {
                        Collactable.instance.DisperseCollectibles(other.gameObject, i, this.gameObject);
                        break;
                    }
                }
            }
        }
        else if (this.gameObject.CompareTag("Finish"))
        {
            if (other.gameObject.tag == "Money" || other.gameObject.tag == "Gold" || other.gameObject.tag == "Diamond")
            {
                Collactable.instance.FinishCollecterMoney(other.gameObject);
            }
            if (other.gameObject.tag == "Player")
            {
                GameManager.instance.gameState = GameManager.GameState.finished;
            }
        }
    }
}
