using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CollectableMoney"))
        {
            if (!Collactable.instance.collectedMoney.Contains(other.gameObject))
            {
                
                other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                //other.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.003f, 0.005f, 0.001f);
                other.gameObject.tag = "Money";
                other.gameObject.AddComponent<Collision>();
                other.gameObject.AddComponent<Rigidbody>();
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Collactable.instance.StackMoney(other.gameObject, Collactable.instance.collectedMoney.Count-1 );
            }
        }
        if (other.gameObject.CompareTag("CollectableGold"))
        {
            if (!Collactable.instance.collectedMoney.Contains(other.gameObject))
            {

                other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                //other.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.003f, 0.005f, 0.001f);
                other.gameObject.tag = "Gold";
                other.gameObject.AddComponent<Collision>();
                other.gameObject.AddComponent<Rigidbody>();
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Collactable.instance.StackMoney(other.gameObject, Collactable.instance.collectedMoney.Count - 1);

            }
        }
        if (other.gameObject.CompareTag("CollectableDiamond"))
        {
            if (!Collactable.instance.collectedMoney.Contains(other.gameObject))
            {

                other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                //other.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.003f, 0.005f, 0.001f);
                other.gameObject.tag = "Diamond";
                other.gameObject.AddComponent<Collision>();
                other.gameObject.AddComponent<Rigidbody>();
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Collactable.instance.StackMoney(other.gameObject, Collactable.instance.collectedMoney.Count - 1);

            }

        }
    }

}
