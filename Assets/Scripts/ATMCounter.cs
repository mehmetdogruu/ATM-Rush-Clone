using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ATMCounter : MonoBehaviour
{
    private int count = 0;
    public TextMeshProUGUI counterText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Money")
        {
            count++;
        }
        else if (other.gameObject.tag=="Gold")
        {
            count += 2;
        }
        else if (other.gameObject.tag=="Diamond")
        {
            count += 3;
        }
        counterText.text = count.ToString();
    }
}
